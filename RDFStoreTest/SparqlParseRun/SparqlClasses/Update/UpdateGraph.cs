using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class UpdateGraph
    {
        public SparqlGrpahRefTypeEnum SparqlGrpahRefTypeEnum;
        public IUriNode UriNode;

        public UpdateGraph(IUriNode uriNode)
        {
            // TODO: Complete member initialization
            this.UriNode = uriNode;
            SparqlGrpahRefTypeEnum = SparqlGrpahRefTypeEnum.Setted;
        }

        public UpdateGraph(SparqlGrpahRefTypeEnum sparqlGrpahRefTypeEnum1)
        {
            // TODO: Complete member initialization
            this.SparqlGrpahRefTypeEnum = sparqlGrpahRefTypeEnum1;
        }

      

       
    }
}