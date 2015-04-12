using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlUcase :SparqlExpression
    {
        public SparqlUcase(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is SimpleLiteralNode)
                {
                    f.Content = f.Content.ToUpper();
                    return f;
                }
                throw new ArgumentException();
            };
        }
    }
}
