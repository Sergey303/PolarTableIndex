using System.Collections.Generic;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.Expressions;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;

namespace SparqlParseRun
{
    public class RdfQuery11Translator 
    {
        public Dictionary<string, VariableNode> Variables = new Dictionary<string, VariableNode>();
        public Dictionary<string, SparqlBlankNode> BlankNodes = new Dictionary<string, SparqlBlankNode>();
        public DataSet ActiveGraphs = new DataSet();
      public DataSet NamedGraphs = new DataSet();
      public readonly Prologue prolog = new Prologue();
        //   public IUriNode With;
        public IStore Store
        {
            get { return store; }
            set
            {
                store = value;
                StoreCalls = new SparqlTripletsStoreCalls(Store);                
            }
        }

     

        public ISparqlTripletsStoreCalls StoreCalls;
        private IStore store;

        public RdfQuery11Translator(IStore store1)
        {
            Store = store1;
        }

        internal VariableNode GetVariable(string p)
        {
            VariableNode variable;
            if (Variables.TryGetValue(p, out variable)) return variable;
            Variables.Add(p, variable=new VariableNode(p));
            return variable;
        }


        //internal IEnumerable<VariableNode> GetVariables(int p)
        //{
        //    return Variables.Values.Cast<>.Skip(p);
        //}

        internal IVariableNode CreateExpressionAsVariable(VariableNode variableNode, SparqlExpression sparqlExpression)
        {
            return new SparqlExpressionAsVariable(variableNode, sparqlExpression, this);
        }



        internal DataSet SetNamedGraphOrVariable(IUriNode sparqlNode, DataSet namedDataSet)
        {
       
            VariableNode graphVariable = sparqlNode as VariableNode;
            return graphVariable != null ? new VariableDataSet(graphVariable, namedDataSet) : new DataSet(){sparqlNode};
        
        }

        public INode CreateLiteralIfNotNode(dynamic value)
        {
                        if (value is INode) return value;
                        return store.NodeGenerator.CreateLiteralNode(value);
        }
        public new SparqlBlankNode CreateBlankNode(string blankNodeString)
        {
            SparqlBlankNode blankNode;
            if (BlankNodes.TryGetValue(blankNodeString, out blankNode)) return blankNode;
            BlankNodes.Add(blankNodeString, blankNode = new SparqlBlankNode(blankNodeString));
            return blankNode;
        }

        public new SparqlBlankNode CreateBlankNode()
        {
            var sparqlBlankNode = new SparqlBlankNode();  
            return sparqlBlankNode;
        }
    }
}
