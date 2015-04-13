using System;
using System.Security.Cryptography;
using System.Text;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlMD5 : SparqlExpression
    {
        readonly MD5 md5 = new MD5CryptoServiceProvider();

        public SparqlMD5(SparqlExpression value)
        {
            IsAggragate = value.IsAggragate;
            IsDistinct = value.IsDistinct;
               Func = result =>
            {
                var f = value.Func(result);
                if (f is SimpleLiteralNode)
                {
                    Func<byte, string> byteToStr = b => b.ToString("x2");
                    f.Content = string.Join("", md5.ComputeHash(Encoding.UTF8.GetBytes(f.Content)).Select(byteToStr));
                    return f;
                }
                throw new ArgumentException();
            };
        }
    }
}
