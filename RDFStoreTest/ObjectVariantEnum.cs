using System;
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

   }