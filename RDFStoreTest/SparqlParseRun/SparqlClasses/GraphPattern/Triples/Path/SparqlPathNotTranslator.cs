using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.Expressions;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Path
{
    public class SparqlPathNotTranslator : SparqlPathTranslator
    {
        public readonly List<SparqlPathTranslator> alt=new List<SparqlPathTranslator>();

        public SparqlPathNotTranslator(SparqlPathTranslator path)
            : base(path.predicate)
        {
            // TODO: Complete member initialization
            alt.Add(path); 
        }
        public override IEnumerable<ISparqlGraphPattern> CreateTriple(INode subject, INode @object, RdfQuery11Translator q)
        {
                var subjectNode = IsInverse ? @object : subject;
           var objectNode = IsInverse ? subject : @object;
            var directed = alt.Where(translator => !translator.IsInverse).ToList();
            var anyDirected = directed.Any();
            if (anyDirected)
            {
                var variableNode = q.CreateBlankNode();
                yield return new SparqlTriple((ISubjectNode) subjectNode, variableNode, objectNode, q);
                foreach (var pathTranslator in directed)
                {
                    var expr = new SparqlVarExpression(variableNode);
                    expr.NotEquals(new SparqlIriExpression(pathTranslator.predicate));
                    yield return new SparqlFilter(expr);
                }
            }

            //TODO drop subject object variables
            var inversed = alt.Where(translator => translator.IsInverse).ToList();
            var anyInversed = inversed.Any();
            if (anyInversed)
            {
                var variableNode =q.CreateBlankNode();
                yield return new SparqlTriple((ISubjectNode) objectNode, variableNode, subjectNode, q);
                foreach (var pathTranslator in inversed)
                {
                    var expr = new SparqlVarExpression(variableNode);
                    expr.NotEquals(new SparqlIriExpression(pathTranslator.predicate));
                    yield return new SparqlFilter(expr);
                }
            }
        }
    }
}
