using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node
{
    public class SparqlBlankNode : VariableNode, IBlankNode
    {


        public SparqlBlankNode(string  varName):base(varName)
        {
            
        }

        public SparqlBlankNode()
        {
         
        }


        public string Name
        {
            get { return VariableName; }
        }


        public NodeType Type { get; private set; }
    }
}