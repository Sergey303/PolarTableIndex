using System;
using System.Linq;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.SparqlAggregateExpression
{
    class SparqlMaxExpression : SparqlAggregateExpression
    {
        public SparqlMaxExpression():base()
        {
            Func = result =>
            {
                if (result is SpraqlGroupOfResults)
                {
                    return (result as SpraqlGroupOfResults).Group.Max(sparqlResult => Expression.Func(sparqlResult));
                }
                else throw new Exception();
            };
        }
    }
}
