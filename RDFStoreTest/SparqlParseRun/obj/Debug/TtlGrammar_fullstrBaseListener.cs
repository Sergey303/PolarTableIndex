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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ITtlGrammar_fullstrListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.3")]
[System.CLSCompliant(false)]
public partial class TtlGrammar_fullstrBaseListener : ITtlGrammar_fullstrListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlBase"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSparqlBase([NotNull] TtlGrammar_fullstrParser.SparqlBaseContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlBase"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSparqlBase([NotNull] TtlGrammar_fullstrParser.SparqlBaseContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.prefixID"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrefixID([NotNull] TtlGrammar_fullstrParser.PrefixIDContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.prefixID"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrefixID([NotNull] TtlGrammar_fullstrParser.PrefixIDContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.subject"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSubject([NotNull] TtlGrammar_fullstrParser.SubjectContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.subject"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSubject([NotNull] TtlGrammar_fullstrParser.SubjectContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPredicate([NotNull] TtlGrammar_fullstrParser.PredicateContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPredicate([NotNull] TtlGrammar_fullstrParser.PredicateContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.rDFLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRDFLiteral([NotNull] TtlGrammar_fullstrParser.RDFLiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.rDFLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRDFLiteral([NotNull] TtlGrammar_fullstrParser.RDFLiteralContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.booleanLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBooleanLiteral([NotNull] TtlGrammar_fullstrParser.BooleanLiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.booleanLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBooleanLiteral([NotNull] TtlGrammar_fullstrParser.BooleanLiteralContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.collection"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCollection([NotNull] TtlGrammar_fullstrParser.CollectionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.collection"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCollection([NotNull] TtlGrammar_fullstrParser.CollectionContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.@object"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterObject([NotNull] TtlGrammar_fullstrParser.ObjectContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.@object"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitObject([NotNull] TtlGrammar_fullstrParser.ObjectContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.objectList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterObjectList([NotNull] TtlGrammar_fullstrParser.ObjectListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.objectList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitObjectList([NotNull] TtlGrammar_fullstrParser.ObjectListContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.iri"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIri([NotNull] TtlGrammar_fullstrParser.IriContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.iri"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIri([NotNull] TtlGrammar_fullstrParser.IriContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNode"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlankNode([NotNull] TtlGrammar_fullstrParser.BlankNodeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNode"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlankNode([NotNull] TtlGrammar_fullstrParser.BlankNodeContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.turtleDoc"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTurtleDoc([NotNull] TtlGrammar_fullstrParser.TurtleDocContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.turtleDoc"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTurtleDoc([NotNull] TtlGrammar_fullstrParser.TurtleDocContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] TtlGrammar_fullstrParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] TtlGrammar_fullstrParser.StatementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.@string"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterString([NotNull] TtlGrammar_fullstrParser.StringContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.@string"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitString([NotNull] TtlGrammar_fullstrParser.StringContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNodePropertyList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlankNodePropertyList([NotNull] TtlGrammar_fullstrParser.BlankNodePropertyListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.blankNodePropertyList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlankNodePropertyList([NotNull] TtlGrammar_fullstrParser.BlankNodePropertyListContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.@base"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBase([NotNull] TtlGrammar_fullstrParser.BaseContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.@base"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBase([NotNull] TtlGrammar_fullstrParser.BaseContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.triples"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTriples([NotNull] TtlGrammar_fullstrParser.TriplesContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.triples"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTriples([NotNull] TtlGrammar_fullstrParser.TriplesContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicateObjectList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPredicateObjectList([NotNull] TtlGrammar_fullstrParser.PredicateObjectListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.predicateObjectList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPredicateObjectList([NotNull] TtlGrammar_fullstrParser.PredicateObjectListContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.boolean"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBoolean([NotNull] TtlGrammar_fullstrParser.BooleanContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.boolean"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBoolean([NotNull] TtlGrammar_fullstrParser.BooleanContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDirective([NotNull] TtlGrammar_fullstrParser.DirectiveContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDirective([NotNull] TtlGrammar_fullstrParser.DirectiveContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.numericLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNumericLiteral([NotNull] TtlGrammar_fullstrParser.NumericLiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.numericLiteral"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNumericLiteral([NotNull] TtlGrammar_fullstrParser.NumericLiteralContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlPrefix"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSparqlPrefix([NotNull] TtlGrammar_fullstrParser.SparqlPrefixContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.sparqlPrefix"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSparqlPrefix([NotNull] TtlGrammar_fullstrParser.SparqlPrefixContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="TtlGrammar_fullstrParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteral([NotNull] TtlGrammar_fullstrParser.LiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TtlGrammar_fullstrParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteral([NotNull] TtlGrammar_fullstrParser.LiteralContext context) { }

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
