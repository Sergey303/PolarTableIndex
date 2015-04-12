namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlStrLength  : SparqlExpression
    {
        public SparqlStrLength(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;

            Func = result => value.Func(result).Content.Length;
        }
    }
}
