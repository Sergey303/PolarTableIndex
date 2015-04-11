using System.Collections.Generic;
using System.IO;
using System.Linq;
using PolarDB;
using RdfStoreSparqlNamespace;
using SparqlParseRun.RdfCommon;    
    public class RDFStoreStringsPrefixedQuads  :RDFStoreStringsQuads
    {
        private new StringPrefixedNodeGenerator nodeGenerator;
        public override INodeGenerator NodeGenerator { get { return nodeGenerator; } }

        public RDFStoreStringsPrefixedQuads()
        {
            nodeGenerator = new StringPrefixedNodeGenerator();
        }
    }
