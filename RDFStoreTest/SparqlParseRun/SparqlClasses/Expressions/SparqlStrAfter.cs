using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlStrAfter : SparqlExpression
    {
     

        public SparqlStrAfter(SparqlExpression str, SparqlExpression pattern)
        {
      
            // TODO: Complete member initialization
            IsAggragate = pattern.IsAggragate || str.IsAggragate;
            IsDistinct = pattern.IsDistinct ||str.IsDistinct;
            Func = result =>
            {
                var strValue = str.Func(result);
                var patternValue = pattern.Func(result);
                var strLit = (strValue as IStringLiteralNode);
                if (strLit != null && strLit.ComparebleWith(strValue))
                {
                    strValue.Content = StringAfter(strValue.Content, patternValue.Content);
                    return strValue;
                }
                throw new ArgumentException();
            };
          
        }

        string StringAfter(string str, string pattern)
        {
            if (Equals(pattern, string.Empty)) return string.Empty;
           int index = str.LastIndexOf(pattern, StringComparison.InvariantCultureIgnoreCase);
            if (Equals(index, -1) || (index += pattern.Length )>= str.Length) return string.Empty;
            return str.Substring(index);
        }
    }
}
