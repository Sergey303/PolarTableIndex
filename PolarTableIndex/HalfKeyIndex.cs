using System;
using System.Collections.Generic;
using System.Linq;
using IndexCommon;
using PolarDB;

namespace PolarTableIndex
{
    public class HalfKeyForIndex<TStrKey, THalfKey>  :IIndex<TStrKey>
    {
        private readonly Func<object[], TStrKey> fullKeyGenerator;
        private readonly IIndex<THalfKey> subIndex;
        private readonly Func<TStrKey, THalfKey> hKeyProducer;

        public HalfKeyForIndex(Func<object[], TStrKey> fullKeyGenerator, Func<TStrKey,THalfKey> hKeyProducer, IIndex<THalfKey> subIndex)
        {
            this.fullKeyGenerator = fullKeyGenerator;
            this.subIndex = subIndex;
            this.hKeyProducer = hKeyProducer;
           }

        public void Build()
        {
           subIndex.Build();
        }
        public void BuildScale(int N) { }

        public void Warmup()
        {
           subIndex.Warmup();
        }

        public IEnumerable<PaEntry> GetAllByKey(long start, long number, TStrKey key)
        {
            return subIndex.GetAllByKey(start, number, hKeyProducer(key)).Where(entry =>key.Equals(fullKeyGenerator((object[]) entry.Get())));
        }

        public IEnumerable<object[]> GetAllReadedByKey(long start, long number, TStrKey key)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<PaEntry> GetAllByKey(TStrKey key)
        {
            return subIndex.GetAllByKey(hKeyProducer(key)).Where(entry =>key.Equals(fullKeyGenerator((object[]) entry.Get()))); 
        }

        public IEnumerable<object[]> GetAllReadedByKey(TStrKey key)
        {
            return subIndex.GetAllReadedByKey(hKeyProducer(key)).Where(entry => key.Equals(fullKeyGenerator(entry))); 
        }

        public PaEntry Table { get { return subIndex.Table; } }
        public TStrKey KeyProducer(PaEntry entry)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public void Build2()
        {
            throw new NotImplementedException();
        }

        public void Build3()
        {
            throw new NotImplementedException();
        }
    }
 

}

