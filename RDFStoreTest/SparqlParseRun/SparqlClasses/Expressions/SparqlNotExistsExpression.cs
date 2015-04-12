using SparqlParseRun.SparqlClasses.GraphPattern;

namespace SparqlParseRun.SparqlClasses
{
    public class SparqlNotExistsExpression : SparqlExistsExpression
    {

        public SparqlNotExistsExpression(ISparqlGraphPattern sparqlGraphPattern)
            : base(sparqlGraphPattern)
        {
            // TODO: Complete member initialization
            //this.sparqlGraphPattern = sparqlGraphPattern;
            var funcClone = FunkClone;
            Func = result => !funcClone(result);
        }
    }
}
