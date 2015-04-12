using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.InlineValues
{
    public class SparqlInline : ISparqlGraphPattern
    {
        private readonly List<VariableNode> variables = new List<VariableNode>();
        private int currentVariableIndex=0; 
        public List<SparqlVariableBinding[]> VariablesBindingsList=new List<SparqlVariableBinding[]>();
        internal void AddVar(VariableNode variableNode)
        {
            variables.Add(variableNode);
        }

        internal void AddValue(INode sparqlNode)
        {
              if (currentVariableIndex == 0)
                   VariablesBindingsList.Add(new SparqlVariableBinding[variables.Count]);
            if (sparqlNode is SparqlUnDefinedNode) { currentVariableIndex++; return; }
            VariablesBindingsList.Last()[currentVariableIndex] = new SparqlVariableBinding(variables[currentVariableIndex], sparqlNode);
            currentVariableIndex++;     
        }

        internal void NextListOfVarBindings()
        {
            currentVariableIndex = 0;
          
        }

        public IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> bindings)
        {
            SparqlVariableBinding exists;
            foreach (SparqlResult result in bindings)
            {
                foreach (var arrayofBindings in VariablesBindingsList)
                {
                    bool iSContinue = false;
                    var newResult =
                        new Dictionary<VariableNode, SparqlVariableBinding>(result.row);
                    foreach (var sparqlVariableBinding in arrayofBindings.Where(binding => binding!=null))
                        if (result.row.TryGetValue(sparqlVariableBinding.Variable, out exists))
                        {
                            if (exists.Value.Equals(sparqlVariableBinding.Value)) continue;
                            iSContinue = true;
                            break;
                        }
                        else newResult.Add(sparqlVariableBinding.Variable, sparqlVariableBinding);
                    if (iSContinue) continue;
                    yield return new SparqlResult(newResult);  
                }
            }
        }

        public SparqlGraphPatternType PatternType { get { return SparqlGraphPatternType.InlineDataValues; } }

    }
}
