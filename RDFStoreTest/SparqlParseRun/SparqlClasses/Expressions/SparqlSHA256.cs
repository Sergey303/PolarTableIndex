using System;
using System.Security.Cryptography;
using System.Text;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlSHA256 : SparqlExpression
    {
        private SparqlExpression sparqlExpression;
        readonly SHA256 hash=new SHA256CryptoServiceProvider();
        public SparqlSHA256(SparqlExpression value)
        {

            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
            Func = result =>
            {
                var f = value.Func(result);
                if (f is SimpleLiteralNode)
                {
                    Func<byte, string> byteToStr = b => b.ToString("x2");
                    f.Content = string.Join("", hash.ComputeHash(Encoding.UTF8.GetBytes(f.Content)).Select(byteToStr));
                    return f;
                }
                throw new ArgumentException();
            };
        }
    }
}
