using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlDataType : SparqlExpression
    {
        public SparqlDataType(SparqlExpression value)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
          
            Func = result =>
            {
                var r = value.Func(result);
                var literalNode = r as ILiteralNode;
                if (literalNode != null)
                    return literalNode.DataType;
                throw new ArgumentException();
            };
        }
    }
}