using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlIriExpression : SparqlExpression
    {
        public SparqlIriExpression(IUriNode sparqlUriNode)
        {  
            Func = result => sparqlUriNode;
        }
    }
}
