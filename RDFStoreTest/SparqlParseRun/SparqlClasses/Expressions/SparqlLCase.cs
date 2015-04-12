using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlLCase :SparqlExpression
    {
        private SparqlExpression sparqlExpression;

        public SparqlLCase(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is SimpleLiteralNode)
                {
                    f.Content = f.Content.ToLower();
                    return f;
                }
                throw new ArgumentException();
            };
        }
    }
}
