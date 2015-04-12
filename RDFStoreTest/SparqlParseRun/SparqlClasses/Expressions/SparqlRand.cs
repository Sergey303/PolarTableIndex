using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlRand : SparqlExpression
    {
        public SparqlRand()
        {
            Random r=new Random();
            Func = result => r.NextDouble();
        }
    }
}