using System;
using System.Collections.Generic;
using SparqlParseRun.SparqlClasses.Query;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.SolutionModifier
{
    public class SparqlSolutionModifier
    {
        public SparqlSolutionModifier()
        {
            //if (isDistinct)
            //Modifier = enumerable => enumerable.Distinct();
        }

        private Func<IEnumerable<SparqlResult>, IEnumerable<SparqlResult>> LimitOffset;
        private Func<IEnumerable<SparqlResult>, IEnumerable<SparqlResult>> Order;
        private Func<IEnumerable<SparqlResult>, IEnumerable<SparqlResult>> Having;
        private Func<IEnumerable<SparqlResult>, IEnumerable<SparqlResult>> Group;
        private SparqlSelect Select;

        internal void Add(SparqlSolutionModifierLimit sparqlSolutionModifierLimit)
        {
        
                LimitOffset = sparqlSolutionModifierLimit.LimitOffset;
        }

        internal void Add(SparqlSolutionModifierOrder sparqlSolutionModifierOrder)
        {
                Order = sparqlSolutionModifierOrder.Order;
        }

        internal void Add(SparqlSolutionModifierHaving sparqlSolutionModifierHaving)
        {
            Having = sparqlSolutionModifierHaving.Having;
        }

        internal void Add(SparqlSolutionModifierGroup sparqlSolutionModifierGroup)
        {
            Group = sparqlSolutionModifierGroup.Group;
        }

        internal void Add(SparqlSelect projection)
        {
            Select = projection;
        }
        public IEnumerable<SparqlResult> Run( IEnumerable<SparqlResult> results, SparqlResultSet sparqlResultSet=null)
        {
            if (Group != null)
                results = Group(results);
            if (Having != null)
                results = Having(results);

            if (Order != null)
                results = Order(results);

            if (Select != null)
                results = Select.Run(results, sparqlResultSet);

            if (LimitOffset!= null)
                results = LimitOffset(results);
            return results;
        }
    }
}
