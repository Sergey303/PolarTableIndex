using System;
using System.Collections.Generic;
using System.Linq;
using PolarDB;

namespace PolarTableIndex
{
    internal static class Class2PolarType
    {
        public static PType ToPolar<T>()
        {
            var type = typeof (T);
            // none = 0,
            return ToPType(type);
        }

        private static PType ToPType(Type type)
        {
            if (type == typeof (bool))
                return new PType(PTypeEnumeration.boolean);
            if (type == typeof (char))
                return new PType(PTypeEnumeration.character);
            if (type == typeof (Int32))
                return new PType(PTypeEnumeration.integer);
            if (type == typeof (long))
                return new PType(PTypeEnumeration.longinteger);
            if (type == typeof (double))
                return new PType(PTypeEnumeration.real); //  real = 5,
            if (type == typeof (byte))
                return new PType(PTypeEnumeration.@byte);
            if (type == typeof (string)) //fstring = 6, sstring = 7,

                return new PType(PTypeEnumeration.sstring);
            if (type.IsArray)
                return new PTypeSequence(ToPType(type.GetElementType())); 
            //union = 10,
            //record = 8,
            return
                new PTypeRecord(
                    type.GetProperties().Select(info => new NamedType(info.Name, ToPType(info.PropertyType))).ToArray());



        }


        public static PType ToPolar<TSeq, TElem>() where TSeq : IEnumerable<TElem> //sequence = 9,
        {
            return new PTypeSequence(ToPolar<TElem>());
        }
    
    }
}
