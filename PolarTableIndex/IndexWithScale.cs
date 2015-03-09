using System;
using System.Collections.Generic;
using System.Linq;
using IndexCommon;
using PolarDB;

namespace PolarTableIndex
{
    public class IndexWithScale<Tkey> : IIndex<Tkey> where Tkey : IComparable
    {
        internal PaEntry table;
        private PaCell index_cell;
        internal Func<object[], Tkey> keyProducer;
        private Func<Tkey, int> halfProducer;
        private readonly bool useScale;
        // Эта булевская переменная управляет "половинчатостью". Если она истина, то используется полуиндекс
        private bool isHalf = false;
        // Это признак того, что второе целое в индексном массиве используется. Можно было бы варьировать тип элемента индексного массива 
        private bool isUsed = true;  
        Scale scale;
        private Func<object[], long> func4Diapasons;
        private PaEntry ptr;

        /// <summary>
        /// Конструктор ключевого индекса
        /// </summary>
        /// <param name="indexName">Имя индекса, требуется для создания файла.</param>
        /// <param name="table">Последовательность (таблица) индексируемых элементов</param>
        /// <param name="keyProducer">Функция вычисления ключа по ссылке на элемент</param>
        /// <param name="halfProducer">Функция вычисления полуключа по ключу, null если не испльзуется</param>
        public IndexWithScale(string indexName, PaEntry table, Func<object[], Tkey> keyProducer,
            Func<Tkey, int> halfProducer, bool useScale)
        {
            this.table = table;
            ptr = table.Element(0);
            this.keyProducer = keyProducer;
            this.halfProducer = halfProducer;
            this.useScale = useScale;
            PType tp_index;
            var q = typeof (Tkey);
            // Тип ключа или полуключа или длинный или целый
            PType tp_key = q == typeof (long)
                ? new PType(PTypeEnumeration.longinteger)
                : new PType(PTypeEnumeration.integer);
            tp_index = new PTypeSequence(new PTypeRecord(
                new NamedType("key", tp_key),
                new NamedType("offset", new PType(PTypeEnumeration.longinteger))));
            if (halfProducer != null) isHalf = true;
            else
            {
                if (q == typeof (string)) isUsed = false;
            }
            this.index_cell = new PaCell(tp_index, indexName + ".pac", false);     
            if (isHalf)
                func4Diapasons = objects => Convert.ToInt64(objects[0]);
            else if (!isHalf && !isUsed)
                func4Diapasons = objects =>
                {
                    long off = (long) (objects[1]);
                    ptr.offset = off;
                    return Convert.ToInt64(keyProducer((object[]) ptr.Get()));
                };
            else
               func4Diapasons=objects => Convert.ToInt64((objects)[0]);
            if (index_cell.IsEmpty) return;

            
            scale = new Scale(func4Diapasons, index_cell);
        }

        public class HalfPair : IComparable, IComparer<Tkey>
        {
            private long record_off;
            private int hkey;
            private IndexWithScale<Tkey> index;
            public HalfPair(long rec_off, int hkey, IndexWithScale<Tkey> index)
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
                Tkey key = index.keyProducer((object[])entry.Get());
                entry.offset = record_off;
                return index.keyProducer((object[])entry.Get()).CompareTo(key);
            }

            public int Compare(Tkey x, Tkey y)
            {
                return x.CompareTo(y);
            }
        }
      
        public void Build()
        {
            index_cell.Clear();
            index_cell.Fill(new object[0]);
            //bool firsttime = true;   
            int i = 0;
            table.Scan((offset, o) =>
            {
                var row = (object[])o;
                if ((bool)row[0]) return true;

                var key = keyProducer(row);
                var key_hkey = isHalf ? (object)halfProducer(key) : (isUsed ? (object)key : (object)(-1));
                object[] i_element = new object[] { key_hkey, offset };
                index_cell.Root.AppendElement(i_element);
                return true;
            });
            index_cell.Flush();
            if (index_cell.Root.Count() == 0) return; // потому что следующая операция не пройдет
            // Сортировать index_cell по (полу)ключу, а если совпадает и полуключ определен, то находя истинный ключ по offset'у
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
            else if (!isHalf && !isUsed)
            {
                index_cell.Root.SortByKey<Tkey>((object v) =>
                {
                    long off = (long)(((object[])v)[1]);
                    ptr.offset = off;
                    return keyProducer((object[])ptr.Get());
                });
             
            }
            else
            {
                index_cell.Root.SortByKey<Tkey>((object v) =>
                {
                    var vv = (Tkey)((object[])v)[0];
                    return vv;
                });
               
            }
            scale = new Scale(func4Diapasons, index_cell);

        }


           public void Warmup()
        {
            foreach (var v in index_cell.Root.ElementValues()) ;
        }
        public IEnumerable<PaEntry> GetAllByKey(long start, long number, Tkey key)
        {
            if (table.Count() == 0) return Enumerable.Empty<PaEntry>();
            PaEntry entry = table.Element(0);
            var candidates = GetAllCandidates(start, number, key, entry);
            return candidates.Select(en => { entry.offset = (long)en.Field(1).Get(); return entry; })
                .Where(en => (bool)en.Field(0).Get() != true);
        }

        private IEnumerable<PaEntry> GetAllCandidates(long start, long number, Tkey key, PaEntry entry)
        {
            IEnumerable<PaEntry> candidates;
            if (!isHalf && !isUsed)
            {
                candidates = index_cell.Root.BinarySearchAll(start, number, ent =>
                {
                    long off = (long)ent.Field(1).Get();
                    entry.offset = off;
                    return ((IComparable)keyProducer((object[])entry.Get())).CompareTo(key);
                });
            }
            else if (!isHalf)
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
                    return ((IComparable)keyProducer((object[])entry.Get())).CompareTo(key);
                });
            }
            return candidates;
        }

        public IEnumerable<object[]> GetAllReadedByKey(long start, long number, Tkey key)
        {
            if (table.Count() == 0) return Enumerable.Empty<object[]>();
            PaEntry entry = table.Element(0);
            var candidates = GetAllCandidates(start, number, key, entry);
            return candidates.Select(en =>
            { entry.offset = (long)en.Field(1).Get(); return entry; })
             .Select(en => (object[])en.Get())
                .Where(en => (bool)en[0] != true);
        }

        public IEnumerable<PaEntry> GetAllByKey(Tkey key)
        {
            if (useScale)
            {
                Diapason d = scale.Search( Convert.ToInt64((int) (isHalf ? (object)halfProducer(key) : key)));
                return GetAllByKey(d.start, d.numb, key);
            }
            return GetAllByKey(0, index_cell.Root.Count(), key);
        }

        public IEnumerable<object[]> GetAllReadedByKey(Tkey key)
        {
            if (useScale)
            {
                Diapason d = scale.Search(Convert.ToInt64((int)(isHalf ? (object)halfProducer(key) : key)));
                return GetAllReadedByKey(d.start, d.numb, key);
            }
            return GetAllReadedByKey(0, index_cell.Root.Count(), key);
        }

        public PaEntry Table { get; private set; }
        public Tkey KeyProducer(PaEntry entry)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public void Build2()
        {
            throw new NotImplementedException();
        }
    }
}
