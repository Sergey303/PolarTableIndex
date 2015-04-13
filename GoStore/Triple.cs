using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStore
{
    public abstract class Triple
    {
        public string subj;
        public string pred;
    }
    public class OTriple : Triple
    {
        public string obj;
    }
    public class DTriple : Triple
    {
        public Literal data;
    }
    public enum LiteralVidEnumeration { unknown, integer, text, date }
    public class Literal
    {
        public LiteralVidEnumeration vid;
        public object value;
        public override string ToString()
        {
            switch (vid)
            {
                case LiteralVidEnumeration.text:
                    {
                        Text txt = (Text)value;
                        return "\"" + txt.s + "\"@" + txt.l;
                    }
                default: return value.ToString();
            }
        }
    }
    public class Text { public string s, l; }

}
