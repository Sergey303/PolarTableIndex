using System;

namespace SparqlParseRun.RdfCommon.Literals
{
    public class SparqlLanguageLiteralNode : TypedLiteralNode, ILanguageLiteral

    {
        public readonly string lang;

        public SparqlLanguageLiteralNode(string text, string lang, IUriNode uriType)
            : base(text, uriType)
        {
            this.lang = lang;
        }


        public override LiteralType LiteralType
        {
            get { return LiteralType.LanguageType; }
        }

        public bool ComparebleWith(ILiteralNode other)
        {
            return other.LiteralType != LiteralType.LanguageType || Lang == ((SparqlLanguageLiteralNode) other).Lang;
        }

        public string Lang
        {
            get { return lang; }
        }

        public int CompareTo(object obj)
        {
            var stringLiteralNode = obj as IStringLiteralNode;
            if (stringLiteralNode != null)
                return string.Compare(Content, stringLiteralNode.Content);
            else
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return string.Format("\"{0}\"@{1}", Content, Lang);
        }

     
    }
}
