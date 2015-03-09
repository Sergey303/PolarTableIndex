using System;
using IndexCommon;
using PolarDB;

namespace GoIndex
{
    public class HalfPair<Tkey> : IComparable where Tkey : IComparable
    {
        private long record_off;
        private int hkey;
        private IIndex<Tkey> index;
        public HalfPair(long rec_off, int hkey, IIndex<Tkey> index)
        {
            this.record_off = rec_off; this.hkey = hkey; this.index = index;
        }
        public int CompareTo(object pair)
        {
            if (!(pair is HalfPair<Tkey>)) throw new Exception("Exception 284401");
            HalfPair<Tkey> pa = (HalfPair<Tkey>)pair;
            int cmp = this.hkey.CompareTo(pa.hkey);
            if (cmp != 0) return cmp;
            if (index.Table.Count() == 0) throw new Exception("Ex: 2943991");
            // Определяем ключ 
            PaEntry entry = index.Table.Element(0);
            entry.offset = pa.record_off;
            Tkey key = index.KeyProducer(entry);
            entry.offset = record_off;
            return index.KeyProducer(entry).CompareTo(key);
        }
    }
}