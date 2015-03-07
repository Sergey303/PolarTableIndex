﻿using System;
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

        public void Warmup()
        {
           subIndex.Warmup();
        }

        public IEnumerable<PaEntry> GetAllByKey(long start, long number, TStrKey key)
        {
            return subIndex.GetAllByKey(start, number, hKeyProducer(key)).Where(entry =>key.Equals(fullKeyGenerator((object[]) entry.Get())));
        }

      

        public IEnumerable<PaEntry> GetAllByKey(TStrKey key)
        {
            return subIndex.GetAllByKey(hKeyProducer(key)).Where(entry =>key.Equals(fullKeyGenerator((object[]) entry.Get()))); 
        }
    }
 

}
