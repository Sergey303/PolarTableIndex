using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlIsBlank : SparqlExpression
    {
        private SparqlExpression sparqlExpression;

        public SparqlIsBlank(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
          
            Func = result => sparqlExpression.Func(result) is SparqlBlankNode;
        }
    }
}
