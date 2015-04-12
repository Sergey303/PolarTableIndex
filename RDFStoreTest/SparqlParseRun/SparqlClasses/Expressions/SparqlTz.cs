using System;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlTz : SparqlExpression
    {
        public SparqlTz(SparqlExpression value, INodeGenerator q)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is DateTimeOffset)
                {
                    return new SimpleLiteralNode(((DateTimeOffset)f).Offset.ToString(), q.CreateUriNode(SpecialTypes.SimpleLiteral));
                }
                throw new ArgumentException();
            };
        }
    }
}
