using System;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun.SparqlClasses.Update
{
    public class SparqlUpdateDrop : SparqlUpdateSilent
    {
        public UpdateGraph Graph;
        public override void RunUnSilent(IStore store)
        {
            switch (Graph.SparqlGrpahRefTypeEnum)
            {
                case SparqlGrpahRefTypeEnum.Setted:
                    store.NamedGraphs.DropGraph(Graph.UriNode);
                    break;
                case SparqlGrpahRefTypeEnum.Default:
                    store.Clear();
                    break;
                case SparqlGrpahRefTypeEnum.Named:
                    store.NamedGraphs.ClearAllNamedGraphs();
                    break;
                case SparqlGrpahRefTypeEnum.All:
                    store.ClearAll();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
