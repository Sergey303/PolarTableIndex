using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolarDB;

namespace GoIndex
{
    class Index0<Tkey> where Tkey: IComparable
    {
        internal PaEntry table;
        internal Func<PaEntry, Tkey> keyProducer;
        public Index0(string indexName, PaEntry table, Func<PaEntry, Tkey> keyProducer)
        {
            this.table = table;
            this.keyProducer = keyProducer;
        }
    }
}
