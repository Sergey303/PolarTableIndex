using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateCopy : SparqlUpdateSilent
    {
        SparqlUpdateClear clear = new SparqlUpdateClear(){Graph = new UpdateGraph(SparqlGrpahRefTypeEnum.Default)};
        SparqlUpdateAdd add = new SparqlUpdateAdd();

        public override void RunUnSilent(IStore store)
        {
            clear.RunUnSilent(store);
            add.RunUnSilent(store);
        }

        public IUriNode To{
            set
            {
                clear.Graph.UriNode = value;
                clear.Graph.SparqlGrpahRefTypeEnum = SparqlGrpahRefTypeEnum.Setted;
                add.To = value;
            }
        }

        public IUriNode From
        {
            set
            {
                add.From = value;
            }
        }
    }
}
