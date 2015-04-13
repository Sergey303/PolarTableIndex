using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlStrDataType : SparqlExpression
    {
        private SparqlExpression sparqlExpression1;
        private SparqlExpression sparqlExpression2;

        public SparqlStrDataType(SparqlExpression sparqlExpression1, SparqlExpression sparqlExpression2)
        {
            // TODO: Complete member initialization
            this.sparqlExpression1 = sparqlExpression1;
            this.sparqlExpression2 = sparqlExpression2;
            Func = result =>
            {
                var str = sparqlExpression1.Func(result);
                var type = sparqlExpression2.Func(result);
                if (str is string && type is Uri)
                {
                    return new TypedLiteralNode(str, type);
                }
                throw new ArgumentException();
            };
        }
    }
}
