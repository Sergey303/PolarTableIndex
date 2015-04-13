using System;
using System.Net;
using System.Xml.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateLoad : SparqlUpdateSilent
    {
        private IUriNode from;
        public IUriNode Graph;

        internal void SetIri(IUriNode sparqlUriNode)
        {
           from = sparqlUriNode;
        }

        internal void Into(IUriNode sparqlUriNode)
        {
            Graph = sparqlUriNode;
        }


        public override void RunUnSilent(IStore store)
        {
            using (WebClient wc = new WebClient())
            {
                //  wc.Headers[HttpRequestHeader.ContentType] = "application/sparql-query"; //"query="+ 
                string gString = wc.DownloadString(from.UriString);
                var graph = (Graph != null)
                    ? store.NamedGraphs.CreateGraph(Graph)
                    : store;
                try
                {
                    var gXml = XElement.Parse(gString);
                    graph.FromXml(gXml);
                }
                catch (Exception)
                {
                    try
                    {
                        graph.FromTurtle(gString);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
