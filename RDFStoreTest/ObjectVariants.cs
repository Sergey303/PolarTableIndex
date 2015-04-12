using System;
using SparqlParseRun.RdfCommon;

namespace RDFStoreTest
{
    public abstract class ObjectVariants
    {
        public  abstract ObjectVariantEnum Variant { get; }
        public abstract object WritableValue { get; }

        public virtual object[] ToWritable()
        {
            return new object[]{(int)Variant, WritableValue};
        }
        
        public static ObjectVariants CreateLiteralNode(string p)
        {
            p = p.Trim('"', '\'');
            return new OV_string(p);//SimpleLiteralNode
        }

        public static ObjectVariants CreateLang(string s, string lang)
        {
            s = s.Trim('"', '\'');
            return new OV_langstring( s, lang );

        }

        public static ObjectVariants CreateLiteralNode(int parse)
        {
            return new OV_int(parse);//int

        }

        public static ObjectVariants CreateLiteralNode(decimal p)
        {
            return new OV_decimal(p);//decimal
        }

        public static ObjectVariants CreateLiteralNode(double p)
        {
            return new OV_double(p);//double
        }

        public static ObjectVariants CreateLiteralNode(bool p)
        {
            return new OV_bool(p);// ? BoolLiteralNode.TrueNode((SpecialTypes.Bool)) : BoolLiteralNode.FalseNode((SpecialTypes.Bool));
        }

        public static ObjectVariants CreateLiteralNode(string p, string typeUriNode)
        {
            p = p.Trim('"', '\'');
            typeUriNode = typeUriNode.ToLower();
            if (typeUriNode == SpecialTypes.String.FullName)
                return new OV_string(p);
            else if (typeUriNode == SpecialTypes.Date.FullName)
            {
                DateTime date;
                if (!DateTime.TryParse(p, out date)) throw new ArgumentException(p);
                return new OV_date(date);
            }
            else if (typeUriNode == SpecialTypes.DateTime.FullName)
            {
                DateTimeOffset date;
                if (!DateTimeOffset.TryParse(p, out date)) throw new ArgumentException(p);
                return new OV_dateTimeZone(date);
            }
            else if (typeUriNode == (SpecialTypes.Bool.FullName))
            {
                bool b;
                if (!bool.TryParse(p, out b)) throw new ArgumentException(p);
                return new OV_bool(b);
            }
            else if (typeUriNode == SpecialTypes.Decimal.FullName)
            {
                decimal d;
                if (!decimal.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                return new OV_decimal( d);
            }
            else if (typeUriNode == SpecialTypes.Double.FullName)
            {
                double d;
                if (!double.TryParse(p.Replace(".", ","), out d)) throw new ArgumentException(p);
                return new OV_double(d);
            }
            else if (typeUriNode == SpecialTypes.Float.FullName)
            {
                float f;
                if (!float.TryParse(p.Replace(".", ","), out f)) throw new ArgumentException(p);
                return new OV_float(f);
            }
            else if (typeUriNode == SpecialTypes.Integer.FullName)
            {
                int i;
                if (!int.TryParse(p, out i)) throw new ArgumentException(p);
                return new OV_int(i);
            }
            else if (typeUriNode == SpecialTypes.DayTimeDuration.FullName)
            {
                TimeSpan i;
                if (!TimeSpan.TryParse(p, out i)) throw new ArgumentException(p);
                return new OV_time(i);
            }
            else
                return new OV_typed(p, typeUriNode);
        }

        public string CreateBlankNode(string graph, string blankNodeString = null)
        {
            if (blankNodeString == null)
                blankNodeString = "blank" + (long)(random.NextDouble() * 1000 * 1000 * 1000 * 1000);

            if (graph != null) blankNodeString = graph + "/" + blankNodeString;

            return blankNodeString;
        }

        private static readonly Random random = new Random();
    }
    public class OV_iri : ObjectVariants
    {
        public string full_id;

        public OV_iri(string fullId)
        {
            full_id = fullId;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Iri; }
        }

        public override object WritableValue
        {
            get { return full_id; }
        }

    }
    public class OV_string : ObjectVariants
    {
        public string value;

        public OV_string(string value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Str; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_iriint : ObjectVariants
    {
        public int code;

        public OV_iriint(int code)
        {
            this.code = code;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.IriInt; }
        }

        public override object WritableValue
        {
            get { return code; }
        }

    }
    public class OV_bool : ObjectVariants
    {
        public bool value;

        public OV_bool(bool value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Bool; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_int : ObjectVariants
    {
        public int value;

        public OV_int(int value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Int; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_double : ObjectVariants
    {
        public double value;

        public OV_double(double value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Double; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_decimal : ObjectVariants
    {
        public decimal value;

        public OV_decimal(decimal value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Decimal; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_float : ObjectVariants
    {
        public float value;

        public OV_float(float value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Float; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_typed : ObjectVariants
    {
        public string value; public string turi;

        public OV_typed(string value, string turi)
        {
            this.value = value;
            this.turi = turi;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Other; }
        }

        public override object WritableValue
        {
            get
            {
                return new object[] { value, turi };
            }
        }

    }
    public class OV_typedint : ObjectVariants
    {
        private readonly string value; public int curi;

        public OV_typedint(string value, int curi)
        {
            this.value = value;
            this.curi = curi;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.OtherIntType; }
        }

        public override object WritableValue
        {
            get { return new object[] { value, curi }; }
        }

    }
    public class OV_time : ObjectVariants
    {
        public TimeSpan value;

        public OV_time(TimeSpan value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Time; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_date : ObjectVariants
    {
        public DateTime value;

        public OV_date(DateTime value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Date; }
        }

        public override object WritableValue
        {
            get { return value; }
        }


    }
    public class OV_dateTime : ObjectVariants
    {
        public DateTime value;

        public OV_dateTime(DateTime value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.DateTime; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_dateTimeZone : ObjectVariants
    {
        public DateTimeOffset value;

        public OV_dateTimeZone(DateTimeOffset value)
        {
            this.value = value;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.DateTimeZone; }
        }

        public override object WritableValue
        {
            get { return value; }
        }

    }
    public class OV_langstring : ObjectVariants
    {
        public string value; public string lang;

        public OV_langstring(string value, string lang)
        {
            this.value = value;
            this.lang = lang;
        }

        public override ObjectVariantEnum Variant
        {
            get { return ObjectVariantEnum.Lang; }
        }

        public override object WritableValue
        {
            get { return new object[] { value, lang }; }
        }


    }

}