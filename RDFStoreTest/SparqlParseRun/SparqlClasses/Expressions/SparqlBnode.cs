using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlBnode : SparqlExpression
    {
        public SparqlBnode(SparqlExpression value, RdfQuery11Translator q)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var str = value.Func(result);
                if (str is SimpleLiteralNode)
                    return q.CreateBlankNode(str);
                throw new ArgumentException();
            };
        }

        public SparqlBnode(RdfQuery11Translator q)
        {
            Func = result => q.CreateBlankNode();
        }
    }
}
