using System;
using System.Net.NetworkInformation;
using PolarDB;

namespace RDFStoreTest
{
  
    public enum ObjectVariantEnum
    {
        Iri, IriInt, Bool, Str, Lang, Double, Decimal, Float,Int, DateTimeZone,DateTime, Date, Time, Other, OtherIntType
    }

    public static class ObjectVariantsPolarType
    {
        public static readonly PTypeUnion ObjectVariantPolarType = new PTypeUnion(
            new NamedType("iri", new PType(PTypeEnumeration.sstring)),
            new NamedType("iri coded", new PType(PTypeEnumeration.integer)),
            new NamedType("bool", new PType(PTypeEnumeration.boolean)),
            new NamedType("str", new PType(PTypeEnumeration.sstring)),
            new NamedType("lang str",
                new PTypeRecord(new NamedType("str", new PType(PTypeEnumeration.sstring)),
                    new NamedType("lang", new PType(PTypeEnumeration.sstring)))),
            new NamedType("double", new PType(PTypeEnumeration.real)),
            new NamedType("decimal", new PType(PTypeEnumeration.real)),
            new NamedType("float", new PType(PTypeEnumeration.real)),
            new NamedType("int", new PType(PTypeEnumeration.integer)),
            new NamedType("date time zone", new PType(PTypeEnumeration.longinteger)),
            new NamedType("date time", new PType(PTypeEnumeration.longinteger)),
            new NamedType("date", new PType(PTypeEnumeration.longinteger)),
            new NamedType("time", new PType(PTypeEnumeration.longinteger)),
            new NamedType("other",
                new PTypeRecord(new NamedType("str", new PType(PTypeEnumeration.sstring)),
                    new NamedType("type", new PType(PTypeEnumeration.sstring)))),
            new NamedType("other coded type",
                new PTypeRecord(new NamedType("str", new PType(PTypeEnumeration.sstring)),
                    new NamedType("type", new PType(PTypeEnumeration.integer)))));
    }
    class ObjectVariantsEx
    {
        public static readonly Func<object, IComparable>[] w2c = new Func<object, IComparable>[]
            {
                  s=>new Comparer2(0, (IComparable) s),
                  s=>new Comparer2(1, (IComparable) s),   
                  s=>new Comparer2(2, (IComparable) s),   
                  s=>new Comparer2(3, (IComparable) s),   
                  strLang=> new Comparer3(4, (IComparable) ((object[])strLang)[1],(IComparable) ((object[])strLang)[0]),
                  s=>new Comparer2(5, (IComparable) s),   
                  s=>new Comparer2(6, (IComparable) s),   
                  s=>new Comparer2(7, (IComparable) s),   
                  s=>new Comparer2(8, (IComparable) s),   
                  s=>new Comparer2(9, (IComparable) s),   
                  s=>new Comparer2(10, (IComparable) s),   
                  s=>new Comparer2(11, (IComparable) s),   
                  s=>new Comparer2(12, (IComparable) s),   
                  typed=>new Comparer3(13,(IComparable) ((object[])typed)[1],(IComparable) ((object[])typed)[0]),   
                  typed=>new Comparer3(14, (IComparable) ((object[])typed)[1],(IComparable) ((object[])typed)[0]),   
            };


        public static IComparable Writeble2Comparable(object[] @object)
        {
            return w2c[(int) @object[0]](@object[1]);
        }

    }

   }