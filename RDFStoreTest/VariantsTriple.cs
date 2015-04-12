using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RDFStoreTest
{
    public struct VariantsTriple
    {
        public readonly string subject, predicate;
        public readonly ObjectVariants Object;

        public VariantsTriple(string subject, string predicate, ObjectVariants o)
        {
            this.subject = subject;
            this.predicate = predicate;
            Object = o;
        }
    }
}
