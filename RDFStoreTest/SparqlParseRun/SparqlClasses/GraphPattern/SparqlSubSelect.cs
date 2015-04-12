using System.Collections.Generic;
using SparqlParseRun.SparqlClasses.Query.Result;
using SparqlParseRun.SparqlClasses.SolutionModifier;

namespace SparqlParseRun.SparqlClasses.GraphPattern
{
    public class SparqlSubSelect : SparqlGraphPattern
    {
      

        private readonly SparqlSolutionModifier sparqlSolutionModifier;
        private readonly ISparqlGraphPattern sparqlValueDataBlock;


        public SparqlSubSelect(SparqlGraphPattern sparqlWhere, SparqlSolutionModifier sparqlSolutionModifier, ISparqlGraphPattern sparqlValueDataBlock)
        {
            // TODO: Complete member initialization
            AddRange(sparqlWhere);
            this.sparqlSolutionModifier = sparqlSolutionModifier;
            this.sparqlValueDataBlock = sparqlValueDataBlock;
            //   this.sparqlValueDataBlock = sparqlValueDataBlock;
        
        }

        public override IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings)
        {
                if(sparqlValueDataBlock!=null)
                 variableBindings = sparqlValueDataBlock.Run(variableBindings);
            return sparqlSolutionModifier.Run(base.Run(variableBindings));
        }

        public override SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.SubSelect;}}
    }
}
