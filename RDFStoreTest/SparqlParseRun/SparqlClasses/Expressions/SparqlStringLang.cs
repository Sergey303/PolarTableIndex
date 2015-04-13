using System;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlStringLang  :SparqlExpression
    {
        public SparqlStringLang(SparqlExpression literalExpression, SparqlExpression langExpression, INodeGenerator q)
        {
            IsAggragate = langExpression.IsAggragate || literalExpression.IsAggragate;
            IsDistinct = langExpression.IsDistinct || literalExpression.IsDistinct;
            Func = result =>
            {
                var literal = literalExpression.Func(result);
                var lang = langExpression.Func(result);
                if (literal is SimpleLiteralNode && lang is SimpleLiteralNode)
                {
                    return new SparqlLanguageLiteralNode(literal.Content, lang.Content, q.CreateUriNode(SpecialTypes.LangString));
                }
                throw new ArgumentException();
            };
        }
    }
}
