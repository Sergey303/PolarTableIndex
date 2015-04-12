using System;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
   public class SparqlSubstringExpression  : SparqlExpression
   {
       private SparqlExpression strExpression, startExpression;
       internal void SetString(SparqlExpression value)
       {
           IsAggragate = value.IsAggragate;
           IsDistinct = value.IsDistinct;

           strExpression = value;
       }
       public void SetStartPosition(SparqlExpression value)
       {

           IsAggragate = value.IsAggragate;
           IsDistinct = value.IsDistinct;

           startExpression = value;
           Func = result =>
           {
               var start = value.Func(result);
               var str = strExpression.Func(result);
               if (start is int)
               {
                   if (str is SimpleLiteralNode)
                       return str.Substring(start);
                   if (str is LiteralofTypeStringNode)
                       return new LiteralofTypeStringNode(str.Content.Substring(start), (str as LiteralofTypeStringNode).DataType);
                   if (str is SparqlLanguageLiteralNode)
                       return new SparqlLanguageLiteralNode(str.Content.Substring(start), str.Lang, (str as SparqlLanguageLiteralNode).DataType);
               }
               throw new ArgumentException(); 
           };
       }

       internal void SetLength(SparqlExpression lengthExpression)
       {
           IsAggragate = lengthExpression.IsAggragate;
           IsDistinct = lengthExpression.IsDistinct;
          
           Func = result =>
           {
               var start = startExpression.Func(result);
               var length = lengthExpression.Func(result);
               var str = strExpression.Func(result);
               if (start is int && length is int)
               {
                   if (str is SimpleLiteralNode)
                       return str.Substring(start,length);
                   if (str is LiteralofTypeStringNode)
                       return new LiteralofTypeStringNode(str.Content.Substring(start, length), (str as LiteralofTypeStringNode).DataType);
                   if (str is SparqlLanguageLiteralNode)
                       return new SparqlLanguageLiteralNode(str.Content.Substring(start, length), str.Lang, (str as SparqlLanguageLiteralNode).DataType);
               }
               throw new ArgumentException();
           };
       }
    }
}
