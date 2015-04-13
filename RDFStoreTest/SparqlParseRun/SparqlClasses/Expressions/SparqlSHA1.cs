using System;
using System.Security.Cryptography;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
   
    class SparqlSHA1 : SparqlExpression
    {
        private readonly SHA1 hash;
        public SparqlSHA1(SparqlExpression value)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is SimpleLiteralNode)
                {
                    f.Content = Convert.FromBase64String(hash.ComputeHash(f.Content));
                    return f;
                }
                throw new ArgumentException();
            };
        }
    }
}
