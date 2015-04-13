using System;
using System.Web;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlEncodeForUri : SparqlExpression
    {
        public SparqlEncodeForUri(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is SimpleLiteralNode)
                {
                    f.Content = HttpUtility.UrlEncode(f.Content);
                    return f;
                }
                throw new ArgumentException();
            };
        }
    }
}
