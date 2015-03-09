using System;
using System.Collections.Generic;
using PolarDB;

namespace IndexCommon
{
  
    public interface IIndex<Tkey>
    {
        void Build();
        void Warmup();
        IEnumerable<PaEntry> GetAllByKey(long start, long number, Tkey key);
        IEnumerable<object[]> GetAllReadedByKey(long start, long number, Tkey key);
        IEnumerable<PaEntry> GetAllByKey(Tkey key);
        IEnumerable<object[]> GetAllReadedByKey(Tkey key);
        PaEntry Table { get; }
        Tkey KeyProducer(PaEntry entry);
        long Count();
        void Build2();
    }
}