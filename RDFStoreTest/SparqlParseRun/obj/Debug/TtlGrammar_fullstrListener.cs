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
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="TtlGrammar_fullstrParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.3")]
[System.CLSCompliant(false)]
public interface ITtlGrammar_fullstrListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlBase"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSparqlBase([NotNull] TtlGrammar_fullstrParser.SparqlBaseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlBase"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSparqlBase([NotNull] TtlGrammar_fullstrParser.SparqlBaseContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.prefixID"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrefixID([NotNull] TtlGrammar_fullstrParser.PrefixIDContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.prefixID"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrefixID([NotNull] TtlGrammar_fullstrParser.PrefixIDContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.subject"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSubject([NotNull] TtlGrammar_fullstrParser.SubjectContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.subject"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSubject([NotNull] TtlGrammar_fullstrParser.SubjectContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicate"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPredicate([NotNull] TtlGrammar_fullstrParser.PredicateContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicate"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPredicate([NotNull] TtlGrammar_fullstrParser.PredicateContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.rDFLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRDFLiteral([NotNull] TtlGrammar_fullstrParser.RDFLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.rDFLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRDFLiteral([NotNull] TtlGrammar_fullstrParser.RDFLiteralContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.booleanLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBooleanLiteral([NotNull] TtlGrammar_fullstrParser.BooleanLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.booleanLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBooleanLiteral([NotNull] TtlGrammar_fullstrParser.BooleanLiteralContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.collection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCollection([NotNull] TtlGrammar_fullstrParser.CollectionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.collection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCollection([NotNull] TtlGrammar_fullstrParser.CollectionContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.object"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterObject([NotNull] TtlGrammar_fullstrParser.ObjectContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.object"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitObject([NotNull] TtlGrammar_fullstrParser.ObjectContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.objectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterObjectList([NotNull] TtlGrammar_fullstrParser.ObjectListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.objectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitObjectList([NotNull] TtlGrammar_fullstrParser.ObjectListContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.iri"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIri([NotNull] TtlGrammar_fullstrParser.IriContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.iri"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIri([NotNull] TtlGrammar_fullstrParser.IriContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlankNode([NotNull] TtlGrammar_fullstrParser.BlankNodeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlankNode([NotNull] TtlGrammar_fullstrParser.BlankNodeContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.turtleDoc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTurtleDoc([NotNull] TtlGrammar_fullstrParser.TurtleDocContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.turtleDoc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTurtleDoc([NotNull] TtlGrammar_fullstrParser.TurtleDocContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] TtlGrammar_fullstrParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] TtlGrammar_fullstrParser.StatementContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterString([NotNull] TtlGrammar_fullstrParser.StringContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitString([NotNull] TtlGrammar_fullstrParser.StringContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNodePropertyList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlankNodePropertyList([NotNull] TtlGrammar_fullstrParser.BlankNodePropertyListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNodePropertyList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlankNodePropertyList([NotNull] TtlGrammar_fullstrParser.BlankNodePropertyListContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.base"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBase([NotNull] TtlGrammar_fullstrParser.BaseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.base"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBase([NotNull] TtlGrammar_fullstrParser.BaseContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.triples"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTriples([NotNull] TtlGrammar_fullstrParser.TriplesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.triples"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTriples([NotNull] TtlGrammar_fullstrParser.TriplesContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicateObjectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPredicateObjectList([NotNull] TtlGrammar_fullstrParser.PredicateObjectListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicateObjectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPredicateObjectList([NotNull] TtlGrammar_fullstrParser.PredicateObjectListContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.boolean"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolean([NotNull] TtlGrammar_fullstrParser.BooleanContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.boolean"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolean([NotNull] TtlGrammar_fullstrParser.BooleanContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDirective([NotNull] TtlGrammar_fullstrParser.DirectiveContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDirective([NotNull] TtlGrammar_fullstrParser.DirectiveContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.numericLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNumericLiteral([NotNull] TtlGrammar_fullstrParser.NumericLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.numericLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNumericLiteral([NotNull] TtlGrammar_fullstrParser.NumericLiteralContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlPrefix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSparqlPrefix([NotNull] TtlGrammar_fullstrParser.SparqlPrefixContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlPrefix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSparqlPrefix([NotNull] TtlGrammar_fullstrParser.SparqlPrefixContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteral([NotNull] TtlGrammar_fullstrParser.LiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteral([NotNull] TtlGrammar_fullstrParser.LiteralContext context);
}
} // namespace SparqlParseRun