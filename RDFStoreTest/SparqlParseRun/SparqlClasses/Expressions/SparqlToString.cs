using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlToString : SparqlExpression
    {
       // private SparqlExpression sparqlExpression;

        public SparqlToString(SparqlExpression value, INodeGenerator q)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;

            Func = result =>
            {
                var func = value.Func(result);
                if (func is SimpleLiteralNode) return func;
                return new SimpleLiteralNode(func.ToString(), q.CreateUriNode(SpecialTypes.SimpleLiteral));
            };
        }
    }
}
