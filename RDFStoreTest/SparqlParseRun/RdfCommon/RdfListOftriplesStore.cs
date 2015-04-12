namespace SparqlParseRun.RdfCommon
{
    public class RamListOftriplesStore : RamListOfTriplesGraph, IStore
    {
        readonly RdfNamedGraphs graphs=new RdfNamedGraphs();

        public RamListOftriplesStore(string name) : base(name)
        {
        }

        public IStoreNamedGraphs NamedGraphs { get { return graphs; } }
        public void ClearAll()
        {
            Clear();
            NamedGraphs.ClearAllNamedGraphs();
        }

        public void Close()
        {
            
        }
    }
}