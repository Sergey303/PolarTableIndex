using System;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlUuid : SparqlExpression
    {
        public SparqlUuid()
        {                          
          
            Func = result => new Uri("urn:uuid:" + Guid.NewGuid());
        }
    }
}
