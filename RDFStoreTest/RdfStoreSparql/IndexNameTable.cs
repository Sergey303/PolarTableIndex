using System;
using System.Collections.Generic;
using System.IO;

using PolarDB;


    public class IndexNameTable :IDisposable
    {
        private readonly DirectoryInfo indexDir;
        private readonly PaEntry table;
        private readonly Func<PaEntry, string> stringProducer;
        private readonly PaCell offsets;
        private  PaEntry ptr;
        private readonly Dictionary<string, long> addCache = new Dictionary<string, long>();

        public IndexNameTable(DirectoryInfo indexDir, PaEntry table, Func<PaEntry, string> stringProducer, long maxCache)
        {
            this.indexDir = indexDir;
            this.table = table;
            this.stringProducer = stringProducer;
            this.maxCache = maxCache;
            if (!indexDir.Exists) indexDir.Create();
            offsets = new PaCell(new PTypeSequence(new PType(PTypeEnumeration.longinteger)), indexDir.FullName + "\\Index Name Table.pac", false);     
            if(offsets.IsEmpty) offsets.Fill(new object[0]);
           // Build();
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
            offsets.Root.SortByKey(offset=>
            {
                ptr.offset = (long)offset;
              return  stringProducer(ptr);
            });
            isBuilded = true;
        }

        public long SearchOffsets(string row)
        {
            long offset;
            if (addCache.TryGetValue(row, out offset))
                return offset;

            if (table.IsEmpty || table.Count() == 0) return -1;
            ptr = table.Element(0);
            var result = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long) entry.Get();
                return System.String.Compare(stringProducer(ptr), row, System.StringComparison.Ordinal);
            });
            if (result.IsEmpty) return -1;
            return (long) result.Get();
        }

        public object Search(string row)
        {
            if (table.IsEmpty || table.Count() == 0) return null;
            ptr = table.Element(0);
            var offset = SearchOffsets(row);
            if(offset==-1) return null;
            ptr.offset = offset;
            return ptr.Get();
        }

        public void Add(string row, long offset)
        {
            isBuilded = false;     
            addCache.Add(row, offset);
            //((HashSet<long>)hash).Add(offset);
            cashed++;
            if (cashed >= maxCache)
            {
                Console.WriteLine("start building index " + indexDir.Name);
                Build();
                addCache.Clear();
                cashed = 0;
            }
        }

        private readonly long maxCache;
        private long cashed;
        private bool isBuilded;

        public bool Contains(string row)
        {     
            if (addCache.ContainsKey(row)) return true;

            if (table.IsEmpty || table.Count() == 0) return false;
            ptr = table.Element(0);
            PaEntry first = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long) entry.Get();
                return System.String.Compare(stringProducer(ptr), row, System.StringComparison.Ordinal);
            });
            return !first.Equals(PaEntry.Empty);
        }

        public void Dispose()
        {
            if (isBuilded) return;
            Build();
        }

        public void Clear()
        {
            addCache.Clear();
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

