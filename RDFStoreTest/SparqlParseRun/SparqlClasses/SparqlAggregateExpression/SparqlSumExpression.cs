using System;
using System.Linq;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.SparqlAggregateExpression
{
    class SparqlSumExpression : SparqlAggregateExpression
    {
        public SparqlSumExpression() :base()
        {
            
            Func = result =>
            {
                if (result is SpraqlGroupOfResults)
                {
                    return (result as SpraqlGroupOfResults).Group.Sum(sparqlResult => Expression.Func(sparqlResult));
                }
                else throw new Exception();
            };
        }
    }
}
