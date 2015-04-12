namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlOrExpression : SparqlExpression
    {
        public SparqlOrExpression(SparqlExpression sparqlExpression1, SparqlExpression sparqlExpression2)
        {
            IsAggragate = sparqlExpression1.IsAggragate || sparqlExpression2.IsAggragate;
            IsDistinct = sparqlExpression1.IsDistinct || sparqlExpression2.IsDistinct;
            Func = result => sparqlExpression1.Func(result) || sparqlExpression2.Func(result);
        }
    }
}
