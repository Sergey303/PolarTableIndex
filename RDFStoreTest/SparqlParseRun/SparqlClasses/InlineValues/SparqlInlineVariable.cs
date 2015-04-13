using System.Collections.Generic;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.InlineValues
{
    public class SparqlInlineVariable : ISparqlGraphPattern
    {
        private readonly VariableNode variableNode;

        public SparqlInlineVariable(VariableNode variableNode)
        {
            // TODO: Complete member initialization
            this.variableNode = variableNode;
        }
        public List<SparqlVariableBinding> VariableBindingsList = new List<SparqlVariableBinding>() ;

        internal void Add(INode sparqlNode)
        {
           VariableBindingsList.Add(new SparqlVariableBinding(variableNode, sparqlNode));
        }

        public IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> bindings)
        {
            SparqlVariableBinding exists;
            foreach (SparqlResult result in bindings)
                if (result.row.TryGetValue(variableNode, out exists))
                {
                    if (VariableBindingsList.Contains(exists)) yield return result; //TODO test
                }
                else
                    foreach (SparqlVariableBinding newvariableBinding in VariableBindingsList)
                        yield return
                            new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>(result.row)
                            {
                                {variableNode, newvariableBinding}
                            });
        }

        public SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.InlineDataValues;} }
    }
}
