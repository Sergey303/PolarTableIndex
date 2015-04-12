using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.SparqlClasses.GraphPattern;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public class SparqlFilter : ISparqlGraphPattern
    {
   
      //  public Func<IEnumerable<Action>> SelectVariableValuesOrFilter { get; set; }
      //   public SparqlResultSet resultSet;
   //     private SparqlConstraint sparqlConstraint;
        private readonly SparqlExpression sparqlExpression;

        public SparqlFilter(SparqlExpression sparqlExpression)
        {
            // TODO: Complete member initialization
            this.sparqlExpression = sparqlExpression;
        }

        public IEnumerable<SparqlResult> Run(IEnumerable<SparqlResult> variableBindings)
        {
            
            return variableBindings.Where(variableBinding => sparqlExpression.Test(variableBinding));
        }

        public SparqlGraphPatternType PatternType { get{return SparqlGraphPatternType.Filter;} }
    }
}