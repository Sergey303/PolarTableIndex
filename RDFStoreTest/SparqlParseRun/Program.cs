using System;
using System.IO;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses;

namespace SparqlParseRun
{
public    class Program
    {

    static void Main(string[] args)
        {
            var rdfInMemoryStore = new RamListOftriplesStore("http://default");
            SparqlQuery sparqlQuery = SparqlQueryParser.Parse(rdfInMemoryStore, File.ReadAllText(
                //@"C:\Users\Admin\Source\Repos\PolarDemo\SparqlParser\sparql data\queries\with constants\9.rq"
                @"C:\Users\Admin\Source\Repos\dotnetrdf_test2\UnitTestDotnetrdf_test\examples\2.5 Creating Values with Expressions\query2.rq"
                ));

            using (StreamReader file = new StreamReader(
                @"C:\Users\Admin\Source\Repos\dotnetrdf_test2\UnitTestDotnetrdf_test\examples\2.5 Creating Values with Expressions\data.ttl" 
                    //@"C:\deployed\1M.ttl"
))                         TurtleParser.FromTurtle(rdfInMemoryStore, file.BaseStream);
            var sparqlResultSet = sparqlQuery.Run(rdfInMemoryStore);
            //var enumerable = sparqlResultSet.GraphResult.GetTriples();
            var enumerable = sparqlResultSet.Results;
            Console.WriteLine(sparqlResultSet.ToXml());
        }

        public static IStore LoadStore(string filePath, IStore store)
        {             
            store.ClearAll();
            using (StreamReader file = new StreamReader(filePath))
                TurtleParser.FromTurtle(store, file.BaseStream);
            return store;
        }

        public static string RunQuery(IStore store, string query)
        {                      
            SparqlQuery sparqlQuery = SparqlQueryParser.Parse(store, query);

            if (sparqlQuery==null)
            {
                return "";
            }
            var sparqlResultSet = sparqlQuery.Run(store);
            //var enumerable = sparqlResultSet.GraphResult.GetTriples();
            //var enumerable = sparqlResultSet.Results;
           return sparqlResultSet.ToXml().ToString();
        }
    }


}
