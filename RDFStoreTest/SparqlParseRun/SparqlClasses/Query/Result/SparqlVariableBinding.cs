using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;

namespace SparqlParseRun.SparqlClasses.Query.Result
{
    public class SparqlVariableBinding
    {
        public readonly VariableNode Variable;
        private readonly INode value;


        public SparqlVariableBinding(VariableNode variable, INode Subj)
        {
            // TODO: Complete member initialization
            Variable = variable;
            value = Subj;
        }

        public INode Value { get { return value; } }
    }
}
