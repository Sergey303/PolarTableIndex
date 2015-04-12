using System;

namespace SparqlParseRun.RdfCommon
{
    public class NoGraphExeption : Exception
    {
        public NoGraphExeption(IUriNode name)
        {
            
        }
    }
}