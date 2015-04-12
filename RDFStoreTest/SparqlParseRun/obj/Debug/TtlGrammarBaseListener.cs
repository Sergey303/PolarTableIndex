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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ITtlGrammarListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.3")]
[System.CLSCompliant(false)]
public partial class TtlGrammarBaseListener : ITtlGrammarListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.iriString"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIriString([NotNull] TtlGrammarParser.IriStringContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.iriString"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIriString([NotNull] TtlGrammarParser.IriStringContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.sparqlBase"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSparqlBase([NotNull] TtlGrammarParser.SparqlBaseContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.sparqlBase"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSparqlBase([NotNull] TtlGrammarParser.SparqlBaseContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.prefixID"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrefixID([NotNull] TtlGrammarParser.PrefixIDContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.prefixID"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrefixID([NotNull] TtlGrammarParser.PrefixIDContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.subject"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSubject([NotNull] TtlGrammarParser.SubjectContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.subject"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSubject([NotNull] TtlGrammarParser.SubjectContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.predicate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPredicate([NotNull] TtlGrammarParser.PredicateContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.predicate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPredicate([NotNull] TtlGrammarParser.PredicateContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.rDFLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRDFLiteral([NotNull] TtlGrammarParser.RDFLiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.rDFLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRDFLiteral([NotNull] TtlGrammarParser.RDFLiteralContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.booleanLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBooleanLiteral([NotNull] TtlGrammarParser.BooleanLiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.booleanLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBooleanLiteral([NotNull] TtlGrammarParser.BooleanLiteralContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.collection"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCollection([NotNull] TtlGrammarParser.CollectionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.collection"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCollection([NotNull] TtlGrammarParser.CollectionContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.@object"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterObject([NotNull] TtlGrammarParser.ObjectContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.@object"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitObject([NotNull] TtlGrammarParser.ObjectContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.objectList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterObjectList([NotNull] TtlGrammarParser.ObjectListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.objectList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitObjectList([NotNull] TtlGrammarParser.ObjectListContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.iri"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIri([NotNull] TtlGrammarParser.IriContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.iri"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIri([NotNull] TtlGrammarParser.IriContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.blankNode"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlankNode([NotNull] TtlGrammarParser.BlankNodeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.blankNode"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlankNode([NotNull] TtlGrammarParser.BlankNodeContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.turtleDoc"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTurtleDoc([NotNull] TtlGrammarParser.TurtleDocContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.turtleDoc"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTurtleDoc([NotNull] TtlGrammarParser.TurtleDocContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] TtlGrammarParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] TtlGrammarParser.StatementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.@string"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterString([NotNull] TtlGrammarParser.StringContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.@string"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitString([NotNull] TtlGrammarParser.StringContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.blankNodePropertyList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlankNodePropertyList([NotNull] TtlGrammarParser.BlankNodePropertyListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.blankNodePropertyList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlankNodePropertyList([NotNull] TtlGrammarParser.BlankNodePropertyListContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.@base"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBase([NotNull] TtlGrammarParser.BaseContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.@base"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBase([NotNull] TtlGrammarParser.BaseContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.triples"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTriples([NotNull] TtlGrammarParser.TriplesContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.triples"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTriples([NotNull] TtlGrammarParser.TriplesContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.predicateObjectList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPredicateObjectList([NotNull] TtlGrammarParser.PredicateObjectListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.predicateObjectList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPredicateObjectList([NotNull] TtlGrammarParser.PredicateObjectListContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.boolean"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBoolean([NotNull] TtlGrammarParser.BooleanContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.boolean"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBoolean([NotNull] TtlGrammarParser.BooleanContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDirective([NotNull] TtlGrammarParser.DirectiveContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDirective([NotNull] TtlGrammarParser.DirectiveContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.numericLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNumericLiteral([NotNull] TtlGrammarParser.NumericLiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.numericLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNumericLiteral([NotNull] TtlGrammarParser.NumericLiteralContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.sparqlPrefix"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSparqlPrefix([NotNull] TtlGrammarParser.SparqlPrefixContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.sparqlPrefix"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSparqlPrefix([NotNull] TtlGrammarParser.SparqlPrefixContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammarParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteral([NotNull] TtlGrammarParser.LiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammarParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteral([NotNull] TtlGrammarParser.LiteralContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace SparqlParseRun
