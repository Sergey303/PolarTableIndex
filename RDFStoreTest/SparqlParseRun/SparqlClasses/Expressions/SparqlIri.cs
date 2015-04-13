using System;
using RdfCommon;
using RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlIri : SparqlExpression
    {
        public SparqlIri(SparqlExpression value, INodeGenerator q)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
         Func = result =>
            {
                var f = value.Func(result);
                if (f is IUriNode)
                    return f;
                if(f is ILiteralNode)      //TODO
                    return q.CreateUriNode(f.Content);
                throw new ArgumentException();  
            }; 
        }
    }
}
