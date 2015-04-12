using System;
using System.Collections.Generic;
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.RdfCommon
{
    public class NodeGeneratorFullString
    {
        public ObjectVariant CreateLiteralNode(string p)
        {
            p = p.Trim('"','\'');   
            return new ObjectVariant(3, p);//SimpleLiteralNode
        }

        public ObjectVariant CreateLang(string s, string lang)
        {
           s = s.Trim('"','\'');
          return new ObjectVariant(4,new object[]{s, lang}); 
          
        }

        public ObjectVariant CreateLiteralNode(int parse)
        {
            return new ObjectVariant(8, parse);//SimpleLiteralNode

        }

        public ObjectVariant CreateLiteralNode(decimal p)
        {
            return new ObjectVariant(6, p);//SimpleLiteralNode
        }

        public ObjectVariant CreateLiteralNode(double p)
        {
            return new ObjectVariant(5, p);//SimpleLiteralNode
        }

        public ObjectVariant CreateLiteralNode(bool p)
        {
            return new ObjectVariant(1, p);// ? BoolLiteralNode.TrueNode((SpecialTypes.Bool)) : BoolLiteralNode.FalseNode((SpecialTypes.Bool));
        }

        public ObjectVariant CreateLiteralNode(string p, string typeUriNode)
        {
            p = p.Trim('"','\'');
            typeUriNode = typeUriNode.ToLower();
            if (typeUriNode == SpecialTypes.String.FullName)
                return new ObjectVariant(2, p);
            else if (typeUriNode == SpecialTypes.Date.FullName)
            {
                DateTime date;
                if(!DateTime.TryParse(p, out date)) throw new ArgumentException(p);
                return new ObjectVariant(10, date.Ticks);
            }
            else if (typeUriNode == SpecialTypes.DateTime.FullName)
            {
                DateTimeOffset date;
                if (!DateTimeOffset.TryParse(p, out date)) throw new ArgumentException(p);
                return new ObjectVariant(9, date.Ticks);
            }
            else if (typeUriNode == (SpecialTypes.Bool.FullName))
            {
                bool b;
                if (!bool.TryParse(p, out b)) throw new ArgumentException(p);
                return new ObjectVariant(1,b);
            }
            else if (typeUriNode == SpecialTypes.Decimal.FullName)
            {
                decimal d;
                if (!decimal.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
             return new ObjectVariant(6,d);
            }
            else if (typeUriNode == SpecialTypes.Double.FullName)
            {
                double d;
                if (!double.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                return new ObjectVariant(5,p);
            }
            else if (typeUriNode == SpecialTypes.Float.FullName)
            {
                float f;
                if (!float.TryParse(p.Replace(".",","), out f)) throw new ArgumentException(p);
            return new ObjectVariant(7, f);
            }
            else if (typeUriNode == SpecialTypes.Integer.FullName)
            {
                int i;
                if (!int.TryParse(p, out i)) throw new ArgumentException(p);
                return new ObjectVariant(8, i);
            }
            else if (typeUriNode == SpecialTypes.DayTimeDuration.FullName)
            {
                TimeSpan i;
                if (!TimeSpan.TryParse(p, out i)) throw new ArgumentException(p);
                return new ObjectVariant(11, i.Ticks);
            }
            else 
            return new ObjectVariant(12, new object[]{p, typeUriNode});   
        }

        public string CreateBlankNode(string graph, string blankNodeString = null)
        {
            if (graph != null) blankNodeString = graph + "/" + blankNodeString;
            if(blankNodeString==null)
                blankNodeString = "blank" + (long)(random.NextDouble() * 1000 * 1000 * 1000 * 1000);

          
            return blankNodeString;
        }

        private Random random = new Random();

      
    }
    
}