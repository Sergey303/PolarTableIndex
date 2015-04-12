using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlHours : SparqlExpression
    {
        public SparqlHours(SparqlExpression value)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is DateTime)
                    return ((DateTime)f).Hour;
                throw new ArgumentException();
            };
        }
    }
}
