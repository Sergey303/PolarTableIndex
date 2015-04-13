using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlFloor : SparqlExpression
    {
        public SparqlFloor(SparqlExpression value)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
          
            Func = result =>
            {
                var val = value.Func(result);
                if (val is decimal || val is double)
                    return Math.Floor(val);
                if (val is float)
                    return (float)Math.Floor((double)val);
                if (val is int)
                    return val;
                throw new ArgumentException("Ceil " + val);
            };
        }
    }
}