using System;
using System.Collections.Generic;
using System.Linq;
using SparqlParseRun.SparqlClasses.Query.Result;

namespace SparqlParseRun.SparqlClasses.Expressions
{
    public abstract class SparqlExpression
    {
        public Func<SparqlResult, dynamic> Func;
        public bool IsAggragate;
        public bool IsDistinct;
        internal virtual void EqualsExpression(SparqlExpression value, RdfQuery11Translator q)
        {
            IsDistinct = IsDistinct || value.IsDistinct;
            IsAggragate = IsAggragate || value.IsAggragate;  
            var funkClone = FunkClone;   
            Func = result =>
            {
                var firstVar = this as SparqlVarExpression;
                var secondVar = value as SparqlVarExpression;
                SparqlVariableBinding firstVarValue;
                SparqlVariableBinding secondVarValue;
                var firstsKnowns = firstVar == null || result.row.TryGetValue(firstVar.Variable, out firstVarValue);
                var secondsKnowns = secondVar == null || result.row.TryGetValue(secondVar.Variable, out secondVarValue);
                if (firstsKnowns)
                {               
                    if (secondsKnowns) return funkClone(result).Equals(value.Func(result));
                    result.row.Add(secondVar.Variable,
                        new SparqlVariableBinding(secondVar.Variable, q.CreateLiteralIfNotNode(funkClone(result))));
                    return true;
                }
                if (secondsKnowns)
                    result.row.Add(firstVar.Variable,
                        new SparqlVariableBinding(firstVar.Variable, q.CreateLiteralIfNotNode(value.Func(result))));
                else throw new NotImplementedException();

                return true;
            };
        }

        public virtual void NotEquals(SparqlExpression value)
        {

            IsAggragate = IsAggragate || value.IsAggragate;
            IsDistinct = IsDistinct || value.IsDistinct;
                      var funkClone = FunkClone;    
            Func = result => !funkClone(result).Equals(value.Func(result));
        }

        public virtual void Smaller(SparqlExpression value)
        {

            IsAggragate = IsAggragate || value.IsAggragate;
            IsDistinct = IsDistinct || value.IsDistinct;
            var funkClone = FunkClone;
            Func = result => funkClone(result) < value.Func(result);
        }

        internal virtual void Greather(SparqlExpression value)
        {

            IsAggragate = IsAggragate || value.IsAggragate;
            IsDistinct = IsDistinct || value.IsDistinct;
            var funkClone = FunkClone;
            Func = result =>
            {
                var clone = funkClone(result);
                var func = value.Func(result);
                return clone > func;
            };
        }

        internal virtual void SmallerOrEquals(SparqlExpression value)
        {

            IsAggragate = IsAggragate || value.IsAggragate;
            IsDistinct = IsDistinct || value.IsDistinct;
            var funkClone = FunkClone;
            Func = result => funkClone(result) <= value.Func(result);
        }

        internal virtual void GreatherOrEquals(SparqlExpression value)
        {
            IsAggragate = IsAggragate || value.IsAggragate;
            IsDistinct = IsDistinct || value.IsDistinct;
                      var funkClone = FunkClone;    
            Func = result => funkClone(result) >= value.Func(result);
        }

        internal virtual void InCollection(List<SparqlExpression> collection)
        {
           
            var funkClone = FunkClone;
            Func = result =>
            {
                var v= funkClone(result);
                return collection.Any(element =>
                {
                    IsAggragate = IsAggragate || element.IsAggragate;
                    IsDistinct = IsDistinct || element.IsDistinct;
                    return element.Func(result).Equals(v);
                });
            };
        }

        internal virtual void NotInCollection(List<SparqlExpression> collection)
        {              
            var funkClone = FunkClone;
            Func = result =>
            {
                var v = funkClone(result);
                return !collection.Any(element =>
                {
                    IsAggragate = IsAggragate || element.IsAggragate;
                    IsDistinct = IsDistinct || element.IsDistinct;
                    return element.Func(result).Equals(v);
                });
            };
        }

        public static SparqlExpression operator +(SparqlExpression l, SparqlExpression r)
        {
            var funkClone =  l.FunkClone;
            l.Func = result =>funkClone(result) + r.Func(result);
            l.IsAggragate = l.IsAggragate || r.IsAggragate;
            l.IsDistinct = l.IsDistinct || r.IsDistinct;
            return l;
        }

        public static SparqlExpression operator -(SparqlExpression l, SparqlExpression r)
        {
            var funkClone = l.FunkClone;
            l.Func = result => funkClone(result) - r.Func(result);
            l.IsAggragate = l.IsAggragate || r.IsAggragate;
            l.IsDistinct = l.IsDistinct || r.IsDistinct;
            return l;
        }

        public static SparqlExpression operator *(SparqlExpression l, SparqlExpression r)
        {
            var funkClone = l.FunkClone;
            l.Func = result => funkClone(result) * r.Func(result);
            l.IsAggragate = l.IsAggragate || r.IsAggragate;
            l.IsDistinct = l.IsDistinct || r.IsDistinct;
            return l;          
        }

        public static SparqlExpression operator /(SparqlExpression l, SparqlExpression r)
        {
            var funkClone = l.FunkClone;
            l.Func = result => funkClone(result) / r.Func(result);
            l.IsAggragate = l.IsAggragate || r.IsAggragate;
            l.IsDistinct = l.IsDistinct || r.IsDistinct;
            return l;
        }

        public static SparqlExpression operator !(SparqlExpression e)
        {
            var funkClone = e.FunkClone;
            e.Func = result => !funkClone(result);
            return e;
        }

        public static SparqlExpression operator -(SparqlExpression e)
        {
            var funkClone = e.FunkClone;
            e.Func = result => !funkClone(result);
            return e;
        }

        internal bool Test(SparqlResult result)
        {
            return Func(result);
        }

        public Func<SparqlResult, dynamic> FunkClone
        {
            get
            {
                return (Func<SparqlResult, dynamic>) Func.Clone();
            }
        }
}
}
