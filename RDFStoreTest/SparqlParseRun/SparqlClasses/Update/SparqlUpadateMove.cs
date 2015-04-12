using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateMove : SparqlUpdateSilent
    {
        public IUriNode To;
        public IUriNode From;
        public override void RunUnSilent(IStore store)
        {
            if ((From == null && To == null) || (From != null && From.Equals(To))) return;
            IGraph fromGraph, toGraph;
            if (From == null) fromGraph = store;
            else
            {
                fromGraph = store.NamedGraphs.GetGraph(From);
                if (!fromGraph.Any()) throw new NoGraphExeption(From);
            }
            if (To == null)
            {
                foreach (var triple in fromGraph.GetTriples())
                    store.Add(triple);
                fromGraph.Clear();
            }
            else //if (!store.NamedGraphs.ContainsGraph(To)) 
                store.NamedGraphs.AddGraph(To, fromGraph);
            //else store.NamedGraphs.ReplaceGraph(To,fromGraph);
        }
    }
}
