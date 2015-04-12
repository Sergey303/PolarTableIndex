using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlMinutes : SparqlExpression
    {
        private SparqlExpression sparqlExpression;

        public SparqlMinutes(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
           Func = result =>
            {
                var f = value.Func(result);
                if (f is DateTime)
                    return ((DateTime)f).Minute;
                throw new ArgumentException();
            };
        }
    }
}
