﻿using System;
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
        internal Func<object[], Tkey> keyProducer;
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
        public Index(string indexName, PaEntry table, Func<object[], Tkey> keyProducer,
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
        public class HalfPair : IComparable, IComparer<Tkey>
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
                Tkey key = index.keyProducer((object[])entry.Get());
                entry.offset = record_off;
                return index.keyProducer((object[])entry.Get()).CompareTo(key);
            }

            public int Compare(Tkey x, Tkey y)
            {
                return x.CompareTo(y);
            }
        }
        private long min = long.MaxValue, max = long.MinValue;
        Diapason[] diapasons = null;
        public void Build()
        {
            index_cell.Clear();
            index_cell.Fill(new object[0]);
            table.Scan((offset, o) =>
            {
                var row = (object[]) o;
                if (row[0].Equals(true)) return true;

                var key = keyProducer(row);    
                //var key_hkey = isHalf ? (object) halfProducer(key) : (isUsed ? (object) key : (object) (-1));
                int key_hkey = isHalf ? (int)halfProducer(key) : (isUsed ? Convert.ToInt32(key) : (-1));
                if (key_hkey < min) min = key_hkey;
                if (key_hkey > max) max = key_hkey;
                object[] i_element = new object[] { key_hkey, offset };
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
                    return keyProducer((object[]) ptr.Get());
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

            //if (isUsed) BuildScale(10000);
        }
       private Func<int, int> ToPosition = null;
        public void BuildScale(int N)
        {
            if (index_cell.IsEmpty || index_cell.Root.Count() == 0) return;
            // Если минимальное и максимальное не вычислены
            if (min == long.MaxValue || max == long.MinValue)
            {
                //Берем первый и последний ключ/полуключ
                min = (int)index_cell.Root.Element(0).Field(0).Get();
                max = (int)index_cell.Root.Element(index_cell.Root.Count() - 1).Field(0).Get();
            }

            diapasons = new Diapason[N];
            ToPosition = (int key) => (int)(((long)key - min) * (long)(N-1) / (max - min));
            index_cell.Root.Scan((long off, object val) =>
            {
                object[] pair = (object[])val;
                int position = ToPosition((int)pair[0]);
                // Предполагаю, что начальная разметка диапазона - нули
               // if (diapasons[position].start == 0L) diapasons[position].start = off;
                diapasons[position].numb += 1;
                return true;
            });
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                diapasons[i].start = sum;
                sum += diapasons[i].numb;
            }
        }
   

       class TkeyComparer: IComparer<Tkey>
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
                   return ((IComparable) keyProducer((object[]) entry.Get())).CompareTo(key);
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
                   return ((IComparable) keyProducer((object[]) entry.Get())).CompareTo(key);
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
            long start = 0;
            long number = index_cell.Root.Count();
            if (isUsed)
            {
                int key_hkey = isHalf ? (int)halfProducer(key) : Convert.ToInt32(key);
                if (ToPosition != null && diapasons != null)
                {
                    int pos = ToPosition(key_hkey);
                    if (pos < 0 || pos >= diapasons.Length) return Enumerable.Empty<PaEntry>();
                    start = diapasons[pos].start;
                    number = diapasons[pos].numb;
                }
                if (number == 0L) return Enumerable.Empty<PaEntry>();
            }
            return GetAllByKey(start, number, key);
        }

       public IEnumerable<object[]> GetAllReadedByKey(Tkey key)
       {
           long start = 0;
           long number = index_cell.Root.Count();
           if (isUsed)
           {
               int key_hkey = isHalf ? (int)halfProducer(key) : Convert.ToInt32(key);
               if (ToPosition != null && diapasons != null)
               {
                   int pos = ToPosition(key_hkey);
                   if (pos < 0 || pos >= diapasons.Length) return Enumerable.Empty<object[]>();
                   start = diapasons[pos].start;
                   number = diapasons[pos].numb;
               }
               if (number == 0L) return Enumerable.Empty<object[]>();
           }
           return GetAllReadedByKey(start, number, key);
       }

       public PaEntry Table { get; private set; }
       public Tkey KeyProducer(PaEntry entry)
       {
           throw new NotImplementedException();
       }

       public long Count()
       {
           return index_cell.Root.Count();
       }   
    }
}
