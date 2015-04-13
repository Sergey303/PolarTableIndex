using System.Collections.Generic;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Path
{
    public class SparqlPathOneOrMany : SparqlPathTranslator
    {
        private readonly SparqlPathTranslator path;

        public SparqlPathOneOrMany(SparqlPathTranslator path)
            : base(path.predicate)
        {
            this.path = path;
        }

        public override IEnumerable<ISparqlGraphPattern> CreateTriple(INode subject, INode @object, RdfQuery11Translator q)
        {
            var subjectNode = IsInverse ? @object : subject;
            var objectNode = IsInverse ? subject : @object;
            yield return new SparqlPathManyTriple(subjectNode, path, objectNode,q);
        }
    }
}