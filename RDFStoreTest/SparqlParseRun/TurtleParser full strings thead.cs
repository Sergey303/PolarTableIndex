using System;
using System.IO;
using Antlr4.Runtime;
using SparqlParseRun.RdfCommon;

namespace SparqlParseRun
{
    public static class TurtleParserFullstringsThread
    {
        public static void TurtleThread(Stream turtlefileStream, Action<string, string, ObjectVariant> foreachTriple)
        {
           TtlGrammar_fullstrParser parser =
                new TtlGrammar_fullstrParser(new CommonTokenStream(new TtlGrammar_fullstrLexer(new AntlrInputStream(turtlefileStream))));
           parser.turtleDoc(foreachTriple);
          
        }

        public static void TurtleThread(string graphString, Action<string, string, ObjectVariant> foreachTriple)
        {
            TtlGrammar_fullstrParser parser =
                new TtlGrammar_fullstrParser(new CommonTokenStream(new TtlGrammar_fullstrLexer(new AntlrInputStream(graphString))));
            parser.turtleDoc(foreachTriple);
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
