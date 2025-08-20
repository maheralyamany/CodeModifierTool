using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

public class NoInliningRewriter2 : CSharpSyntaxRewriter {
	public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
		if (!HasNoInliningAttribute(node))
			node = AddNoInliningAttribute(node);

		return base.VisitMethodDeclaration(node);
	}

	public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node) {
		if (!HasNoInliningAttribute(node))
			node = AddNoInliningAttribute(node);

		return base.VisitConstructorDeclaration(node);
	}

	public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node) {
		if (!HasNoInliningAttribute(node))
			node = AddNoInliningAttribute(node);

		return base.VisitAccessorDeclaration(node);
	}

	private bool HasNoInliningAttribute(MemberDeclarationSyntax node) {
		return node.AttributeLists
			.SelectMany(a => a.Attributes)
			.Any(attr => attr.Name.ToString().Contains("MethodImpl"));
	}

	private bool HasNoInliningAttribute(AccessorDeclarationSyntax node) {
		return node.AttributeLists
			.SelectMany(a => a.Attributes)
			.Any(attr => attr.Name.ToString().Contains("MethodImpl"));
	}

	private T AddNoInliningAttribute<T>(T node) where T : SyntaxNode {
		var attr = Attribute(ParseName("MethodImpl"))
			.WithArgumentList(
				AttributeArgumentList(
					SingletonSeparatedList(
						AttributeArgument(
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								IdentifierName("MethodImplOptions"),
								IdentifierName("NoInlining"))))));

		var attrList = AttributeList(SingletonSeparatedList(attr))
			.WithTrailingTrivia(ElasticCarriageReturnLineFeed);

		// Get trivia from node
		var leadingTrivia = node.GetLeadingTrivia();

		var (xmlTrivia, otherTrivia) = SplitTrivia(leadingTrivia);

		// Attach xml trivia to attribute
		attrList = attrList.WithLeadingTrivia(xmlTrivia);

		// Update node
		if (node is MemberDeclarationSyntax member) {
			return (T)(SyntaxNode)member
				.WithAttributeLists(member.AttributeLists.Insert(0, attrList))
				.WithLeadingTrivia(otherTrivia);
		} else if (node is AccessorDeclarationSyntax accessor) {
			return (T)(SyntaxNode)accessor
				.WithAttributeLists(accessor.AttributeLists.Insert(0, attrList))
				.WithLeadingTrivia(otherTrivia);
		}

		return node;
	}

	private (SyntaxTriviaList xmlTrivia, SyntaxTriviaList otherTrivia) SplitTrivia(SyntaxTriviaList triviaList) {
		var xmlTrivia = new SyntaxTriviaList();
		var otherTrivia = new SyntaxTriviaList();
		bool inXmlBlock = false;
		foreach (var trivia in triviaList) {
			if (trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
				trivia.IsKind(SyntaxKind.DocumentationCommentExteriorTrivia) ||
				(inXmlBlock && trivia.IsKind(SyntaxKind.EndOfLineTrivia))) {
				xmlTrivia = xmlTrivia.Add(trivia);
				inXmlBlock = true;
			} else {
				otherTrivia = otherTrivia.Add(trivia);
				inXmlBlock = false;
			}
		}
		return (xmlTrivia, otherTrivia);
	}
}
