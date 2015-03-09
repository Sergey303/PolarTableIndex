using System;
using System.Collections.Generic;
using System.Linq;
using GoIndex;
using IndexCommon;
using PolarDB;

namespace PolarTableIndex
{
   public class IndexWithScale<Tkey> : IIndex<Tkey> where Tkey : IComparable
    {
        internal PaEntry table;
        private PaCell index_cell;
        internal Func<PaEntry, Tkey> keyProducer;
        private Scale<Tkey> scale;

       

        private Func<Tkey, int> halfProducer;
       private readonly bool isScaleUsed;
       // Эта булевская переменная управляет "половинчатостью". Если она истина, то используется полуиндекс
        private bool isHalf = false;
        // Это признак того, что второе целое в индексном массиве используется. Можно было бы варьировать тип элемента индексного массива 
        private bool isUsed = true;

       /// <summary>
       /// Конструктор ключевого индекса
       /// </summary>
       /// <param name="indexName">Имя индекса, требуется для создания файла.</param>
       /// <param name="table">Последовательность (таблица) индексируемых элементов</param>
       /// <param name="keyProducer">Функция вычисления ключа по ссылке на элемент</param>
       /// <param name="halfProducer">Функция вычисления полуключа по ключу, null если не испльзуется</param>
       public IndexWithScale(string indexName, PaEntry table, Func<PaEntry, Tkey> keyProducer,
           Func<Tkey, int> halfProducer, bool isScaleUsed=false)
       {
           this.table = table;
           this.keyProducer = keyProducer;
           this.halfProducer = halfProducer;
           this.isScaleUsed = isScaleUsed;
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

           if(!isScaleUsed || index_cell.IsEmpty) return;
           //создание диапазонов сканированием таблицы индексов 
           var ptr = table.Element(0);

           scale = new Scale<Tkey>(isHalf || isUsed
               ? (Func<object[], Tkey>)(row => (Tkey) row[0])
               : row =>
               {
                   long off1 = (long) (row[1]);
                   ptr.offset = off1;
                   return keyProducer(ptr);
               }, index_cell);
       }




       //private long min, max;
        public void Build()
        {
            index_cell.Clear();
            index_cell.Fill(new object[0]);
            //bool firsttime = true;
            foreach (var rec in table.Elements().Where(ent => (bool)ent.Field(0).Get() == false)) // загрузка всех элементов за исключением уничтоженных
            {
                long offset = rec.offset;
                var key = keyProducer(rec);
                var key_hkey = isHalf ? (object)halfProducer(key) : (isUsed? (object)key : (object)(-1));
                object[] i_element = new object[] { key_hkey, offset };
                index_cell.Root.AppendElement(i_element);
            }
            index_cell.Flush();
            if (index_cell.Root.Count() == 0) return; // потому что следующая операция не пройдет
            // Сортировать index_cell по (полу)ключу, а если совпадает и полуключ определен, то находя истинный ключ по offset'у
            var ptr = table.Element(0);
            if (isHalf)
            {
                index_cell.Root.SortByKey<HalfPair<Tkey>>((object v) =>
                {
                    object[] vv = (object[])v;
                    object half_key = vv[0];
                    long offset = (long)vv[1];
                    ptr.offset = offset;
                    return new HalfPair<Tkey>(offset, (int)half_key, this);
                });  
           
            scale = new Scale<Tkey>(row=>(Tkey) row[0], index_cell); 
            }
            else if (!isHalf && !isUsed)
            {
                index_cell.Root.SortByKey<Tkey>((object v) =>
                {
                    long off = (long)(((object[])v)[1]);
                    ptr.offset = off;
                    return keyProducer(ptr);
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
                   long off = (long) ent.Field(1).Get();
                   entry.offset = off;
                   return ((IComparable) keyProducer(entry)).CompareTo(key);
               });
           }
           else if (!isHalf)
           {
               candidates = index_cell.Root.BinarySearchAll(start, number, ent => ((Tkey) ent.Field(0).Get()).CompareTo(key));
           }
           else
           {
               int hkey = halfProducer(key);
               candidates = index_cell.Root.BinarySearchAll(start, number, ent =>
               {
                   object[] pair = (object[]) ent.Get();
                   int hk = (int) pair[0];
                   int cmp = hk.CompareTo(hkey);
                   if (cmp != 0) return cmp;
                   long off = (long) pair[1];
                   entry.offset = off;
                   return ((IComparable) keyProducer(entry)).CompareTo(key);
               });
           }
           return candidates;
       }

       public IEnumerable<object[]> GetAllReadedByKey(long start, long number, Tkey key)
       {
           if (table.Count() == 0) return Enumerable.Empty<object[]>();
           PaEntry entry = table.Element(0);
           var candidates = GetAllCandidates(start, number, key, entry);
           return candidates.Select(en=>
            { entry.offset = (long)en.Field(1).Get(); return entry; })
            .Select(en => (object[])en.Get())
               .Where(en => (bool)en[0] != true);
       }

       public IEnumerable<PaEntry> GetAllByKey(Tkey key)
        {
           if(!isScaleUsed)
            return GetAllByKey(0,index_cell.Root.Count(), key);
           var d = scale.Search(key);
       return GetAllByKey(d.start,d.numb, key);
        }

       public IEnumerable<object[]> GetAllReadedByKey(Tkey key)
       {
           if (!isScaleUsed)
               return GetAllReadedByKey(0, index_cell.Root.Count(), key);
           var d = scale.Search(key);
           return GetAllReadedByKey(d.start, d.numb, key);    
       }

       public PaEntry Table { get { return table; } }
       public Tkey KeyProducer(PaEntry entry)
       {
           return keyProducer(entry);
       }
    }
}
