using System;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlLang  : SparqlExpression
    {
        public SparqlLang(SparqlExpression value, INodeGenerator q)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is SparqlLanguageLiteralNode)
                    return  q.CreateLiteralNode(f.Lang.Substring(1));
                throw new ArgumentException();
            };
        }
    }
}
