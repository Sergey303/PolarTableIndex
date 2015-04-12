using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node
{
    public class SparqlUnDefinedNode : INode
    {
        public NodeType Type
        {
            get { return NodeType.Undefined; }
        }
    }
}