using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlIf : SparqlExpression
    {
        public SparqlIf(SparqlExpression conditionExpression1, SparqlExpression sparqlExpression2, SparqlExpression sparqlExpression3)
        {
            IsDistinct = conditionExpression1.IsDistinct || sparqlExpression2.IsDistinct || sparqlExpression3.IsDistinct;
            IsAggragate = conditionExpression1.IsAggragate || sparqlExpression2.IsAggragate || sparqlExpression3.IsAggragate;
            Func = result =>
            {
                var condition = conditionExpression1.Func(result);
                if (condition is bool)
                {
                    return condition ? sparqlExpression2.Func(result) : sparqlExpression3.Func(result);
                }   throw new ArgumentException();
            };
        }
    }
}
