using System;
using System.Linq;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.SparqlAggregateExpression
{
    class SparqlCountExpression : SparqlAggregateExpression
    {
        public SparqlCountExpression() :base()
        {
            if(isAll)
                Func = result =>
                {
                    if (result is SpraqlGroupOfResults)
                    {
                        return (result as SpraqlGroupOfResults).Group.Count();
                    }
                    else throw new Exception();
                };
            else
            Func = result =>
            {
                if (result is SpraqlGroupOfResults)
                {
                    return (result as SpraqlGroupOfResults).Group.Count(sparqlResult => Expression.Func(sparqlResult));
                }
                else throw new Exception();
            };
        }
    }
}
