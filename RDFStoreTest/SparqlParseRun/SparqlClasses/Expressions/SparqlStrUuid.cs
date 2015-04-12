using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlStrUuid : SparqlExpression
    {
        public SparqlStrUuid()
        {
          
            Func = result => Guid.NewGuid().ToString();
        }
    }
}
