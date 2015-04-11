﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PolarDB;

namespace IterationIndexLib
{
    public class IterationIndexMultiplyValues
    {
        private readonly PaEntry table;
        private readonly Func<object, Row> rowProducer;
        private readonly PaCell offsets;
        private  PaEntry ptr;
        private readonly Dictionary<dynamic, dynamic> addCache = new Dictionary<dynamic, dynamic>();
        private readonly int rowLength;
        private long maxCache;
        private long cashed;
        private bool isBuilded;
        private DirectoryInfo indexDir;
        public IterationIndexMultiplyValues(DirectoryInfo indexDir, PaEntry table, Func<object, Row> rowProducer, int rowLength, long maxCache)
        {
            this.table = table;
            this.rowProducer = rowProducer;
            this.rowLength = rowLength;
            this.maxCache = maxCache;
            this.indexDir = indexDir;
            if (!indexDir.Exists) indexDir.Create();
            offsets = new PaCell(new PTypeSequence(new PType(PTypeEnumeration.longinteger)), indexDir.FullName + "\\offsets_index.pac", false);     
            if(offsets.IsEmpty) offsets.Fill(new object[0]);
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
            offsets.Root.SortByKey(offset=>
            {
                ptr.offset = (long)offset;
              return  rowProducer(ptr.Get());
            },new Row());
            isBuilded = true;
        }

        public IEnumerable<long> SearchOffsets(Row row)
        {
            dynamic recursive = addCache;
            for (int i = 0; i < row.Length; i++)
                if (!((Dictionary<dynamic, dynamic>)recursive).TryGetValue(row[i], out recursive))
                    break;
            if (recursive != null)
                if (row.Length == rowLength)
                    foreach (var offset in (HashSet<long>) recursive)
                        yield return offset;
                else
                {
                    IEnumerable<object> last = Enumerable.Repeat(recursive, 1);
                    for (int i = row.Length; i < rowLength; i++)
                        last = last.SelectMany(objects => ((Dictionary<dynamic, dynamic>)objects).Values);
                    foreach (var ofset in last.SelectMany(objects => ((HashSet<long>) objects)))
                        yield return ofset;
                }                
            if (table.IsEmpty || table.Count() == 0) yield break;
            ptr = table.Element(0);
            foreach (var result in offsets.Root.BinarySearchAll(entry =>
            {
                ptr.offset = (long) entry.Get();
                return rowProducer(ptr.Get()).CompareTo(row);
            }).Select(entry => entry.Get())
                .Cast<long>())
                yield return result;
        }

        public IEnumerable<object> Search(Row row)
        {
            if (table.IsEmpty || table.Count()==0) return Enumerable.Empty<object>();
            ptr = table.Element(0);
            return SearchOffsets(row).Select(offset =>
            {
                ptr.offset = offset;
                return ptr.Get();
            });
        }

        public void Add(Row row, long offset)
        {
            isBuilded = false;
            Dictionary<dynamic, dynamic> recursive = addCache;
            object nextRecursive;
            for (int i = 0; i < row.Length - 1; i++)
            {
                if (recursive.TryGetValue(row[i], out nextRecursive))
                { recursive = (Dictionary<dynamic, dynamic>)nextRecursive; continue; }
                var dictionary = new Dictionary<dynamic, dynamic>();
                recursive.Add(row[i], dictionary);
                recursive = dictionary;
            }
            object hash;
            if (!recursive.TryGetValue(row[rowLength - 1], out hash))
                recursive.Add(row[rowLength - 1], hash = new HashSet<long>());
            ((HashSet<long>)hash).Add(offset);
            cashed++;
            if (cashed >= maxCache)
            {
                Console.WriteLine("start building index " + indexDir.Name);
                Build();
                addCache.Clear();
                cashed = 0;
            }
        }

      

        public bool Contains(Row row)
        {
            dynamic recursive = addCache;
            for (int i = 0; i < row.Length - 1; i++)
            {
                if (((Dictionary<dynamic, dynamic>)recursive).TryGetValue(row[i], out recursive)) continue;
                //recursive = null;
                break;
            }

            if (recursive != null)
            {
                IEnumerable<Dictionary<dynamic, dynamic>> last = Enumerable.Repeat((Dictionary<dynamic, dynamic>)recursive, 1);
                for (int i = row.Length - 1; i < rowLength - 1; i++)
                    last = last.SelectMany(objects => objects.Values.Cast<Dictionary<dynamic, dynamic>>());
                var hashes = last.SelectMany(objects => objects.Values.Cast<HashSet<long>>()
                                                            .SelectMany(set => set));
               if(hashes.Any()) return true;
            }

            if (table.IsEmpty || table.Count() == 0) return false;
            ptr = table.Element(0);
            PaEntry first = offsets.Root.BinarySearchFirst(entry =>
            {
                ptr.offset = (long) entry.Get();
                return rowProducer(ptr.Get()).CompareTo(row);
            });
            return !first.IsEmpty;
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
    
}
