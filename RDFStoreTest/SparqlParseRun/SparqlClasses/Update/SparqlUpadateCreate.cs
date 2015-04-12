using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateCreate : SparqlUpdateSilent
    {
        public IUriNode Graph;
        public override void RunUnSilent(IStore store)
        {
            store.NamedGraphs.CreateGraph(Graph);
        }
    }
}
