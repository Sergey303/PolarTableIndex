using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlCeil : SparqlExpression
    {
        public SparqlCeil(SparqlExpression value)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var val = value.Func(result);
                if(val is decimal || val is double)
                return Math.Ceiling(val);
                if (val is float)
                    return (float)Math.Ceiling((double) val);
                if (val is int)
                    return val;
                throw new ArgumentException("Ceil " + val);
            };
        }
    }
}