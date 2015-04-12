using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateDeleteData : ISparqlUpdate
    {
        private readonly SparqlQuardsPattern sparqlQuardsPattern;

        public SparqlUpdateDeleteData(SparqlQuardsPattern sparqlQuardsPattern)
        {
            // TODO: Complete member initialization
            this.sparqlQuardsPattern = sparqlQuardsPattern;
        }

        public void Run(IStore store)
        {
            store.Delete( sparqlQuardsPattern.Where(pattern => pattern.PatternType==SparqlGraphPatternType.SparqlTriple).Cast<Triple>());

            foreach (var sparqlGraphGraph in
                sparqlQuardsPattern.Where(pattern => Equals(pattern.PatternType, SparqlGraphPatternType.Graph))
                    .Cast<SparqlGraphGraph>())
            {
                if (sparqlGraphGraph.Name == null)
                    store.Delete(sparqlGraphGraph.GetTriples());
                store.NamedGraphs.Delete(sparqlGraphGraph.Name, sparqlGraphGraph.GetTriples());
            }
        }
    }
}
