using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.Query.Result;
using SparqlParseRun.SparqlClasses.SolutionModifier;

namespace SparqlParseRun.SparqlClasses.Query
{
    public class SparqlSelectQuery : SparqlQuery
    {
        private readonly RdfQuery11Translator q;

        public SparqlSelectQuery(RdfQuery11Translator q) : base(q)
        {
          
        }

      
      
       

        internal void Create( SparqlGraphPattern sparqlWhere, SparqlSolutionModifier sparqlSolutionModifier)
        {
            this.sparqlWhere = sparqlWhere;
            this.sparqlSolutionModifier = sparqlSolutionModifier;
            //  this.sparqlSolutionModifier.IsDistinct=sparqlSelect.IsDistinct;
            //if (this.sparqlSolutionModifier.IsDistinct)
            //    sparqlSelect.IsDistinct = false;
        }

        public override SparqlResultSet Run(IStore store)
        {
            Q.Store = store;
            ResultSet.Variables = Q.Variables;
            ResultSet.Results = sparqlWhere.Run(ResultSet.Results);
       
            if (sparqlSolutionModifier != null )
                ResultSet.Results = sparqlSolutionModifier.Run(ResultSet.Results, ResultSet);

            ResultSet.ResultType = ResultType.Select;
            return ResultSet;
        }

        public override SparqlQueryTypeEnum QueryType
        {
            get { return SparqlQueryTypeEnum.Select; }
        }
    }
}
