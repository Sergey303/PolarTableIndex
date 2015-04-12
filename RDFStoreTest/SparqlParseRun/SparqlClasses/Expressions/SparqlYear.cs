using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlYear : SparqlExpression
    {
        public SparqlYear(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is DateTime)
                    return ((DateTime)f).Year;
                throw new ArgumentException();
            };
          
        }
    }
}
