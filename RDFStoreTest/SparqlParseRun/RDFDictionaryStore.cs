using SparqlParseRun.RdfCommon;

namespace SparqlParseRun
{
    public class RamDictionaryStore : RamDictionaryGraph, IStore
    {
        private readonly RdfNamedGraphs namedGraphs =new RdfNamedGraphs();
        public IStoreNamedGraphs NamedGraphs { get { return namedGraphs; } }
        public void ClearAll()
        {
            Clear();
            namedGraphs.ClearAllNamedGraphs();
        }

        public void Close()
        {
            
        }
    }

}