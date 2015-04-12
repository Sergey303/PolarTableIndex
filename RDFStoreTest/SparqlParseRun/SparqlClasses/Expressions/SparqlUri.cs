using System;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlUri : SparqlExpression
    {
        public SparqlUri(SparqlExpression value, RdfQuery11Translator q)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is IUriNode)
                    return f;                                                   
                if (f is SimpleLiteralNode) //TODO 
                    return q.Store.NodeGenerator.CreateUriNode(q.prolog.GetFromString(f.Content));
                throw new ArgumentException();
            }; 
        }
    }
}
