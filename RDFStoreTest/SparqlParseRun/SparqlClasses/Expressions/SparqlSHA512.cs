using System;
using System.Security.Cryptography;
using System.Text;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
  public  class SparqlSHA512 : SparqlExpression
    {
      readonly SHA512 hash=new SHA512CryptoServiceProvider();
      public SparqlSHA512(SparqlExpression value)
      {
          IsAggragate = value.IsAggragate;
          IsDistinct = value.IsDistinct;
           Func = result =>
            {
                var f = value.Func(result);
                if (f is SimpleLiteralNode)
                {
                    Func<byte,string> byteToStr = b => b.ToString("x2");
                    f.Content = string.Join("", hash.ComputeHash(Encoding.UTF8.GetBytes(f.Content)).Cast<byte>().Select(byteToStr));
                    return f;
                }
                throw new ArgumentException();
            };
      }
    }
}
