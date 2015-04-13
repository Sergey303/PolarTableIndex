namespace SparqlParseRun.RdfCommon
{
    public interface IStore : IGraph 
    {
        //IStoreNamedGraphs NamedGraphs { get; }
        IStoreNamedGraphs NamedGraphs { get; }


        void ClearAll();
        void Close();
    }
}