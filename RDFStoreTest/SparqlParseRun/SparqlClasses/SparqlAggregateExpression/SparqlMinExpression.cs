using System;
using System.Linq;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.SparqlAggregateExpression
{
    class SparqlMinExpression : SparqlAggregateExpression
    {
        public SparqlMinExpression()
            : base()
        {
            Func = result =>
            {
                if (result is SpraqlGroupOfResults)
                {
                    return (result as SpraqlGroupOfResults).Group.Min(sparqlResult => Expression.Func(sparqlResult));
                }
                else throw new Exception();
            };
        }
    }
}
