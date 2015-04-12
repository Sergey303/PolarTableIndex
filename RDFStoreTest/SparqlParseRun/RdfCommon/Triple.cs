
using SparqlParseRun.RdfCommon.Literals;

namespace SparqlParseRun.RdfCommon
{
    public class Triple
    {
        protected readonly ISubjectNode subj;
        protected readonly IUriNode pred;
        protected readonly INode obj;

        public Triple(ISubjectNode subj, IUriNode pred, INode obj) 
        { 
            this.subj = subj; this.pred = pred; this.obj = obj;
            //this.g = subj.Graph;
           // if (!g.Equals(pred.Graph) || !g.Equals(obj.Graph)) throw new Exception("Err in Triple constructor");
        }

        public Triple(ISubjectNode subj, IUriNode p, IUriNode sUriNode2)
        {
            // TODO: Complete member initialization
            this.subj = subj;
            this.pred = p;
            this.obj = sUriNode2;
        }

        public Triple(ISubjectNode subj, IUriNode p, ILiteralNode sLiteralNode)
        {
            // TODO: Complete member initialization
            this.subj = subj;
            this.pred = p;
            this.obj = sLiteralNode;
        }


   
        public ISubjectNode Subject { get {  return subj; } }
        public IUriNode Predicate { get { return pred; } }
        public INode Object { get { return obj; } }
    }
}