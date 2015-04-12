using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlRegexExpression : SparqlExpression
    {
        private SparqlExpression variableExpression;
        private SparqlExpression patternExpression;

        internal void SetVariableExpression(SparqlExpression sparqlExpression)
        {
            variableExpression = sparqlExpression;
        }

        internal void SetRegex(SparqlExpression patternExpression)
        {
            this.patternExpression = patternExpression;
            Func = result =>
            {
                var pattern = patternExpression.Func(result).Content;
                //regex.Trim('"');
                Regex regex;
                if (!Regexes.TryGetValue(pattern, out regex))
                    Regexes.Add(pattern, regex = new Regex(pattern));
                //if (parameter.Type == typeof (object))
                //  parameter = Expression.Call(Expression.Convert(parameter, typeof (ILiteralNode)), "GetString", new Type[0]);
                return regex.IsMatch(variableExpression.Func(result).Content);
            };

            //typeof(Regex).GetMethod("IsMatch", new []{typeof(string)}), parameter);
        }

        private static readonly Dictionary<string, Regex> Regexes = new Dictionary<string, Regex>();
        private static readonly Dictionary<KeyValuePair<string, string>, Regex> RegexesParameters = new Dictionary<KeyValuePair<string, string>, Regex>();

        internal void SetParameters(SparqlExpression paramsExpression)
        {
            Func = result =>
            {
                var pattern = patternExpression.Func(result).Content;
                var parameters = paramsExpression.Func(result).Content;
                //regex.Trim('"');
                Regex regex;
                var keyValuePair = new KeyValuePair<string, string>(pattern, parameters);
                if (!RegexesParameters.TryGetValue(keyValuePair, out regex))
                {
                    RegexOptions op=RegexOptions.None;
                    if (parameters.Contains("s"))
                     op|=RegexOptions.Singleline;
                    if (parameters.Contains("m"))
                     op|=RegexOptions.Multiline;
                    if (parameters.Contains("i"))
                     op|=RegexOptions.IgnoreCase;
                    if (parameters.Contains("x"))
                     op|=RegexOptions.IgnorePatternWhitespace;
                    
                    RegexesParameters.Add(keyValuePair, regex = new Regex(pattern,op));
                }
                //if (parameter.Type == typeof (object))
                //  parameter = Expression.Call(Expression.Convert(parameter, typeof (ILiteralNode)), "GetString", new Type[0]);
                return regex.IsMatch(variableExpression.Func(result).Content);
            };

        }

    }
}
