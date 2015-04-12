using System;

namespace SparqlParseRun.RdfCommon.Literals
{
    public class SimpleLiteralNode : IStringLiteralNode 
    {
        

        public SimpleLiteralNode(string content, IUriNode dataType) 
        {
            Content = content;
            DataType = dataType;
        }

        //public bool ComparebleWith(StringLiteralNode other)
        //{
        //    var languagedStringNode = this as SparqlLanguagedStringNode;
        //    var otherLanguagedStringNode = other as SparqlLanguagedStringNode;
        //    if (languagedStringNode != null)
        //    {
        //        if (otherLanguagedStringNode != null)
        //        {
        //            return languagedStringNode.Lang == otherLanguagedStringNode.Lang;
        //        }
        //        else if (other is LiteralofTypeStringNode)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else if (this is LiteralofTypeStringNode)
        //    {
        //        if (other is SparqlLanguagedStringNode)
        //        {
        //            return false;
        //        }
        //        else if (other is LiteralofTypeStringNode)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        if (other is SparqlLanguagedStringNode)
        //        {
        //            return false;
        //        }
        //        else if (other is LiteralofTypeStringNode)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //}
       

        public LiteralType LiteralType
        {
            get { return LiteralType.Simple; }
        }

        public IUriNode DataType { get; private set; }
        public dynamic Content { get; private set; }
        public bool ComparebleWith(ILiteralNode other)
        {
            return other.LiteralType != LiteralType.LanguageType;
        }

        public int CompareTo(object obj)
        {
            var stringLiteralNode = obj as IStringLiteralNode;
            if( stringLiteralNode != null)
                return string.Compare( Content , stringLiteralNode.Content);
            else
            {
                throw new NotImplementedException();
            }
        }

        public NodeType Type { get{return NodeType.Literal;} }
        public override bool Equals(object obj)
        {
            var simpleLiteralNode = obj as SimpleLiteralNode;
            return simpleLiteralNode != null && simpleLiteralNode.Content==Content;
        }
        protected bool Equals(SimpleLiteralNode other)
        {
            return Equals(Content, other.Content);
        }

        public override int GetHashCode()
        {
            return (Content != null ? Content.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Content;
        }
    }
}