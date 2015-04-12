using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateDeleteWhere : ISparqlUpdate
    {
        private readonly SparqlQuardsPattern sparqlQuardsPattern;

        public SparqlUpdateDeleteWhere(SparqlQuardsPattern sparqlQuardsPattern)
        {
            // TODO: Complete member initialization
            this.sparqlQuardsPattern = sparqlQuardsPattern;
        }

        public  void Run(IStore store)
        {
            var results = sparqlQuardsPattern.Run(Enumerable.Repeat(new SparqlResult(), 1)).ToArray();
            store.Delete( results.SelectMany(result =>
                sparqlQuardsPattern.Where(pattern => Equals(pattern.PatternType, SparqlGraphPatternType.SparqlTriple))
                    .Cast<SparqlTriple>()
                    .Select(triple => triple.Substitution(result, store.Name))));


            foreach (var sparqlGraphPattern in
                sparqlQuardsPattern.Where(pattern => Equals(pattern.PatternType, SparqlGraphPatternType.Graph)))
            {
                var sparqlGraphGraph = (SparqlGraphGraph) sparqlGraphPattern;


                if (sparqlGraphGraph.Name == null)
                    store.Delete(results.SelectMany(result =>
                        sparqlGraphGraph.GetTriples()
                            .Select(triple => triple.Substitution(result, ((IGraph)store).Name))));
                store.NamedGraphs.Delete(sparqlGraphGraph.Name, results.SelectMany(result =>
                    sparqlGraphGraph.GetTriples()
                        .Select(triple => triple.Substitution(result, sparqlGraphGraph.Name.UriString))));
            }
        }

    }
}
