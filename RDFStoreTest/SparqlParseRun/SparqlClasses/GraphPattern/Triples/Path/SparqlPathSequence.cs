using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Path
{
    public class SparqlPathSequence : SparqlPathTranslator
    {
        public readonly List<SparqlPathTranslator> seq = new List<SparqlPathTranslator>();

        public SparqlPathSequence(SparqlPathTranslator sparqlPathTranslator, SparqlPathTranslator sparqlPathTranslator1) :base(null)
        {
            seq = new List<SparqlPathTranslator>() {sparqlPathTranslator, sparqlPathTranslator1};
        }

        internal override SparqlPathTranslator AddSeq(SparqlPathTranslator sparqlPathTranslator)
        {
            seq.Add(sparqlPathTranslator);
            return this;
        }

        public override IEnumerable<ISparqlGraphPattern> CreateTriple(INode subject, INode @object, RdfQuery11Translator q)
        {
            VariableNode t=q.CreateBlankNode();
            var subjectNode = IsInverse ? @object : subject;
            var objectNode = IsInverse ? subject : @object;
            return seq.First().CreateTriple((ISubjectNode) subjectNode, t, q)
                .Concat(seq.Skip(1).Take(seq.Count - 2).SelectMany(path => path.CreateTriple(t, t = q.CreateBlankNode(), q)))
                .Concat(seq.Last().CreateTriple(t, objectNode, q));
        }
    }
}