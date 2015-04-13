using System;

namespace SparqlParseRun.RdfCommon.Literals
{
    public class LiteralofTypeStringNode : TypedLiteralNode, IStringLiteralNode
    {
       
        public LiteralofTypeStringNode(string Value, IUriNode uriType) : base(Value, uriType)
        {
        }

        public bool ComparebleWith(ILiteralNode other)
        {
            return other.LiteralType != LiteralType.LanguageType;
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
    }
}