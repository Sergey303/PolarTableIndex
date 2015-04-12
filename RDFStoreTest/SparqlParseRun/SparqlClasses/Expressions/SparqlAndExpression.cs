namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlAndExpression : SparqlExpression
    {
        public SparqlAndExpression(SparqlExpression l, SparqlExpression r)
        {
            IsDistinct = l.IsDistinct || r.IsDistinct;
            IsAggragate = l.IsAggragate || r.IsAggragate;
            // TODO: Complete member initialization
            Func = result => l.Func(result) && r.Func(result);
        }
    }
}
