using System;
using System.Collections.Generic;

namespace SparqlParseRun.SparqlClasses.Query.Result
{
    public class CollectionEqualityComparer : IEqualityComparer<Array>
    {
        public bool Equals(Array x, Array y)
        {
            if(x.Length!=y.Length) return false;
            for (int i = 0; i < x.Length; i++)
                if (! x.GetValue(i).Equals(y.GetValue(i))) return false;
            return true;
        }

        public int GetHashCode(Array obj)
        {
            unchecked
            {
                int sum=0;
                for (int i = 0; i < obj.Length; i++)
                {
                    sum += obj.GetValue(i).GetHashCode();
                }
                return sum;
            }
        }

     
    }
}