using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateInsertData : ISparqlUpdate
    {
        private readonly SparqlQuardsPattern sparqlQuardsPattern;

        public SparqlUpdateInsertData(SparqlQuardsPattern sparqlQuardsPattern)
        {
            // TODO: Complete member initialization
            this.sparqlQuardsPattern = sparqlQuardsPattern;
        }
        public  void Run(IStore store)
        {
            store.Insert( sparqlQuardsPattern.Where(pattern => pattern.PatternType == SparqlGraphPatternType.SparqlTriple).Cast<Triple>());
            foreach (var sparqlGraph in
                    sparqlQuardsPattern.Where(pattern => pattern.PatternType == SparqlGraphPatternType.Graph)
                        .Cast<SparqlGraphGraph>()
                        //.Where(graph => store.NamedGraphs.ContainsGraph(graph.Name))
                        )
                store.NamedGraphs.Insert(sparqlGraph.Name, sparqlGraph.GetTriples());
        }
    }
}
