using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlIsIri : SparqlExpression
    {
        private SparqlExpression sparqlExpression;

        public SparqlIsIri(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
          
            Func = result => sparqlExpression.Func(result) is IUriNode;

        }
    }
}
