using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlLangMatches : SparqlExpression
    {
        public SparqlLangMatches(SparqlExpression value, SparqlExpression sparqlExpression)
        {
            IsDistinct = value.IsDistinct || sparqlExpression.IsDistinct;
            IsAggragate = value.IsAggragate || sparqlExpression.IsAggragate;
            Func = result =>
            {
                var lang = value.Func(result);
                var langString = lang as IStringLiteralNode;
                  var langRange = sparqlExpression.Func(result);
                var langRangeString = langRange as IStringLiteralNode;
                if (langString != null && langRangeString!=null)
                {
                    return Equals(langRangeString.Content, "*")
                        ? !string.IsNullOrWhiteSpace(langString.Content)
                        : Equals(langString.Content, langRangeString.Content);
                }
                throw new ArgumentException();
            };
        }
    }
}