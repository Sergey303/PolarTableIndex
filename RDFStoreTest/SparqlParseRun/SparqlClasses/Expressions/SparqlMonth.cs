using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlMonth : SparqlExpression
    {
        private SparqlExpression sparqlExpression;

        public SparqlMonth(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is DateTime)
                    return ((DateTime)f).Month;
                throw new ArgumentException();
            };
        }
    }
}
