using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateAdd : SparqlUpdateSilent
    {
        public IUriNode To;
        public IUriNode From;

        public override void RunUnSilent(IStore store)
        {
            if ((From == null && To == null) || From != null && From.Equals(To)) return;
            IGraph fromGraph, toGraph;
            if (From == null) fromGraph = store;
            else
            {
                fromGraph = store.NamedGraphs.GetGraph(From);
                if (!fromGraph.Any()) throw new NoGraphExeption(From);
            }
            if (To == null) toGraph = store;
            else
                toGraph = store.NamedGraphs.GetGraph(To);// ?? store.NamedGraphs.CreateGraph(To);

            foreach (var t in fromGraph.GetTriples())
                toGraph.Add(t);
        }
    }

 
}
