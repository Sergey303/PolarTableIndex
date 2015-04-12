using System.Linq;
using SparqlParseRun.SparqlClasses.Expressions;
using SparqlParseRun.SparqlClasses.GraphPattern;

namespace SparqlParseRun.SparqlClasses
{
    public class SparqlExistsExpression :SparqlExpression
    {
        //private SparqlGraphPattern sparqlGraphPattern;

        public SparqlExistsExpression(ISparqlGraphPattern sparqlGraphPattern)
        {
            // TODO: Complete member initialization
            //this.sparqlGraphPattern = sparqlGraphPattern;
            Func=variableBinding => sparqlGraphPattern.Run(Enumerable.Repeat(variableBinding, 1)).Any();
        }

    }
}
