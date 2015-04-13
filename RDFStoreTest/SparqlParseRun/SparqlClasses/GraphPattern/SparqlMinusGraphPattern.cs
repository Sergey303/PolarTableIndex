using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.GraphPattern
{
    public class SparqlMinusGraphPattern : ISparqlGraphPattern
    {
        private readonly ISparqlGraphPattern sparqlGraphPattern;

        public SparqlMinusGraphPattern(ISparqlGraphPattern sparqlGraphPattern)
        {
            // TODO: Complete member initialization
            this.sparqlGraphPattern = sparqlGraphPattern;
        }

        public IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings)
        {
            var minusResults =
                sparqlGraphPattern.Run(Enumerable.Repeat(new SparqlResult(new Dictionary<VariableNode, SparqlVariableBinding>()), 1))
                    .ToArray();
            SparqlVariableBinding value;
            return variableBindings.Where(result => minusResults.All(minusResult =>
                minusResult.row.All(minusVar => 
                    !result.row.TryGetValue(minusVar.Key, out value) 
                    ||
                    !Equals(minusVar.Value.Value, value.Value))));
        }

        public SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.Minus;} }
    }
}
