grammar TtlGrammar;

@header{
	using System;
	using System.Linq;
	using SparqlParseRun.RdfCommon;
	using SparqlParseRun.RdfCommon.Literals;
}
 @members{
public IGraph q;
        private readonly Prologue prologue = new Prologue();
}	

 	turtleDoc	[IGraph inputStore] : {q=$inputStore; }	statement*								   ;
	statement	:	directive | triples '.'						;
	directive	:	prefixID | base | sparqlPrefix | sparqlBase	  ;
	prefixID	:	'@prefix' PNAME_NS IRIREF '.'	{ prologue.AddPrefix($PNAME_NS.text, $IRIREF.text); }			  ;
	base	:	'@base' IRIREF  {prologue.SetBase($IRIREF.text);}'.'								 ;
	sparqlBase	:	'BASE' IRIREF {prologue.SetBase($IRIREF.text);}								 ;
	sparqlPrefix	:	'PREFIX' PNAME_NS IRIREF { prologue.AddPrefix($PNAME_NS.text, $IRIREF.text); }					;
	triples	:	subject predicateObjectList [$subject.value]
| blankNodePropertyList (predicateObjectList [$blankNodePropertyList.value ])?   ;
	predicateObjectList [ISubjectNode subj] :	
	predicate objectList [$subj, $predicate.value]
	( ';' ( predicate objectList [$subj, $predicate.value] )? )*;
	objectList [ISubjectNode subj, IUriNode pred] : object { q.Add($subj, $pred, $object.value);  } (',' object { q.Add($subj, $pred, $object.value); } )* ;
	//verb returns [IUriNode value]	: predicate  {$value = $predicate.value;};
	subject returns [ISubjectNode value]	:	iri {$value=$iri.value;} | blankNode {$value=$blankNode.value;} | collection {$value=$collection.value;} ;
	predicate returns [IUriNode value]	:	iri		{$value=$iri.value;}  | 'a' {$value = q.NodeGenerator.CreateUriNode(SpecialTypes.RdfType);};
	object returns [INode value]	:	iri {$value=$iri.value;}
	 | blankNode {$value=$blankNode.value;}
	 | collection {$value=$collection.value;}
	 | blankNodePropertyList {$value=$blankNodePropertyList.value;}
	 | literal {$value=$literal.value;}	   ;
	literal	returns [ILiteralNode value]:	rDFLiteral {$value=$rDFLiteral.value;} | numericLiteral {$value=$numericLiteral.value;} | booleanLiteral {$value=$booleanLiteral.value;};
	blankNodePropertyList	returns [ISubjectNode value] :	'[' {$value= q.NodeGenerator.CreateBlankNode(q.Name);} predicateObjectList [$value] ']' ;
	collection returns [ISubjectNode value] 	:	'(' {var c=new SparqlRdfCollection();} (object { c.nodes.Add($object.value); })* ')'	{$value=c.GetNode(t=> q.Add(t.Subject, t.Predicate, t.Object), s=>q.NodeGenerator.CreateUriNode(s), ()=>q.NodeGenerator.CreateBlankNode(q.Name)); } ;
	numericLiteral	returns [ILiteralNode value]:	INTEGER {$value=q.NodeGenerator.CreateLiteralNode(int.Parse($INTEGER.text));} | DECIMAL {$value=q.NodeGenerator.CreateLiteralNode(decimal.Parse($DECIMAL.text.Replace(".", ",")));}  | DOUBLE	{$value=q.NodeGenerator.CreateLiteralNode(double.Parse($DOUBLE.text.Replace(".", ",")));};
	rDFLiteral returns [ILiteralNode value] : string { $value=q.NodeGenerator.CreateLiteralNode($string.text); }  
 | (string LANGTAG { $value=q.NodeGenerator.CreateLiteralNode($string.text, $LANGTAG.text);} )
 | ( string '^^' iri { $value=q.NodeGenerator.CreateLiteralNode($string.text, $iri.value);} ) ;  
	  booleanLiteral returns [ILiteralNode value]
 :  boolean {$value=q.NodeGenerator.CreateLiteralNode($boolean.value); } ;
 boolean returns [bool value]
 :  'true' { $value=true; } 
 |	'false' { $value=false; } ;	  
	string	:	STRING_LITERAL_QUOTE | STRING_LITERAL_SINGLE_QUOTE | STRING_LITERAL_LONG_SINGLE_QUOTE | STRING_LITERAL_LONG_QUOTE  ;
	 iri returns [IUriNode value] : iriString { $value=q.NodeGenerator.CreateUriNode($iriString.value);};
 iriString returns [UriPrefixed value] : IRIREF {$value=prologue.GetFromIri($IRIREF.text.Substring(1, $IRIREF.text.Length-2));} |	PNAME_LN {$value=prologue.GetUriFromPrefixed($PNAME_LN.text); } | PNAME_NS {$value=prologue.GetUriFromPrefixedNamespace($PNAME_NS.text); };
	blankNode	returns[IBlankNode value]:	BLANK_NODE_LABEL {$value=q.NodeGenerator.CreateBlankNode($BLANK_NODE_LABEL.text,q.Name);} | ANON {$value=q.NodeGenerator.CreateBlankNode(q.Name);}	  ;

	IRIREF	: '<'([a-zA-Zà-ÿÀ-ß0-9:/\\#.%-@])*'>';
	//IRIREF	:	'<' ([^#x00-#x20<>"{}|^`\] | UCHAR)* '>' /* #x00=NULL #01-#x1F=control codes #x20=space */
	PNAME_NS	:	PN_PREFIX? ':'			  ;
	PNAME_LN	:	PNAME_NS PN_LOCAL		  ;
	BLANK_NODE_LABEL	:	'_:' (PN_CHARS_U | [0-9]) ((PN_CHARS | '.')* PN_CHARS)?	 ;
	LANGTAG	:	'@' [a-zA-Z]+ ('-' [a-zA-Z0-9]+)*						  ;
	INTEGER	:	[+-]? [0-9]+						 ;
	DECIMAL	:	[+-]? [0-9]* '.' [0-9]+			  ;
	DOUBLE	:	[+-]? ([0-9]+ '.' [0-9]* EXPONENT | '.' [0-9]+ EXPONENT | [0-9]+ EXPONENT) ;
	EXPONENT	:	[eE] [+-]? [0-9]+								 ;
	STRING_LITERAL_QUOTE : '"' ( (~(["\\\n\r])) | ECHAR | UCHAR)* '"';  /* #x22=" #x5C=\ #xA=new line #xD=carriage return */
	STRING_LITERAL_SINGLE_QUOTE : '\''(~(['\\\n\r]) |  ECHAR  | UCHAR)*'\''; /* #x27=' #x5C=\ #xA=new line #xD=carriage return */ 
	STRING_LITERAL_LONG_SINGLE_QUOTE	 :'\'\'\'' ( ( '\'' | '\'\'\'' )? ( ~[\'\\] | ECHAR | UCHAR) )* '\'\'\'' ;
	STRING_LITERAL_LONG_QUOTE 	       :'"""' ( ( '"' | '""' )? ( ~[\"\\] | ECHAR | UCHAR) )* '"""'  ;
	UCHAR	:	'\\u' HEX HEX HEX HEX | '\\U' HEX HEX HEX HEX HEX HEX HEX HEX  ;
	ECHAR	:	'\\' [tbnrf"'\\]	;
WS : [ \s\t\r\n]+ ->skip	;		 //	WS	:	\x20 | \x9 | \xD | \xA /* #x20=space #x9=character tabulation #xD=carriage return #xA=new line */ ;
	ANON	:	'[' WS* ']'	 ;
	PN_CHARS_BASE	:	[A-Z] | [a-z] | [\x00C0-\x00D6] | [\x00D8-\x00F6] | [\x00F8-\x02FF] | [\x0370-\x037D] | [\x037F-\x1FFF] | [\x200C-\x200D] | [\x2070-\x218F] | [\x2C00-\x2FEF] | [\x3001-\xD7FF] | [\xF900-\xFDCF] | [\xFDF0-\xFFFD] | [\x10000-\xEFFFF]	 ;
	PN_CHARS_U	:	PN_CHARS_BASE | '_' ;
	PN_CHARS	:	PN_CHARS_U | '-' | [0-9] | '#x00B7' | [\x0300-\x036F] | [\x203F-\x2040]	;
	PN_PREFIX	:	PN_CHARS_BASE ((PN_CHARS | '.')* PN_CHARS)?	 ;
	PN_LOCAL	:	(PN_CHARS_U | ':' | [0-9] | PLX) ((PN_CHARS | '.' | ':' | PLX)* (PN_CHARS | ':' | PLX))?  ;
	PLX	:	PERCENT | PN_LOCAL_ESC	;
	PERCENT	:	'%' HEX HEX		;
	HEX	:	[0-9] | [A-F] | [a-f]		;
	PN_LOCAL_ESC	:	'\\' ('_' | '~' | '.' | '-' | '!' | '$' | '&' | '\'' | '(' | ')' | '*' | '+' | ',' | ';' | '=' | '/' | '?' | '#' | '@' | '%')	;
	LineComment
    :   '#' ~('\n'|'\r')* //NEWLINE
       ->skip
    ;
