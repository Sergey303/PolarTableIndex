using System.IO;
using Antlr4.Runtime;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun
{
    public static class TurtleParser
    {
        public static IGraph FromTurtle(this IGraph graph, Stream turtlefileStream)
        {
           TtlGrammarParser parser =
                new TtlGrammarParser(new CommonTokenStream(new TtlGrammarLexer(new AntlrInputStream(turtlefileStream))));
            parser.turtleDoc(graph);
            return graph;
        }

        public static IGraph FromTurtle(this IGraph graph, string graphString)
        {
            TtlGrammarParser parser =
                new TtlGrammarParser(new CommonTokenStream(new TtlGrammarLexer(new AntlrInputStream(graphString))));
            parser.turtleDoc(graph);

            return graph;
        }

        //public static string ToTurtle(this IGraph graph)
        //{
        //    return string.Join(Environment.NewLine,
        //        graph.GetAllSubjects()
        //            .Select(subjectNode => subjectNode + Environment.NewLine +
        //                                   string.Join(@";" + Environment.NewLine+"     ",
        //                                       graph.GetTriplesWithSubject(subjectNode)
        //                                           .GroupBy(triple => triple.Predicate)
        //                                           .Select(pGroup =>"   " + pGroup.Key +string.Join(@",\n\r              ", pGroup)))
        //            )+".");
        //}

}
}
