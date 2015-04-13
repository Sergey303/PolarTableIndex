using System;
using System.Linq;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.SparqlAggregateExpression
{
    class SparqlGroupConcatExpression : SparqlAggregateExpression
    {
        public SparqlGroupConcatExpression() :base()
        {
            Func = result =>
            {
                var spraqlGroupOfResults = (result as SpraqlGroupOfResults);
                if (spraqlGroupOfResults != null)
                {
                    try
                    {
                        return string.Join(Separator, spraqlGroupOfResults.Group.Select(Func));
                    }
                    catch
                    {
                    }
                    return null;
                }
                else throw new Exception();
            };
        }
    }
}
