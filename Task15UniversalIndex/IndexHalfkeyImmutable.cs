using System;
using System.Collections.Generic;
using System.Linq;
using PolarDB;

namespace Task15UniversalIndex
{
    public class IndexHalfkeyImmutable<Tkey> : IIndexImmutable<Tkey> where Tkey : IComparable
    {
        private PaCell index_cell;
        public PaCell IndexCell { get { return index_cell; } }
        public IndexHalfkeyImmutable(string path_name)
        {
            PType tp_hkey = new PType(PTypeEnumeration.integer);
            PType tp_index = new PTypeSequence(new PTypeRecord(
                new NamedType("halfkey", tp_hkey),
                new NamedType("offset", new PType(PTypeEnumeration.longinteger))));
            index_cell = new PaCell(tp_index, path_name + ".pac", false);
        }
        public Func<object, Tkey> KeyProducer { get; set; }
        public Func<Tkey, int> HalfProducer { get; set; }
        public IBearingTableImmutable Table { get; set; }
        public IScale Scale { get; set; }

        public class HalfPair : IComparable, IComparer<Tkey>
        {
            private long record_off;
            private int hkey;
            private IndexHalfkeyImmutable<Tkey> index;
            public HalfPair(long rec_off, int hkey, IndexHalfkeyImmutable<Tkey> index)
            {
                this.record_off = rec_off; this.hkey = hkey; this.index = index;
            }
            public int CompareTo(object pair)
            {
                if (!(pair is HalfPair)) throw new Exception("Exception 284401");
                HalfPair pa = (HalfPair)pair;
                int cmp = this.hkey.CompareTo(pa.hkey);
                if (cmp != 0) return cmp;
                if (index.Table.Count() == 0) throw new Exception("Ex: 2943991");
                // Определяем ключ 
                PaEntry entry = index.Table.Element(0);
                entry.offset = pa.record_off;
                Tkey key = index.KeyProducer((object[])entry.Get());
                entry.offset = record_off;
                return index.KeyProducer((object[])entry.Get()).CompareTo(key);
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
            if (KeyProducer == null) throw new Exception("Err: KeyProducer not defined");
            Table.Scan((offset, o) =>
            {
                var key = KeyProducer(o);
                int hkey = (int)HalfProducer(key);
                index_cell.Root.AppendElement(new object[] { hkey, offset });
                return true;
            });
            index_cell.Flush();
            if (index_cell.Root.Count() == 0) return; // потому что следующая операция не пройдет
            var ptr = Table.Element(0);
            index_cell.Root.SortByKey<HalfPair>((object v) =>
            {
                object[] vv = (object[])v;
                object half_key = vv[0];
                long offset = (long)vv[1];
                ptr.offset = offset;
                return new HalfPair(offset, (int)half_key, this);
            });

            if (Scale != null) Scale.Build();
        }
        public void BuildScale() { Scale.Build(); }

        public void Warmup() { foreach (var v in index_cell.Root.ElementValues()); if (Scale != null) Scale.Warmup(); }

        public IEnumerable<PaEntry> GetAllByKey(long start, long number, Tkey key)
        {
            if (Table == null || Table.Count() == 0) return Enumerable.Empty<PaEntry>();
            PaEntry entry = Table.Element(0);
            PaEntry entry1 = entry;
            int hkey = HalfProducer(key);
            var candidates = index_cell.Root.BinarySearchAll(start, number, ent =>
            {
                object[] pair = (object[])ent.Get();
                int hk = (int)pair[0];
                int cmp = hk.CompareTo(hkey);
                if (cmp != 0) return cmp;
                long off = (long)pair[1];
                entry.offset = off;
                return ((IComparable)KeyProducer((object[])entry.Get())).CompareTo(key);
            });
            return candidates.Select(ent =>
            {
                entry1.offset = (long)ent.Field(1).Get();
                return entry1;
            });
        }

        public IEnumerable<PaEntry> GetAllByKey(Tkey key)
        {
            if (Scale != null)
            {
                Diapason dia = Scale.GetDiapason(HalfProducer(key));
                if (dia.numb == 0) return Enumerable.Empty<PaEntry>();
                else return GetAllByKey(dia.start, dia.numb, key);
            }
            return GetAllByKey(0, index_cell.Root.Count(), key);
        }
        public long Count() { return index_cell.Root.Count(); }
    }
}
