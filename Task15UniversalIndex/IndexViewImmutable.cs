using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace Task15UniversalIndex
{
    public class IndexViewImmutable<Tkey> : IIndexImmutable<Tkey> where Tkey : IComparable
    {
        private PaCell index_cell;
        public PaCell IndexCell { get { return index_cell; } }
        public IndexViewImmutable(string path_name)
        {
            index_cell = new PaCell(new PTypeSequence(new PType(PTypeEnumeration.longinteger)),
                path_name + ".pac", false);
        }
        public Func<object, Tkey> KeyProducer { get; set; }
        public IBearingTableImmutable Table { get; set; }

        public void Build()
        {
            index_cell.Clear();
            index_cell.Fill(new object[0]);
            if (KeyProducer == null) throw new Exception("Err: KeyProducer not defined");
            Table.Scan((offset, o) =>
            {
                var key = KeyProducer(o);
                index_cell.Root.AppendElement(offset);
                return true;
            });
            index_cell.Flush();
            if (index_cell.Root.Count() == 0) return; // потому что следующая операция не пройдет
            var ptr = Table.Element(0);
            index_cell.Root.SortByKey<Tkey>((object v) =>
            {
                long off = (long)v;
                ptr.offset = off;
                return KeyProducer((object[])ptr.Get());
            });
        }
        public void Warmup() { foreach (var v in index_cell.Root.ElementValues()); }

        public IEnumerable<PaEntry> GetAllByKey(long start, long number, Tkey key)
        {
            if (Table == null || Table.Count() == 0) return Enumerable.Empty<PaEntry>();
            PaEntry entry = Table.Element(0);
            PaEntry entry1 = entry;
            var query = index_cell.Root.BinarySearchAll(start, number, ent =>
            {
                long off = (long)ent.Get();
                entry.offset = off;
                return KeyProducer((object[])entry.Get()).CompareTo(key);
            });
            return query.Select(ent =>
            {
                entry1.offset = (long)ent.Get();
                return entry1;
            });
        }

        public IEnumerable<PaEntry> GetAllByKey(Tkey key)
        {
            return GetAllByKey(0, index_cell.Root.Count(), key);
        }
        public long Count() { return index_cell.Root.Count(); }
    }
}
