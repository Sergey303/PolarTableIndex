using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Path;

namespace SparqlParseRun.SparqlClasses.GraphPattern
{
    public class SparqlGraphPattern : SparqlQuardsPattern
    {

        internal void CreateTriple(INode subj, IUriNode predicate, INode obj, RdfQuery11Translator q)
        {
            if (predicate is SparqlPathTranslator)
                AddRange(((SparqlPathTranslator) predicate).CreateTriple(subj, obj, q));
            else Add(new SparqlTriple(subj, predicate, obj, q));
        }
    }
}

