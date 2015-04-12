using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.Query;
using SparqlParseRun.SparqlClasses.Query.Result;
using SparqlParseRun.SparqlClasses.SolutionModifier;


namespace SparqlParseRun
{
    public abstract class SparqlQuery
    {
        internal SparqlGraphPattern sparqlWhere;
        internal readonly SparqlResultSet ResultSet;
        protected RdfQuery11Translator Q;
        protected SparqlSolutionModifier sparqlSolutionModifier;

        public SparqlQuery(RdfQuery11Translator q)
        {
            Q = q;
            ResultSet=new SparqlResultSet(q.prolog);
        }


        public virtual SparqlResultSet Run(IStore store)
        {
            Q.Store = store;
            ResultSet.Variables = Q.Variables;
            ResultSet.Results = sparqlWhere.Run(ResultSet.Results);
            if (sparqlSolutionModifier != null)
                ResultSet.Results = sparqlSolutionModifier.Run(ResultSet.Results);
          
          
            return ResultSet;
        }
        public SparqlQueryTypeEnum Type { get; set; }
        public abstract SparqlQueryTypeEnum QueryType { get; }


        internal void SetValues(ISparqlGraphPattern valueDataBlock)
        {
            if(valueDataBlock!=null)
          ResultSet.Results=   valueDataBlock.Run(ResultSet.Results);
           
        }

      
    }
}
