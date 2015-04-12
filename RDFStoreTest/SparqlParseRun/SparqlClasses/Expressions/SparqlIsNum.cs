using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlIsNum : SparqlExpression
    {
        public SparqlIsNum(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
          Func = result =>
            {
                var f = value.Func(result);
                return f is double ||
                       f is long ||
                       f is int ||
                       f is short ||
                       f is byte ||
                       f is ulong ||
                       f is uint ||
                       f is ushort ||
                       (f is TypedLiteralNode);
            };
        }
    }
}
// xsd:nonPositiveInteger
//xsd:negativeInteger
//xsd:long
//xsd:int
//xsd:short
//xsd:byte
//xsd:nonNegativeInteger
//xsd:unsignedLong
//xsd:unsignedInt
//xsd:unsignedShort
//xsd:unsignedByte
//xsd:positiveInteger;