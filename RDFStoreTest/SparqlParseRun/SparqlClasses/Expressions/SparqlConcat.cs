using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    class SparqlConcat : SparqlExpression
    {
        public SparqlConcat(List<SparqlExpression> list, INodeGenerator q)
        {
             IsAggragate = list.Any(value=> value.IsAggragate);
            IsDistinct = list.Any(value=>  value.IsDistinct);
            Func = result =>
            {
                var values = list.Select(expression => expression.Func(result)).ToArray();
                if (values.Length == 0) return new SimpleLiteralNode(string.Empty, q.CreateUriNode(SpecialTypes.SimpleLiteral));
                if (values.All(o => o is SparqlLanguageLiteralNode))
                {
                    var commonLang = values[0].Lang;
                    if (values.All(ls => ls.Lang .Equals(commonLang)))
                        return new SparqlLanguageLiteralNode(string.Concat(values.Select(o => o.Content)), commonLang, values[0].UriType);
                }
                if (values.All(o => o is LiteralofTypeStringNode))
                    return new LiteralofTypeStringNode(string.Concat(values.Select(o => o.Content)), values[0].UriType);
                return new SimpleLiteralNode(string.Concat(values.Select(s => s.Content)), q.CreateUriNode(SpecialTypes.SimpleLiteral));
            };

        }
    }
}
