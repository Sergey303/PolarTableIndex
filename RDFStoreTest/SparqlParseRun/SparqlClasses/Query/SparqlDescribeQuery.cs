using System;
using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples.Node;
using SparqlParseRun.SparqlClasses.Query.Result;
using SparqlParseRun.SparqlClasses.SolutionModifier;

namespace SparqlParseRun.SparqlClasses.Query
{
    public class SparqlDescribeQuery : SparqlQuery
    {
        private readonly List<IUriNode> nodeList = new List<IUriNode>();
        private bool isAll;


        public SparqlDescribeQuery(RdfQuery11Translator q) : base(q)
        {
            
        }

        internal void Add(IUriNode sparqlNode)
        {
            nodeList.Add(sparqlNode);
        }

        internal void IsAll()
        {
            isAll = true;
        }

        internal void Create(SparqlGraphPattern sparqlWhere)
        {
            this.sparqlWhere = sparqlWhere;
        }

        internal void Create(SparqlSolutionModifier sparqlSolutionModifier)
        {
            this.sparqlSolutionModifier = sparqlSolutionModifier;
        }

        public override SparqlResultSet Run(IStore store)
         {
            base.Run(store);
            var rdfInMemoryGraph = new RamListOfTriplesGraph("describes "+Guid.NewGuid());
            if (isAll)
                foreach (IUriNode node in ResultSet.Results.SelectMany(result => result.Cast<IUriNode>()))
                {
                    rdfInMemoryGraph.AddRange(store.GetTriplesWithSubject(node));
                    rdfInMemoryGraph.AddRange(store.GetTriplesWithObject(node));
                }
            else
            {
                foreach (IUriNode node in nodeList.Where(node => node is VariableNode)
                    .SelectMany(uriNode => ResultSet.Results.SelectMany(result => result.Cast<IUriNode>())))
                {
                    rdfInMemoryGraph.AddRange(store.GetTriplesWithSubject(node));
                    rdfInMemoryGraph.AddRange(store.GetTriplesWithObject(node));
                }
                            foreach (IUriNode uriNode in nodeList.Where(node => !(node is VariableNode)))

                    {
                        rdfInMemoryGraph.AddRange(store.GetTriplesWithSubject(uriNode));
                        rdfInMemoryGraph.AddRange(store.GetTriplesWithObject(uriNode));
                    }
            }
            ResultSet.ResultType = ResultType.Describe;
            ResultSet.GraphResult = rdfInMemoryGraph;
            return ResultSet;
        }

        public override SparqlQueryTypeEnum QueryType
        {
            get { return SparqlQueryTypeEnum.Describe; }
        }
    }
}
