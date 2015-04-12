using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SparqlParseRun.RdfCommon
{
    public class Grouping<Tkey, TValue> : IGrouping<Tkey, TValue>
    {
        private IEnumerable<TValue> sequence;

        public Grouping(Tkey key, IEnumerable<TValue> sequence)
        {
            this.sequence = sequence;
            Key = key;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return sequence.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Tkey Key { get; private set; }
    }
}