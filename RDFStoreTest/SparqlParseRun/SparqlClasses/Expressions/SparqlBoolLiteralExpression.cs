using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
   public class SparqlBoolLiteralExpression : SparqlExpression  
    {
      //  private SparqlBoolLiteralNode sparqlLiteralNode;

        public SparqlBoolLiteralExpression(ILiteralNode sparqlLiteralNode)
        {
            Func = result => sparqlLiteralNode.Content;
        }
        public override void Smaller(SparqlExpression value)
        {
            var funkClone = FunkClone;
            Func = result => !funkClone(result) && value.Func(result);
        }

        internal override void Greather(SparqlExpression value)
        {
            var funkClone = FunkClone;
            Func = result => funkClone(result) && !value.Func(result);
        }

        internal override void SmallerOrEquals(SparqlExpression value)
        {
            var funkClone = FunkClone;
            Func = result => !funkClone(result) || value.Func(result);
        }

        internal override void GreatherOrEquals(SparqlExpression value)
        {
            var funkClone = FunkClone;
            Func = result => funkClone(result) || !value.Func(result);
        }
    }

}
