//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\Admin\Source\Repos\PolarDemo\SparqlParseRun\TtlGrammar_fullstr.g4 by ANTLR 4.3

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

namespace SparqlParseRun {

	using System;
	using System.Linq;
	using SparqlParseRun.RdfCommon;
	using SparqlParseRun.RdfCommon.Literals;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="TtlGrammar_fullstrParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.3")]
[System.CLSCompliant(false)]
public interface ITtlGrammar_fullstrVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlBase"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSparqlBase([NotNull] TtlGrammar_fullstrParser.SparqlBaseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.prefixID"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrefixID([NotNull] TtlGrammar_fullstrParser.PrefixIDContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.subject"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubject([NotNull] TtlGrammar_fullstrParser.SubjectContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicate"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPredicate([NotNull] TtlGrammar_fullstrParser.PredicateContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.rDFLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRDFLiteral([NotNull] TtlGrammar_fullstrParser.RDFLiteralContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.booleanLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBooleanLiteral([NotNull] TtlGrammar_fullstrParser.BooleanLiteralContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.collection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCollection([NotNull] TtlGrammar_fullstrParser.CollectionContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.object"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObject([NotNull] TtlGrammar_fullstrParser.ObjectContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.objectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjectList([NotNull] TtlGrammar_fullstrParser.ObjectListContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.iri"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIri([NotNull] TtlGrammar_fullstrParser.IriContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlankNode([NotNull] TtlGrammar_fullstrParser.BlankNodeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.turtleDoc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTurtleDoc([NotNull] TtlGrammar_fullstrParser.TurtleDocContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] TtlGrammar_fullstrParser.StatementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] TtlGrammar_fullstrParser.StringContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNodePropertyList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlankNodePropertyList([NotNull] TtlGrammar_fullstrParser.BlankNodePropertyListContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.base"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBase([NotNull] TtlGrammar_fullstrParser.BaseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.triples"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTriples([NotNull] TtlGrammar_fullstrParser.TriplesContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicateObjectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPredicateObjectList([NotNull] TtlGrammar_fullstrParser.PredicateObjectListContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.boolean"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolean([NotNull] TtlGrammar_fullstrParser.BooleanContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDirective([NotNull] TtlGrammar_fullstrParser.DirectiveContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.numericLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumericLiteral([NotNull] TtlGrammar_fullstrParser.NumericLiteralContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlPrefix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSparqlPrefix([NotNull] TtlGrammar_fullstrParser.SparqlPrefixContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="TtlGrammar_fullstrParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] TtlGrammar_fullstrParser.LiteralContext context);
}
} // namespace SparqlParseRun
