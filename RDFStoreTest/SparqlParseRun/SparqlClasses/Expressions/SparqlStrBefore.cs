using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlStrBefore : SparqlExpression
    {
        public SparqlStrBefore(SparqlExpression str, SparqlExpression pattern)
        {
            IsAggragate = pattern.IsAggragate || str.IsAggragate;
            IsDistinct = pattern.IsDistinct || str.IsDistinct;
            Func = result =>
            {
                var strValue = str.Func(result);
                var patternValue = pattern.Func(result);
                 var strLit = (strValue as IStringLiteralNode);
                if (strLit!=null && strLit.ComparebleWith(strValue))
                {
                    strValue.Content = StringBefore(strValue.Content, patternValue.Content);
                    return strValue;
                }
                throw new ArgumentException();
            };
        }
          string StringBefore(string str, string pattern)
        {
            if (pattern == string.Empty) return string.Empty;
           int index = str.LastIndexOf(pattern, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1 || (index += pattern.Length )>= str.Length) return string.Empty;
            return str.Substring(index);
        }
    }
}
