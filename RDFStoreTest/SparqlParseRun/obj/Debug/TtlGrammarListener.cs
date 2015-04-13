//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\Admin\Source\Repos\PolarDemo\SparqlParseRun\TtlGrammar.g4 by ANTLR 4.3

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
/// <see cref="TtlGrammarParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.3")]
[System.CLSCompliant(false)]
public interface ITtlGrammarListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.iriString"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIriString([NotNull] TtlGrammarParser.IriStringContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.iriString"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIriString([NotNull] TtlGrammarParser.IriStringContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.sparqlBase"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSparqlBase([NotNull] TtlGrammarParser.SparqlBaseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.sparqlBase"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSparqlBase([NotNull] TtlGrammarParser.SparqlBaseContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.prefixID"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrefixID([NotNull] TtlGrammarParser.PrefixIDContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.prefixID"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrefixID([NotNull] TtlGrammarParser.PrefixIDContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.subject"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSubject([NotNull] TtlGrammarParser.SubjectContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.subject"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSubject([NotNull] TtlGrammarParser.SubjectContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.predicate"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPredicate([NotNull] TtlGrammarParser.PredicateContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.predicate"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPredicate([NotNull] TtlGrammarParser.PredicateContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.rDFLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRDFLiteral([NotNull] TtlGrammarParser.RDFLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.rDFLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRDFLiteral([NotNull] TtlGrammarParser.RDFLiteralContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.booleanLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBooleanLiteral([NotNull] TtlGrammarParser.BooleanLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.booleanLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBooleanLiteral([NotNull] TtlGrammarParser.BooleanLiteralContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.collection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCollection([NotNull] TtlGrammarParser.CollectionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.collection"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCollection([NotNull] TtlGrammarParser.CollectionContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.object"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterObject([NotNull] TtlGrammarParser.ObjectContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.object"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitObject([NotNull] TtlGrammarParser.ObjectContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.objectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterObjectList([NotNull] TtlGrammarParser.ObjectListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.objectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitObjectList([NotNull] TtlGrammarParser.ObjectListContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.iri"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIri([NotNull] TtlGrammarParser.IriContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.iri"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIri([NotNull] TtlGrammarParser.IriContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.blankNode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlankNode([NotNull] TtlGrammarParser.BlankNodeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.blankNode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlankNode([NotNull] TtlGrammarParser.BlankNodeContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.turtleDoc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTurtleDoc([NotNull] TtlGrammarParser.TurtleDocContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.turtleDoc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTurtleDoc([NotNull] TtlGrammarParser.TurtleDocContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] TtlGrammarParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] TtlGrammarParser.StatementContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterString([NotNull] TtlGrammarParser.StringContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitString([NotNull] TtlGrammarParser.StringContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.blankNodePropertyList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlankNodePropertyList([NotNull] TtlGrammarParser.BlankNodePropertyListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.blankNodePropertyList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlankNodePropertyList([NotNull] TtlGrammarParser.BlankNodePropertyListContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.base"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBase([NotNull] TtlGrammarParser.BaseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.base"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBase([NotNull] TtlGrammarParser.BaseContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.triples"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTriples([NotNull] TtlGrammarParser.TriplesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.triples"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTriples([NotNull] TtlGrammarParser.TriplesContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.predicateObjectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPredicateObjectList([NotNull] TtlGrammarParser.PredicateObjectListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.predicateObjectList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPredicateObjectList([NotNull] TtlGrammarParser.PredicateObjectListContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.boolean"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolean([NotNull] TtlGrammarParser.BooleanContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.boolean"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolean([NotNull] TtlGrammarParser.BooleanContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDirective([NotNull] TtlGrammarParser.DirectiveContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.directive"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDirective([NotNull] TtlGrammarParser.DirectiveContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.numericLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNumericLiteral([NotNull] TtlGrammarParser.NumericLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.numericLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNumericLiteral([NotNull] TtlGrammarParser.NumericLiteralContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.sparqlPrefix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSparqlPrefix([NotNull] TtlGrammarParser.SparqlPrefixContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.sparqlPrefix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSparqlPrefix([NotNull] TtlGrammarParser.SparqlPrefixContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteral([NotNull] TtlGrammarParser.LiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteral([NotNull] TtlGrammarParser.LiteralContext context);
}
} // namespace SparqlParseRun