using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlNumLiteralExpression : SparqlExpression
    {
        public SparqlNumLiteralExpression(ILiteralNode sparqlLiteralNode)
        {
            Func = result => sparqlLiteralNode.Content;
        }
    }
}
