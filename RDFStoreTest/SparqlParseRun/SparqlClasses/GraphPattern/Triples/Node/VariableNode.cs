using System;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node
{
    public class VariableNode : INode, IVariableNode, ISubjectNode, IUriNode
    {
        public readonly string VariableName;
        //public NodeType NodeType { get { return NodeType.Variable; } }
        //public INode Value;
        //  public int index;

        public VariableNode(string variableName)
        {
            VariableName = variableName;
        }

        protected VariableNode()
        {
        }

        public NodeType Type
        {
            get { return NodeType.Variable; }
        }

        public Uri Uri
        {
            get { throw new Exception(); }
        }

        public string UriString
        {
            get { throw new Exception(); }
        }

        public UriPrefixed UriPrefixed { get{throw new Exception();}}

        public string ToStringWithBraces()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}