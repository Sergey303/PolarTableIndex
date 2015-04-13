using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlTimeZone : SparqlExpression
    {
        private SparqlExpression sparqlExpression;

        public SparqlTimeZone(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is DateTime)
                    return TimeZoneInfo.Utc.GetUtcOffset((DateTime) f);
                throw new ArgumentException();
            };
        }
    }
}
