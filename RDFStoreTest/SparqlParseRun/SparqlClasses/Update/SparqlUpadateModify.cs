using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateModify : ISparqlUpdate
    {
        private SparqlGraphPattern @where;
        private IUriNode with;
        private SparqlQuardsPattern insert;
        private SparqlQuardsPattern delete;
        private RdfQuery11Translator q;

        public SparqlUpdateModify(RdfQuery11Translator q)
        {
            this.q = q;
        }

        internal void SetInsert(SparqlQuardsPattern sparqlUpdateInsert)
        {
            insert = sparqlUpdateInsert;
        }

        internal void SetDelete(SparqlQuardsPattern sparqlUpdateDelete)
        {
            delete = sparqlUpdateDelete;
        }

        internal void SetWhere(SparqlGraphPattern sparqlGraphPattern)
        {
            @where = sparqlGraphPattern;
        }

        public void Run(IStore store)
        {
          
            var results = @where.Run(Enumerable.Repeat(new SparqlResult(), 1)).ToArray();
            if (delete != null)
            {
                if (with == null)
                    store.Delete(results.SelectMany(result =>
                        delete.Where(pattern => pattern.PatternType == SparqlGraphPatternType.SparqlTriple)
                            .Cast<SparqlTriple>()
                            .Select(triple => triple.Substitution(result,store.Name))));
                else //if (store.NamedGraphs.ContainsGraph(with))
                    store.NamedGraphs.Delete(with, results.SelectMany(result =>
                        delete.Where(pattern => pattern.PatternType == SparqlGraphPatternType.SparqlTriple)
                            .Cast<SparqlTriple>()
                            .Select(triple => triple.Substitution(result,with.UriString))));      

                foreach (var sparqlGraphGraph in delete.Where(pattern => pattern.PatternType == SparqlGraphPatternType.Graph)
                        .Cast<SparqlGraphGraph>()
                        //.Where(graph => store.NamedGraphs.ContainsGraph(graph.Name))
                        )
                {
                    SparqlGraphGraph graph = sparqlGraphGraph;
                    store.NamedGraphs.Delete(graph.Name,
                        results.SelectMany(result =>
                            graph.GetTriples()
                                .Select(triple => triple.Substitution(result, graph.Name.UriString))));
                }
            }
            if (insert != null)
            {
                if (with == null)
                store.Insert(results.SelectMany(result =>
                    insert.Where(pattern => pattern.PatternType == SparqlGraphPatternType.SparqlTriple)
                        .Cast<SparqlTriple>()
                        .Select(triple => triple.Substitution(result, store.Name))));
                else
                    store.NamedGraphs.Insert(with, results.SelectMany(result => 
                        insert.Where(pattern => pattern.PatternType == SparqlGraphPatternType.SparqlTriple)
                              .Cast<SparqlTriple>()
                              .Select(triple => triple.Substitution(result,with.UriString))));

                foreach (
                    var sparqlGraphGraph in
                        insert.Where(pattern => pattern.PatternType == SparqlGraphPatternType.Graph)
                            .Cast<SparqlGraphGraph>())

                {
                    SparqlGraphGraph graph = sparqlGraphGraph;
                    store.NamedGraphs.Insert(graph.Name,
                        results.SelectMany(result =>
                            graph.GetTriples()
                                .Select(triple => triple.Substitution(result,graph.Name.UriString))));
                }
            }
        }
    }
}
