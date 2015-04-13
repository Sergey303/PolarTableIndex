using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlLiteralExpression : SparqlExpression
    {
        private ILiteralNode sparqlLiteralNode;

        public SparqlLiteralExpression(ILiteralNode sparqlLiteralNode)
        {
            // TODO: Complete member initialization
            this.sparqlLiteralNode = sparqlLiteralNode;
            Func = result => sparqlLiteralNode.Content is string ? sparqlLiteralNode : sparqlLiteralNode.Content;
        }
    }
}
