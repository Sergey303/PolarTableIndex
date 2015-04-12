using System.Collections.Generic;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.GraphPattern.Triples.Path
{
    public class SparqlPathTranslator : IUriNode
    {
        internal IUriNode predicate;

        public SparqlPathTranslator(IUriNode predicate)
        {
            // TODO: Complete member initialization
            this.predicate = predicate;
        }

    

        internal bool IsInverse { get; set; }

        public SparqlPathTranslator Inverse()
        {
            IsInverse = !IsInverse;
            return this;
        }

        public virtual IEnumerable<ISparqlGraphPattern> CreateTriple(INode subject, INode @object, RdfQuery11Translator q)
       {
           var subjectNode = IsInverse ? @object : subject;
           var objectNode = IsInverse ? subject : @object;
           yield return new SparqlTriple((ISubjectNode) subjectNode, predicate, objectNode, q);
       }

     
        public string UriString { get { return predicate.UriString; }}
        public UriPrefixed UriPrefixed { get { return predicate.UriPrefixed; }}

        public string ToStringWithBraces()
        {
            return predicate.ToStringWithBraces();
        }

        internal virtual SparqlPathTranslator AddAlt(SparqlPathTranslator sparqlPathTranslator)
        {
            return new SparqlPathAlternative(this,sparqlPathTranslator);
        }

        internal virtual SparqlPathTranslator AddSeq(SparqlPathTranslator sparqlPathTranslator)
        {
            return new SparqlPathSequence(this, sparqlPathTranslator);
        }

        public NodeType Type { get { return NodeType.Uri; } }
        public int CompareTo(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}