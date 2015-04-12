using SparqlParseRun.RdfCommon;

namespace SparqlParseRun
{
    public class Rdf2DictionaryStore : RDF2DictionaryGraph, IStore
    {
        private readonly RdfNamedGraphs namedGraphs =new RdfNamedGraphs();

        public Rdf2DictionaryStore(string name) : base(name)
        {
        }

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