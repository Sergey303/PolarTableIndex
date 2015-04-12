using System;
using System.Linq;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.SparqlAggregateExpression
{
    class SparqlAvgExpression : SparqlAggregateExpression
    {
        public SparqlAvgExpression():base()
        {
            Func = result =>
            {
                if (result is SpraqlGroupOfResults)
                    return (result as SpraqlGroupOfResults).Group.Average(sparqlResult => Expression.Func(sparqlResult));
                else throw new Exception();
            };
        }
    }
}
