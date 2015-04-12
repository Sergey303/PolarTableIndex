using System.Collections.Generic;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Path
{
    public class SparqlPathMaybeOne : SparqlPathTranslator
    {
        private readonly SparqlPathTranslator path;

        public SparqlPathMaybeOne(SparqlPathTranslator path) : base(path.predicate)
        {
            this.path = path;
        }

        public override IEnumerable<ISparqlGraphPattern> CreateTriple(INode subject, INode @object, RdfQuery11Translator q)
        {
            var subjectNode = IsInverse ? @object : subject;
            var objectNode = IsInverse ? subject : @object;
            //if (subjectNode is ISubjectNode && objectNode is ISubjectNode)
            {
                yield return new SparqlMayBeOneTriple(path.CreateTriple((ISubjectNode)subjectNode, objectNode, q), (ISubjectNode) subjectNode, (ISubjectNode) objectNode,q);
            }
            //else                                                                                             
            //foreach (var t in path.CreateTriple((ISubjectNode) subjectNode, objectNode, q))
            //    yield return t;
        }
    }
}