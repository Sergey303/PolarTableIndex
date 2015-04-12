using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.Query.Result;
using SparqlParseRun.SparqlClasses.SolutionModifier;

namespace SparqlParseRun.SparqlClasses.Query
{
    public class SparqlAsqQuery : SparqlQuery
    {

        public SparqlAsqQuery(RdfQuery11Translator q) : base(q)
        {
         
        }
        internal void Create(SparqlGraphPattern sparqlWhere, SparqlSolutionModifier sparqlSolutionModifier)
        {
            this.sparqlWhere = sparqlWhere;
            this.sparqlSolutionModifier = sparqlSolutionModifier;
        }

        public override SparqlResultSet Run(IStore store)
        {
            base.Run(store);
            ResultSet.ResultType = ResultType.Ask;
            return ResultSet;

        }

        public override SparqlQueryTypeEnum QueryType
        {
            get { return SparqlQueryTypeEnum.Ask; }
        }
    }
}
