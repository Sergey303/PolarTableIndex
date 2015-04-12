using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlStrStarts : SparqlExpression
    {
       

        public SparqlStrStarts(SparqlExpression str, SparqlExpression pattern)
        {
            IsAggragate = pattern.IsAggragate || str.IsAggragate;
            IsDistinct = pattern.IsDistinct || str.IsDistinct;
            Func = result =>
            {
                var strValue = str.Func(result);
                var patternValue = pattern.Func(result);
                var strLit = (strValue as IStringLiteralNode);
                if (strLit!=null && strLit.ComparebleWith(strValue))
                    return strValue.Content.StartsWith(patternValue.Content);
                throw new ArgumentException();
            };
        }
    }
}
