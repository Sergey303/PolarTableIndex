using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace GoIndex
{
    public class IndexKeyView<Tkey> where Tkey : IComparable
    {
        internal PaEntry table;
        private PaCell index_cell;
        internal Func<PaEntry, Tkey> keyProducer;
        private Func<Tkey, int> halfProducer;
        // Эта булевская переменная управляет "половинчатостью". Если она истина, то используется полуиндекс
        private bool isHalf = false;
        /// <summary>
        /// Конструктор ключевого индекса
        /// </summary>
        /// <param name="indexName">Имя индекса, требуется для создания файла.</param>
        /// <param name="table">Последовательность (таблица) индексируемых элементов</param>
        /// <param name="keyProducer">Функция вычисления ключа по ссылке на элемент</param>
        /// <param name="halfProducer">Функция вычисления полуключа по ключу, null если не испльзуется</param>
        public IndexKeyView(string indexName, PaEntry table, Func<PaEntry, Tkey> keyProducer,
            Func<Tkey, int> halfProducer)
        {
            this.table = table;
            this.keyProducer = keyProducer;
            this.halfProducer = halfProducer;
            PType tp_index;
            var q = typeof(Tkey);
            // Тип ключа или полуключа или длинный или целый
            PType tp_key = q == typeof(long) ? new PType(PTypeEnumeration.longinteger) : new PType(PTypeEnumeration.integer);
            tp_index = new PTypeSequence(new PTypeRecord(
                new NamedType("key", tp_key),
                new NamedType("offset", new PType(PTypeEnumeration.longinteger))));
            if (halfProducer != null) isHalf = true;
            this.index_cell = new PaCell(tp_index, indexName + ".pac", false);
        }
        public class HalfPair : IComparable
        {
            private long record_off;
            private int hkey;
            private IndexKeyView<Tkey> index;
            public HalfPair(long rec_off, int hkey, IndexKeyView<Tkey> index)
            {
                this.record_off = rec_off; this.hkey = hkey; this.index = index;
            }
            public int CompareTo(object pair)
            {
                if (!(pair is HalfPair)) throw new Exception("Exception 284401");
                HalfPair pa = (HalfPair)pair;
                int cmp = this.hkey.CompareTo(pa.hkey);
                if (cmp != 0) return cmp;
                if (index.table.Count() == 0) throw new Exception("Ex: 2943991");
                // Определяем ключ 
                PaEntry entry = index.table.Element(0);
                entry.offset = pa.record_off;
                Tkey key = index.keyProducer(entry);
                entry.offset = record_off;
                return index.keyProducer(entry).CompareTo(key);
            }
        }
        private long min, max;
        public void Build()
        {
            index_cell.Clear();
            index_cell.Fill(new object[0]);
            //bool firsttime = true;
            foreach (var rec in table.Elements().Where(ent => (bool)ent.Field(0).Get() == false)) // загрузка всех элементов за исключением уничтоженных
            {
                long offset = rec.offset;
                var key = keyProducer(rec);
                var key_hkey = isHalf ? (object)halfProducer(key) : (object)key;
                object[] i_element = new object[] { key_hkey, offset };
                index_cell.Root.AppendElement(i_element);
            }
            index_cell.Flush();
            if (index_cell.Root.Count() == 0) return; // потому что следующая операция не пройдет
            // Сортировать index_cell по (полу)ключу, а если совпадает и полуключ определен, то находя истинный ключ по offset'у
            var ptr = table.Element(0);
            if (isHalf)
            {
                index_cell.Root.SortByKey<HalfPair>((object v) =>
                {
                    object[] vv = (object[])v;
                    object half_key = vv[0];
                    long offset = (long)vv[1];
                    ptr.offset = offset;
                    return new HalfPair(offset, (int)half_key, this);
                });
            }
            else
            {
                index_cell.Root.SortByKey<Tkey>((object v) =>
                {
                    var vv = (Tkey)(((object[])v)[0]);
                    return vv;
                });
            }

        }
        public void Warmup()
        {
            foreach (var v in index_cell.Root.ElementValues()) ;
        }
        public PaEntry GetFirstByKey(Tkey key)
        {
            return GetFirstByKey(0, index_cell.Root.Count(), key);
        }
        public PaEntry GetFirstByKey(long start, long number, Tkey key)
        {
            if (table.Count() == 0) return PaEntry.Empty;
            PaEntry entry = table.Element(0);
            if (!isHalf)
            {
                var cand = index_cell.Root.BinarySearchFirst(start, number, ent => ((Tkey)ent.Field(0).Get()).CompareTo(key));
                if (cand.IsEmpty) return PaEntry.Empty;
                entry.offset = (long)cand.Field(1).Get();
            }
            else
            {
                int hkey = halfProducer(key);
                var candidate = index_cell.Root.BinarySearchFirst(start, number, ent =>
                {
                    object[] pair = (object[])ent.Get();
                    int hk = (int)pair[0];
                    int cmp = hk.CompareTo(hkey);
                    if (cmp != 0) return cmp;
                    long off = (long)pair[1];
                    entry.offset = off;
                    return ((IComparable)keyProducer(entry)).CompareTo(key);
                });
                if (candidate.IsEmpty) return PaEntry.Empty;
                entry.offset = (long)candidate.Field(1).Get();
            }
            return entry;
        }
        public IEnumerable<PaEntry> GetAllByKey(long start, long number, Tkey key)
        {
            if (table.Count() == 0) return Enumerable.Empty<PaEntry>();
            PaEntry entry = table.Element(0);
            IEnumerable<PaEntry> candidates;
            if (!isHalf)
            {
                candidates = index_cell.Root.BinarySearchAll(start, number, ent => ((Tkey)ent.Field(0).Get()).CompareTo(key));
            }
            else
            {
                int hkey = halfProducer(key);
                candidates = index_cell.Root.BinarySearchAll(start, number, ent =>
                {
                    object[] pair = (object[])ent.Get();
                    int hk = (int)pair[0];
                    int cmp = hk.CompareTo(hkey);
                    if (cmp != 0) return cmp;
                    long off = (long)pair[1];
                    entry.offset = off;
                    return ((IComparable)keyProducer(entry)).CompareTo(key);
                });
            }
            return candidates.Select(en => { entry.offset = (long)en.Field(1).Get(); return entry; })
                //.Where(en => (bool)en.Field(0).Get() != true)
                ;
        }
        public IEnumerable<PaEntry> GetAllByKey(Tkey key)
        {
            return GetAllByKey(0, index_cell.Root.Count(), key);
        }
    }
}
