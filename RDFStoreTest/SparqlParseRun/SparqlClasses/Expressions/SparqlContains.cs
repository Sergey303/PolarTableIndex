using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlContains : SparqlExpression
    {
        public SparqlContains(SparqlExpression str, SparqlExpression pattern)
        {
            IsAggragate = pattern.IsAggragate || str.IsAggragate;
            IsDistinct = pattern.IsDistinct || str.IsDistinct;
            Func = result =>
            {
                var s = str.Func(result);
                var ps = pattern.Func(result);
                var strLit = (s as IStringLiteralNode);
                if (strLit != null && strLit.ComparebleWith(s))
                    return s.Content.Contains(ps.Content);
                throw new ArgumentException();
            };
        }
    }
}
