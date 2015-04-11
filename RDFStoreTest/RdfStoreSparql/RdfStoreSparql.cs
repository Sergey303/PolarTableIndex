
using System.IO;
using SparqlParseRun;
using SparqlParseRun.SparqlClasses;
using SparqlParseRun.SparqlClasses.Query.Result;

public class RdfStoreSparql
{
    public RDFStoreQuadsCoded store;

    public RdfStoreSparql()
    {
        store =new RDFStoreQuadsCoded();   
    }

    public SparqlResultSet ParseRunSparql(string query)
    {
      var q=  SparqlQueryParser.Parse(store, query);
       return q.Run(store); 
    }
    public SparqlResultSet ParseRunSparql(Stream query)
    {
        var q = SparqlQueryParser.Parse(store, query);
        return q.Run(store);
    }
    
   public void ReCreateFrom(Stream ttl)
    {
        store.ClearAll();
        store.FromTurtle(ttl);
               store.spogdTable.Flush();
       store.nodeGenerator. nameTable.Flush();
       store.Build();
    }

}

