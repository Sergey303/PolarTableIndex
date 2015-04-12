using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlDay : SparqlExpression
    {
        private SparqlExpression sparqlExpression;

        public SparqlDay(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is DateTime)
                    return ((DateTime)f).Day;
                throw new ArgumentException();
            };
        }
    }
}
