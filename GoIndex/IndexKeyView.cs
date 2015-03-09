using System;
using System.Collections.Generic;
using System.Linq;
using IndexCommon;
using PolarDB;

namespace GoIndex
{
   public class Index<Tkey> : IIndex<Tkey> where Tkey : IComparable
    {
        internal PaEntry table;
        private PaCell index_cell;
        internal Func<PaEntry, Tkey> keyProducer;
        private Func<Tkey, int> halfProducer;
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
        public Index(string indexName, PaEntry table, Func<PaEntry, Tkey> keyProducer,
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
            else
            {
                if (q == typeof(string)) isUsed = false;  
            }
            this.index_cell = new PaCell(tp_index, indexName + ".pac", false);
        }
        public class HalfPair : IComparable ,IComparer<Tkey>
        {
            private long record_off;
            private int hkey;
            private Index<Tkey> index;
            public HalfPair(long rec_off, int hkey, Index<Tkey> index)
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

            public int Compare(Tkey x, Tkey y)
            {
                return x.CompareTo(y);
            }
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
        public void Build3()
        {
            index_cell.Clear();
            index_cell.Fill(new object[0]);
            //bool firsttime = true;   
            int i = 0;
            table.Scan((offset, o) =>
            {
                var row = (object[]) o;
                if (row[0].Equals(true)) return true;

                var key = (Tkey)Convert.ChangeType(i++.ToString(),(typeof (Tkey)));//keyProducer(rec);     ///
                var key_hkey = isHalf ? (object) halfProducer(key) : (isUsed ? (object) key : (object) (-1));
                object[] i_element = new object[] {key_hkey, offset};
                index_cell.Root.AppendElement(i_element);
                return true;
            });
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
        public void Build2()
        {
            index_cell.Clear();
            index_cell.Fill(new object[0]);
            //bool firsttime = true;
             int max = Int32.MinValue;
                int min = Int32.MaxValue; 
            foreach (var rec in table.Elements().Where(ent => (bool)ent.Field(0).Get() == false)) // вычисление максимального и минимального ключа
            {
              
                var key = keyProducer(rec);
              
                int key_hkey = isHalf ? halfProducer(key) : Convert.ToInt32(key);
                if (key_hkey > max) max = key_hkey;
                if (key_hkey < min) min = key_hkey;
                //  object[] i_element = new object[] { key_hkey, offset };
                //   index_cell.Root.AppendElement(i_element);
            }
            Diapason[] diapasons=new Diapason[10000];
            long differece = (long)max - min;
            Func<int, int> getSubKey = x =>
            {
                var diffX = 1.0*((long)x - min);
                var t = (diapasons.Length - 1)*diffX;
                var d = (t/differece);

                var i = (int) d;
                if (i < 0)
                {
                    
                }
                return i;
            };
            foreach (var rec in table.Elements().Where(ent => (bool)ent.Field(0).Get() == false)) // создание диапазонов
            {
                  var key = keyProducer(rec); 
                int key_hkey = isHalf ? halfProducer(key) : Convert.ToInt32(key);
                int subKey = getSubKey(key_hkey);
                //увеличение числа элементов в диапазоне в несколько строк, т.к. диапазон стркурура, это вынесено в расширение DiapasonExt
              //  diapasons[subKey] = diapasons[subKey].AddNumb(1);
                var d = diapasons[subKey];
                d.numb++;
                diapasons[subKey] = d;
                
            }
            //Выичсление начал диапазонов
            long sum = 0;
            for (int i = 0; i < diapasons.Length; i++)
            {
              //  diapasons[i] = diapasons[i].AddStart(sum); //start must be 0
                var d = diapasons[i];
                d.start=sum;
                diapasons[i] = d;
                sum += diapasons[i].numb;
            }

            
            //пустышка, которой временно заполняется индекс.
            var empty = new object[] { -1, 0l };
            var count = 0l;//index_cell.Root.Count();
            foreach (var rec in table.Elements().Where(ent => (bool)ent.Field(0).Get() == false)) //заполнение таблицы индекса, по диапазонам
            {

                var key = keyProducer(rec);

                int key_hkey_int;
                object key_hkey;
                if (isHalf) key_hkey = key_hkey_int = halfProducer(key);
                else
                {
                    key_hkey = isUsed ? (object)key : (object)(-1);
                    key_hkey_int = Convert.ToInt32(key);
                }
                
                var diapasonIndex = getSubKey(key_hkey_int);

                long position;
                var d = diapasons[diapasonIndex];
                if (diapasonIndex == diapasons.Length - 1)
                    position = sum - d.numb;
                else
                    position = diapasons[diapasonIndex + 1].start - d.numb;
                //    число элементов в диапазоне используется как временный счётчик, что бы смещать позицию внутри диапазона
                d.numb--;
                diapasons[diapasonIndex] = d;
              
                  object[] i_element = new object[] { key_hkey, rec.offset };

                //если число строк в таблице индекса меньше,чем вычисленная позиция
                if (count <= position)
                {
                    //заполним пустышками до нужной позиции
                    for (; count < position ; count++)  index_cell.Root.AppendElement(empty);
                    index_cell.Root.AppendElement(i_element);
                    count ++;
                }
                else  //заменим пустышку элементом.
                    index_cell.Root.Element(position).Set(i_element);
            }
            index_cell.Flush();


            if (count == 0) return; // потому что следующая операция не пройдет
            //восстановим диапазоны, т.к. они использовались как счётчики
            for (int i = 0; i < diapasons.Length - 1; i++)
            {
                // diapasons[i] = diapasons[i].AddNumb(diapasons[i + 1].start - diapasons[i].start); //numb must be 0
                var d = diapasons[i];
                d.numb = diapasons[i + 1].start - d.start;
                diapasons[i] = d;
            }

            var dLast = diapasons[diapasons.Length - 1];
            dLast.numb=(count - dLast.start);
            diapasons[diapasons.Length - 1] = dLast;
            // Сортировать index_cell по (полу)ключу, а если совпадает и полуключ определен, то находя истинный ключ по offset'у
            var ptr = table.Element(0);
            if (isHalf)
            {
                for (int i = 0; i < diapasons.Length; i++)
                    index_cell.Root.SortByKey<HalfPair>(diapasons[i].start, diapasons[i].numb, (object v) =>
                {
                    object[] vv = (object[])v;
                    object half_key = vv[0];
                    long offset = (long)vv[1];
                    ptr.offset = offset;
                    return new HalfPair(offset, (int)half_key, this);
                } ,null);
            }
            else if (!isHalf && !isUsed)
            {
                for (int i = 0; i < diapasons.Length; i++)
                    index_cell.Root.SortByKey<Tkey>(diapasons[i].start, diapasons[i].numb, (object v) =>
                {
                    long off = (long)(((object[])v)[1]);
                    ptr.offset = off;
                    return keyProducer(ptr);
                },new TkeyComparer());
            }
            else
            {
                for (int i = 0; i < diapasons.Length; i++)
                    index_cell.Root.SortByKey<Tkey>(diapasons[i].start, diapasons[i].numb,
                        (object v) =>
                    {
                        var vv = (Tkey) (((object[]) v)[0]);
                        return vv;
                    },new TkeyComparer());
            }

        }

       class TkeyComparer:IComparer<Tkey>
       {
           public int Compare(Tkey x, Tkey y)
           {
               return x.CompareTo(y);
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
            return GetAllByKey(0, index_cell.Root.Count(), key);
        }

       public IEnumerable<object[]> GetAllReadedByKey(Tkey key)
       {
           return GetAllReadedByKey(0, index_cell.Root.Count(), key);
       }

       public PaEntry Table { get; private set; }
       public Tkey KeyProducer(PaEntry entry)
       {
           throw new NotImplementedException();
       }
    }
}
