using System.Collections.Generic;

namespace SparqlParseRun.RdfCommon
{
    public class DataSet : List<IUriNode>
    {
        public DataSet(IEnumerable<IUriNode> gs)
            :base(gs)
        {
            

        }

        public DataSet()                 
        {
            
        }
    }
}