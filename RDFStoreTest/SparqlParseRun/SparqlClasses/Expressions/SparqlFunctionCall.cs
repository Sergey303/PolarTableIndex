using System;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlFunctionCall  :SparqlExpression
    {
   //     private SparqlUriNode sparqlUriNode;
       public SparqlArgs sparqlArgs;

        public SparqlFunctionCall(IUriNode sparqlUriNode, SparqlArgs sparqlArgs)
        {
            // TODO: Complete member initialization
           // this.sparqlUriNode = sparqlUriNode;
            this.sparqlArgs = sparqlArgs;
            IsDistinct = sparqlArgs.isDistinct;   //todo
            if (Equals(sparqlUriNode.UriString, SpecialTypes.Bool.FullName))
            {
              Func= result =>
                {
                    dynamic o = (this.sparqlArgs[0].Func(result));
                    if (o is IStringLiteralNode)
                        return bool.Parse(o.Content);
                    if (o is double || o is int || o is float || o is decimal)
                        return o!=0;
                    if (o is bool)
                        return o;
                    throw new ArgumentException();
                };
                   return;
            }  else
                if (Equals(sparqlUriNode.UriString, SpecialTypes.Double.FullName))
                {
                    Func = result =>
                    {
                        dynamic o = (this.sparqlArgs[0].Func(result));
                        if (o is IStringLiteralNode)
                            return (double.Parse(o.Content.Replace(".", ",")));
                        if (o is double || o is int || o is float || o is decimal)
                            return Convert.ToDouble(o);
                        throw new ArgumentException();
                    };
                    return;
                }
                else
                    if (Equals(sparqlUriNode.UriString, SpecialTypes.Float.FullName))
                    {
                        Func = result =>
                        {
                            dynamic o = (this.sparqlArgs[0].Func(result));
                            if (o is IStringLiteralNode)
                                return (float.Parse(o.Content.Replace(".", ",")));
                            if (o is double || o is int || o is float || o is decimal)
                                return (float)Convert.ToDouble(o);
                            throw new ArgumentException();
                        };
                        return;
                    }
                    else
                        if (Equals(sparqlUriNode.UriString, SpecialTypes.Decimal.FullName))
                        {
                            Func = result =>
                            {
                                dynamic o = (this.sparqlArgs[0].Func(result));
                                if (o is IStringLiteralNode)
                                    return (decimal.Parse(o.Content.Replace(".", ",")));
                                if (o is double || o is int || o is float || o is decimal)
                                    return Convert.ToDecimal(o);
                                throw new ArgumentException();
                            };
                            return;
                        }
                        else
                            if (Equals(sparqlUriNode.UriString, SpecialTypes.Integer.FullName))
                            {
                                Func = result =>
                                {
                                    dynamic o = (this.sparqlArgs[0].Func(result));
                                    if (o is IStringLiteralNode)
                                        return (int.Parse(o.Content));
                                    if (o is double || o is int || o is float || o is decimal)
                                        return Convert.ToInt32(o);
                                    throw new ArgumentException();
                                };
                                return;
                            }
                            else
                                if (Equals(sparqlUriNode.UriString, SpecialTypes.DateTime.FullName))
                                {
                                    Func = result =>
                                    {
                                        dynamic o = (this.sparqlArgs[0].Func(result));
                                        if (o is IStringLiteralNode)
                                            return (DateTime.Parse(o.Content));
                                        if (o is DateTime)
                                            return o;
                                        throw new ArgumentException();
                                    };
                                    return;
                                }
                                else if (Equals(sparqlUriNode.UriString, SpecialTypes.String.FullName))
                                {
                                    Func = result =>  this.sparqlArgs[0].Func(result).ToString();
                                    return;
                                }
                                else
            throw new NotImplementedException("mathod call " + sparqlUriNode);
        }

      //  internal readonly Func<SparqlResult, dynamic> Func;
    }
}
