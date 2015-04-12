using System;
using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.RdfCommon;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.GraphPattern.Triples;
using SparqlParseRun.SparqlClasses.Query.Result;
using SparqlParseRun.SparqlClasses.SolutionModifier;

namespace SparqlParseRun.SparqlClasses.Query
{
    public class SparqlConstructQuery :SparqlQuery
    {
        readonly List<IDataset> datasets = new List<IDataset>();
        private SparqlGraphPattern constract;

        public SparqlConstructQuery(RdfQuery11Translator q) : base(q)
        {
           
        } 
        internal void Create(SparqlSolutionModifier sparqlSolutionModifier)
        {
            this.sparqlSolutionModifier = sparqlSolutionModifier;
        }

        internal void Create(SparqlGraphPattern sparqlTriples)
        {         
            sparqlWhere = sparqlTriples;
        }

        internal void Add(IDataset sparqlDataset)
        {
            datasets.Add(sparqlDataset);
        }



        internal void Create(SparqlGraphPattern sparqlTriples, SparqlGraphPattern sparqlWhere, SparqlSolutionModifier sparqlSolutionModifier)
        {
            constract = sparqlTriples;
            this.sparqlWhere = sparqlWhere;
            this.sparqlSolutionModifier = sparqlSolutionModifier;
        }

        public override SparqlResultSet Run(IStore store)
        {
           base.Run(store);                                 
            ResultSet.GraphResult = new RamListOfTriplesGraph("constructed "+Guid.NewGuid());
            foreach (var triple in
                    ResultSet.Results.SelectMany(
                        result => constract.Cast<SparqlTriple>().Select(st => st.Substitution(result, ResultSet.GraphResult.Name))))
                ResultSet.GraphResult.Add(triple);

            ResultSet.ResultType = ResultType.Construct;  
            return  ResultSet;
        }

        public override SparqlQueryTypeEnum QueryType
        {
            get { return SparqlQueryTypeEnum.Construct; }
        }
    }
}
