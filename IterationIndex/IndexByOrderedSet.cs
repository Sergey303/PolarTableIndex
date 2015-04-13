using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PolarDB;

namespace IterationIndexLib
{
    public class IndexByOrderedSetBase
    {
        protected readonly DirectoryInfo indexDir;
     
        protected PaEntry table;
        protected PaCell offsets;
        protected PaEntry ptr;
        protected long maxCache;
        protected bool isBuilded;
        protected long cashed;

        protected IndexByOrderedSetBase(DirectoryInfo indexDir, PaEntry table, long maxCache)
        {
            this.indexDir = indexDir;
            this.table = table;
            this.maxCache = maxCache;
            if (!indexDir.Exists) indexDir.Create();
            offsets = new PaCell(new PTypeSequence(new PType(PTypeEnumeration.longinteger)),
                indexDir.FullName + "\\Index Name Table.pac", false);
        }

        public void Clear()
        {
           
            offsets.Clear();
            offsets.Fill(new object[0]);
            cashed = 0;
        }

        public void Close()
        {
            offsets.Close();
        }

        public void Warm()
        {
            foreach (var element in offsets.Root.Elements()) ;
        }
    }

    public class IndexByOrderedSet<T> : IndexByOrderedSetBase where T : IComparable<T>
    {
        private readonly Dictionary<T, HashSet<long>> addCache = new Dictionary<T, HashSet<long>>();
        private HashSet<long> hasheSet;  
        protected Func<PaEntry, T> keyProducer;
        public IndexByOrderedSet(DirectoryInfo indexDir, PaEntry table, Func<PaEntry, T> keyProducer, long maxCache)
            : base(indexDir, table, maxCache)
        {
            this.keyProducer = keyProducer;
        
            if (offsets.IsEmpty) offsets.Fill(new object[0]);
            Build();
        }

        public void Build()
        {
            offsets.Clear();
            offsets.Fill(new object[0]);
            if (table.IsEmpty || table.Count() == 0) return;
            ptr = table.Element(0);
            table.Scan((offset, o) =>
            {
                offsets.Root.AppendElement(offset);
                return true;
            });
            offsets.Flush();
            offsets.Root.SortByKey(offset =>
            {
                ptr.offset = (long) offset;
                return keyProducer(ptr);
            });
            isBuilded = true;
        }

        public IEnumerable<long> SearchOffsets(T key)
        {


            if (addCache.TryGetValue(key, out hasheSet))
                foreach (var offset in hasheSet)
                    yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long) entry.Get();
                return keyProducer(ptr).CompareTo(key);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T row)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(row).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }

        public void Add(T row, long offset)
        {
            isBuilded = false;

            if (!addCache.TryGetValue(row, out hasheSet))
                addCache.Add(row, hasheSet = new HashSet<long>());
            hasheSet.Add(offset);

            cashed++;
            if (cashed >= maxCache)
            {
                Console.WriteLine("start building index " + indexDir.Name);
                Build();
                addCache.Clear();
                cashed = 0;
            }
        }



        public bool Contains(T key)
        {
            if (addCache.TryGetValue(key, out hasheSet)) return true;

            if (table.IsEmpty || table.Count() == 0) return false;
            ptr = table.Element(0);
            PaEntry first = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long) entry.Get();
                return keyProducer(ptr).CompareTo(key);
            });
            return !first.IsEmpty; //todo
        }

        public new void Clear()
        {
            addCache.Clear();
            base.Clear();
        }
    }

    public class IndexByOrderedSet<T1,T2> :IndexByOrderedSetBase where T1 : IComparable<T1>
        where T2 : IComparable<T2>
    {

        private readonly Func<PaEntry, T1> key1Producer;
        private readonly Func<PaEntry, T2> key2Producer;
      
   
       
        private readonly Dictionary<T1, Dictionary<T2,HashSet<long>>> addCache = new Dictionary<T1, Dictionary<T2, HashSet<long>>>();
        private Dictionary<T2, HashSet<long>> subDictionary;
       
        private HashSet<long> hasheSet;
       

        public IndexByOrderedSet(DirectoryInfo indexDir, PaEntry table, long maxCache, Func<PaEntry, T1> key1Producer, Func<PaEntry, T2> key2Producer) 
            : base(indexDir, table, maxCache)
        {
        
            this.maxCache = maxCache;
            this.key1Producer = key1Producer;

            this.key2Producer = key2Producer;
            if (!indexDir.Exists) indexDir.Create();
          
            if (offsets.IsEmpty) offsets.Fill(new object[0]);
            Build();
        }

        public void Build()
        {
            if (isBuilded) return;
            offsets.Clear();
            offsets.Fill(new object[0]);
            if (table.IsEmpty || table.Count() == 0) return;
            ptr = table.Element(0);
            table.Scan((offset, o) =>
            {
                offsets.Root.AppendElement(offset);
                return true;
            });
            offsets.Flush();
            offsets.Root.SortByKey(offset =>
            {
                ptr.offset = (long) offset;
                return new Row(key1Producer(ptr), key2Producer(ptr));
            }, new Row());
            isBuilded = true;
        }
        public IEnumerable<long> SearchOffsets(T1 key1)
        {
            if (addCache.TryGetValue(key1, out subDictionary))
                    foreach (var offset in subDictionary.Values.SelectMany(hashSets => hashSets))
                        yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                return key1Producer(ptr).CompareTo(key1);
                
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }
        public IEnumerable<long> SearchOffsets(T1 key1, T2 key2)
        {                         
            if (addCache.TryGetValue(key1, out subDictionary))
                if (subDictionary.TryGetValue(key2, out hasheSet))
                foreach (var offset in hasheSet)
                    yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                return key2Producer(ptr).CompareTo(key2);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1, T2 k2)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1,k2).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }

        public void Add(T1 k1, T2 k2, long offset)
        {
            isBuilded = false;
            if (!addCache.TryGetValue(k1, out subDictionary))
                addCache.Add(k1, new Dictionary<T2, HashSet<long>>() {{k2, new HashSet<long>() {offset}}});
            else if (!subDictionary.TryGetValue(k2, out hasheSet))
                subDictionary.Add(k2, hasheSet = new HashSet<long>(){offset});
            else hasheSet.Add(offset);

            cashed++;
            if (cashed >= maxCache)
            {
                Console.WriteLine("start building index " + indexDir.Name);
                Build();
                addCache.Clear();
                cashed = 0;
            }
        }



        public bool Contains(T1 key1, T2 key2)
        {
            if (addCache.TryGetValue(key1, out subDictionary))
                if (subDictionary.TryGetValue(key2, out hasheSet))
                       return true;

            if (table.IsEmpty || table.Count() == 0) return false;
            ptr = table.Element(0);
            PaEntry first = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                return key2Producer(ptr).CompareTo(key2);
            });
            return !first.IsEmpty; //todo
        }

        public new void Clear()
        {
            addCache.Clear();
            base.Clear();
        }

       
    }
    public class IndexByOrderedSet<T1, T2,T3> : IndexByOrderedSetBase
        where T1 : IComparable<T1>
        where T2 : IComparable<T2>
        where T3 : IComparable<T3>
    {

        private readonly Func<PaEntry, T1> key1Producer;
        private readonly Func<PaEntry, T2> key2Producer;
        private readonly Func<PaEntry, T3> key3Producer;



        private readonly Dictionary<T1, Dictionary<T2, Dictionary<T3,HashSet<long>>>> addCache = new Dictionary<T1, Dictionary<T2, Dictionary<T3, HashSet<long>>>>();
        private Dictionary<T2, Dictionary<T3,HashSet<long>>> subDictionary1;
        private Dictionary<T3, HashSet<long>> subDictionary2;

        private HashSet<long> hasheSet;


        public IndexByOrderedSet(DirectoryInfo indexDir, PaEntry table, long maxCache, Func<PaEntry, T1> key1Producer, Func<PaEntry, T2> key2Producer, Func<PaEntry, T3> key3Producer)
            : base(indexDir, table, maxCache)
        {

            this.maxCache = maxCache;
            this.key1Producer = key1Producer;

            this.key2Producer = key2Producer;
            this.key3Producer = key3Producer;
            if (!indexDir.Exists) indexDir.Create();

            if (offsets.IsEmpty) offsets.Fill(new object[0]);
            Build();
        }

        public void Build()
        {
            if(isBuilded) return;
            offsets.Clear();
            offsets.Fill(new object[0]);
            if (table.IsEmpty || table.Count() == 0) return;
            ptr = table.Element(0);
            table.Scan((offset, o) =>
            {
                offsets.Root.AppendElement(offset);
                return true;
            });
            offsets.Flush();
            offsets.Root.SortByKey(offset =>
            {
                ptr.offset = (long)offset;
                return new Row(key1Producer(ptr), key2Producer(ptr), key3Producer(ptr));
            }, new Row());
            isBuilded = true;
        }

        public IEnumerable<long> SearchOffsets(T1 key1, T2 key2, T3 key3)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                if (subDictionary1.TryGetValue(key2, out subDictionary2))
                    if (subDictionary2.TryGetValue(key3, out hasheSet))
                    foreach (var offset in hasheSet)
                        yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                var compareK2 = key2Producer(ptr).CompareTo(key2);
                if (compareK2 != 0) return compareK2;
                return key3Producer(ptr).CompareTo(key3);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1, T2 k2, T3 k3)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1, k2,k3).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }

        public IEnumerable<long> SearchOffsets(T1 key1, T2 key2)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                if (subDictionary1.TryGetValue(key2, out subDictionary2))
                    foreach (var offset in subDictionary2.Values.SelectMany(v => v))
                        yield return offset;
          
            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                return key2Producer(ptr).CompareTo(key2);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1, T2 k2)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1, k2).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }
        public IEnumerable<long> SearchOffsets(T1 key1)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                foreach (var offset in subDictionary1.Values.SelectMany(v1 => v1.Values.SelectMany(v2 => v2)))
                        yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                return key1Producer(ptr).CompareTo(key1);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }
        public void Add(T1 k1, T2 k2, T3 k3, long offset)
        {
            isBuilded = false;
            if (!addCache.TryGetValue(k1, out subDictionary1))
                addCache.Add(k1, new Dictionary<T2, Dictionary<T3, HashSet<long>>>()
                    {{k2, new Dictionary<T3, HashSet<long>>() {{k3, new HashSet<long>() {offset}}}}});
            else if (!subDictionary1.TryGetValue(k2, out subDictionary2))
                subDictionary1.Add(k2, new Dictionary<T3, HashSet<long>>() {{k3, new HashSet<long>() {offset}}});
            else if (!subDictionary2.TryGetValue(k3, out hasheSet))
                subDictionary2.Add(k3, hasheSet = new HashSet<long>() {offset});
            else hasheSet.Add(offset);

            cashed++;
            if (cashed >= maxCache)
            {
                Console.WriteLine("start building index " + indexDir.Name);
                Build();
                addCache.Clear();
                cashed = 0;
            }
        }



        public bool Contains(T1 key1, T2 key2, T3 key3)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                if (subDictionary1.TryGetValue(key2, out subDictionary2))
                    if (subDictionary2.TryGetValue(key3, out hasheSet))
                    return true;

            if (table.IsEmpty || table.Count() == 0) return false;
            ptr = table.Element(0);
            PaEntry first = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                var compareK2 = key2Producer(ptr).CompareTo(key2);
                if (compareK2 != 0) return compareK2;
                return key3Producer(ptr).CompareTo(key3);
            });
            return !first.IsEmpty;
        }

        public new void Clear()
        {
            addCache.Clear();
            base.Clear();
        }


        public bool Contains(T1 key1)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                        return true;

            if (table.IsEmpty || table.Count() == 0) return false;
            ptr = table.Element(0);
            PaEntry first = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long)entry.Get();
                return key1Producer(ptr).CompareTo(key1);
            });
            return !first.IsEmpty; 
        }
    }
    public class IndexByOrderedSet<T1, T2, T3,T4> : IndexByOrderedSetBase
        where T1 : IComparable<T1>
        where T2 : IComparable<T2>
        where T3 : IComparable<T3>
        where T4 : IComparable<T4>
    {

        private readonly Func<PaEntry, T1> key1Producer;
        private readonly Func<PaEntry, T2> key2Producer;
        private readonly Func<PaEntry, T3> key3Producer;
        private readonly Func<PaEntry, T4> key4Producer;



        private readonly Dictionary<T1, Dictionary<T2, Dictionary<T3, Dictionary<T4, HashSet<long>>>>> addCache = new Dictionary<T1, Dictionary<T2, Dictionary<T3, Dictionary<T4, HashSet<long>>>>>();
        private Dictionary<T2, Dictionary<T3, Dictionary<T4, HashSet<long>>>> subDictionary1;
        private Dictionary<T3, Dictionary<T4, HashSet<long>>> subDictionary2;
        private Dictionary<T4, HashSet<long>> subDictionary3;

        private HashSet<long> hasheSet;


        public IndexByOrderedSet(DirectoryInfo indexDir, PaEntry table, long maxCache, Func<PaEntry, T1> key1Producer, Func<PaEntry, T2> key2Producer, Func<PaEntry, T3> key3Producer, Func<PaEntry, T4> key4Producer)
            : base(indexDir, table, maxCache)
        {

            this.maxCache = maxCache;
            this.key1Producer = key1Producer; 
            this.key2Producer = key2Producer;
            this.key3Producer = key3Producer;
            this.key4Producer = key4Producer;
            if (!indexDir.Exists) indexDir.Create();

            if (offsets.IsEmpty) offsets.Fill(new object[0]);
            Build();
        }

        public void Build()
        {
            if (isBuilded) return;
            offsets.Clear();
            offsets.Fill(new object[0]);
            if (table.IsEmpty || table.Count() == 0) return;
            ptr = table.Element(0);
            table.Scan((offset, o) =>
            {
                offsets.Root.AppendElement(offset);
                return true;
            });
            offsets.Flush();
            offsets.Root.SortByKey(offset =>
            {
                ptr.offset = (long)offset;
                return new Row(key1Producer(ptr), key2Producer(ptr), key3Producer(ptr), key4Producer(ptr));
            }, new Row());
            isBuilded = true;
        }

        public IEnumerable<long> SearchOffsets(T1 key1, T2 key2, T3 key3, T4 key4)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                if (subDictionary1.TryGetValue(key2, out subDictionary2))
                    if (subDictionary2.TryGetValue(key3, out subDictionary3))
                        if (subDictionary3.TryGetValue(key4, out hasheSet))
                            foreach (var offset in hasheSet)
                                yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                var compareK2 = key2Producer(ptr).CompareTo(key2);
                if (compareK2 != 0) return compareK2;
                var compareK3 = key3Producer(ptr).CompareTo(key3);
                if (compareK3 != 0) return compareK3;
                return key4Producer(ptr).CompareTo(key4);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1, T2 k2, T3 k3, T4 k4)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1, k2, k3, k4).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }
        public IEnumerable<long> SearchOffsets(T1 key1, T2 key2, T3 key3)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                if (subDictionary1.TryGetValue(key2, out subDictionary2))
                    if (subDictionary2.TryGetValue(key3, out subDictionary3))
                        foreach (var offset in subDictionary3.Values.SelectMany(v=>v))
                                yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                var compareK2 = key2Producer(ptr).CompareTo(key2);
                if (compareK2 != 0) return compareK2;
                return key3Producer(ptr).CompareTo(key3);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1, T2 k2, T3 k3)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1, k2, k3).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }
        public IEnumerable<long> SearchOffsets(T1 key1, T2 key2)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                if (subDictionary1.TryGetValue(key2, out subDictionary2))
                    foreach (var offset in subDictionary2.Values.SelectMany(v => v.Values.SelectMany(set => set)))
                        yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                return key2Producer(ptr).CompareTo(key2);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1, T2 k2)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1, k2).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }
        public IEnumerable<long> SearchOffsets(T1 key1)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                foreach (var offset in subDictionary1.Values.SelectMany(v1 => subDictionary2.Values.SelectMany(v2 => subDictionary3.Values.SelectMany(v3=> v3))))
                    yield return offset;

            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long)entry.Get();
                return key1Producer(ptr).CompareTo(key1);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(T1 k1)
        {
            if (table.IsEmpty || table.Count() == 0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(k1).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }

        public void Add(T1 k1, T2 k2, T3 k3, T4 k4, long offset)
        {
            isBuilded = false;
            if (!addCache.TryGetValue(k1, out subDictionary1))
                addCache.Add(k1,
                    new Dictionary<T2, Dictionary<T3, Dictionary<T4, HashSet<long>>>>()
                    {{k2, new Dictionary<T3, Dictionary<T4, HashSet<long>>>(){
                                {k3, new Dictionary<T4, HashSet<long>>() {{k4, new HashSet<long>() { offset}}}}}}});
            else if (!subDictionary1.TryGetValue(k2, out subDictionary2))
                subDictionary1.Add(k2, new Dictionary<T3, Dictionary<T4, HashSet<long>>>{{k3, new Dictionary<T4, HashSet<long>>() { { k4, new HashSet<long>() { offset } } }}});
            else if (!subDictionary2.TryGetValue(k3, out subDictionary3))
                subDictionary2.Add(k3, new Dictionary<T4, HashSet<long>>() { { k4, new HashSet<long>() { offset } } });
            else if (!subDictionary3.TryGetValue(k4, out hasheSet))
                subDictionary3.Add(k4, hasheSet = new HashSet<long>() { offset });
            else hasheSet.Add(offset);

            cashed++;
            if (cashed >= maxCache)
            {
                Console.WriteLine("start building index " + indexDir.Name);
                Build();
                addCache.Clear();
                cashed = 0;
            }
        }           

        public new void Clear()
        {
            addCache.Clear();
            base.Clear();
        }


        public bool Contains(T1 key1)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                return true;

            if (table.IsEmpty || table.Count() == 0) return false;
            ptr = table.Element(0);
            PaEntry first = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long)entry.Get();
                return key1Producer(ptr).CompareTo(key1);
            });
            return !first.IsEmpty;
        }   
        public bool Contains(T1 key1, T2 key2, T3 key3, T4 key4)
        {
            if (addCache.TryGetValue(key1, out subDictionary1))
                if (subDictionary1.TryGetValue(key2, out subDictionary2))
                    if (subDictionary2.TryGetValue(key3, out subDictionary3))
                        if (subDictionary3.TryGetValue(key4, out hasheSet))
                        return true;

            if (table.IsEmpty || table.Count() == 0) return false;
            ptr = table.Element(0);
            PaEntry first = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long)entry.Get();
                var compareK1 = key1Producer(ptr).CompareTo(key1);
                if (compareK1 != 0) return compareK1;
                var compareK2 = key2Producer(ptr).CompareTo(key2);
                if (compareK2 != 0) return compareK2;
                var compareK3 = key3Producer(ptr).CompareTo(key3);
                if (compareK3 != 0) return compareK3;
                return key4Producer(ptr).CompareTo(key4);
            });
            return !first.IsEmpty;
        }                               
    } 
}
