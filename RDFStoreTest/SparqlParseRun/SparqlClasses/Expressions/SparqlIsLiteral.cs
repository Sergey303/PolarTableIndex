using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlIsLiteral : SparqlExpression
    {
        public SparqlIsLiteral(SparqlExpression value)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;

            Func = result =>
            {
                var func = value.Func(result);
                return !(func is IUriNode || func is IBlankNode);
            };
        }
    }
}
