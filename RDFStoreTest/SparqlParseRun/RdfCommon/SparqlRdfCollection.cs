using System;
using System.Collections.Generic;
using System.Linq;

namespace SparqlParseRun.RdfCommon
{
    public class SparqlRdfCollection    
    {
        public List<INode> nodes = new List<INode>();

        public IBlankNode GetNode(Action<Triple> addTriple, Func<UriPrefixed, IUriNode> createUri, Func<IBlankNode> createBlank)
        {            
            var rdfFirst = createUri(SpecialTypes.RdfFirst);
            var rdfRest = createUri(SpecialTypes.RdfRest);
                IBlankNode sparqlBlankNodeFirst = createBlank();
                IBlankNode sparqlBlankNodeNext = createBlank();
            foreach (var node in nodes.Take(nodes.Count - 1))
            {
                addTriple(new Triple(sparqlBlankNodeNext, rdfFirst, node));
                addTriple(new Triple(sparqlBlankNodeNext, rdfRest, sparqlBlankNodeNext = createBlank()));
            }
            addTriple(new Triple(sparqlBlankNodeNext, rdfFirst, nodes[nodes.Count - 1]));
            addTriple(new Triple(sparqlBlankNodeNext, rdfRest, createUri(SpecialTypes.Nil)));
            return sparqlBlankNodeFirst;
        }
    }
}