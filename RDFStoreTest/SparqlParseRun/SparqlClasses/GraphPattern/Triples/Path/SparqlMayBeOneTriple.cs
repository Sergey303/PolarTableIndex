using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Path
{
    public class SparqlMayBeOneTriple:  ISparqlGraphPattern
    {
        private readonly IEnumerable<ISparqlGraphPattern> triples;
        private readonly INode sNode;
        private readonly INode oNode;
        private readonly RdfQuery11Translator q;

        public SparqlMayBeOneTriple(IEnumerable<ISparqlGraphPattern> triples, INode s, INode o, RdfQuery11Translator q)
        {
            oNode = o;
            this.q = q;
            this.triples = triples;
            this.sNode = s;
        }

        public IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings)
        {
            var firstVar = sNode as VariableNode;
            var secondVar = oNode as VariableNode;
            SparqlVariableBinding firstVarValue;
            SparqlVariableBinding secondVarValue;
            INode s = null, o = null;
            var bindings = variableBindings as SparqlResult[] ?? variableBindings.ToArray();
            foreach (var variableBinding in bindings)
            {
                bool isSKnowns = true;
                bool isOKnowns = true;
                if (firstVar == null)
                {
                    s = sNode;
                }
                else if (variableBinding.row.TryGetValue(firstVar, out firstVarValue))
                {
                    s = firstVarValue.Value;
                }
                else isSKnowns = false;
                if (secondVar == null)
                {
                    o = sNode;
                }
                else if (variableBinding.row.TryGetValue(secondVar, out secondVarValue))
                {
                    o = secondVarValue.Value;
                }
                else isOKnowns = false;
                if (isSKnowns)
                {
                    if (isOKnowns)
                    {
                        if (s.Equals(o)) yield return variableBinding;
                    }  else
                     yield return new SparqlResult(variableBinding,s, secondVar);
                }
                else if (isOKnowns) yield return new SparqlResult(variableBinding, o, firstVar);
                else q.Store.GetAllSubjects();
            }
            foreach (var tr in triples.Aggregate(bindings, (current, sparqlGraphPattern) => sparqlGraphPattern.Run(current).ToArray()))
                yield return tr;
        }

        public SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.SparqlTriple;} }
    }
}