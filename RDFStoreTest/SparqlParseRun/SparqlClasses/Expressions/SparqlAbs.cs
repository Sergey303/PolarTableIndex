using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlAbs : SparqlExpression
    {
        public SparqlAbs(SparqlExpression value)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result => Math.Abs(value.Func(result));
        }
    }
}