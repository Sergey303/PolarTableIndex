using SparqlParseRun.SparqlClasses.Expressions;

namespace SparqlParseRun.SparqlClasses.SparqlAggregateExpression
{
    public abstract class SparqlAggregateExpression  : SparqlExpression
    {
        public bool isAll;
        public SparqlAggregateExpression()
        {
            IsAggragate = true;
            if(Expression!=null)
            IsDistinct = Expression.IsDistinct;

        }

        internal void IsAll()
        {
            isAll = true;
        }

        public SparqlExpression Expression { get; set; }

        public string Separator { get; set; }
    }
}
