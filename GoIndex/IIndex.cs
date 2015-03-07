using System;
using System.Collections.Generic;
using PolarDB;

namespace GoIndex
{
    public interface IIndex<Tkey> where Tkey : IComparable
    {
        void Build();
        void Warmup();
        IEnumerable<PaEntry> GetAllByKey(Tkey key);
    }
}