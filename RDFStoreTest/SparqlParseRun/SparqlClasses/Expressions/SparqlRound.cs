using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlRound : SparqlExpression
    {
        private SparqlExpression sparqlExpression;

        public SparqlRound(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
          
            sparqlExpression = value;
            Func = result =>
            {
                var val = value.Func(result);
                if (val is decimal || val is double)
                    return Math.Round(val);
                if (val is float)
                    return (float)Math.Round((double)val);
                if (val is int)
                    return val;
                throw new ArgumentException("Round " + val);
            };
        }
    }
}
