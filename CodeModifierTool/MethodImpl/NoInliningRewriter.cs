
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
public class NoInliningRewriter : BaseCodeSyntaxRewriter {
	public NoInliningRewriter(SemanticModel semanticModel) : base(semanticModel) {

	}
	public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
		if (!IsTopLevelMember(node) || HasNoInlining(node.AttributeLists) || !IsTypeDeclaration(node))
			return base.VisitMethodDeclaration(node);
		var newNode = AddNoInliningAttribute(node);
		return base.VisitMethodDeclaration(newNode);
	}
	public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node) {
		if (HasNoInlining(node.AttributeLists) || !IsTypeDeclaration(node))
			return base.VisitConstructorDeclaration(node);
		var newNode = AddNoInliningAttribute(node);
		return base.VisitConstructorDeclaration(newNode);
	}
	public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node) {
		if (!HasBody(node) || HasNoInlining(node.AttributeLists) || !IsPropertyDeclaration(node, out PropertyDeclarationSyntax property) || !IsTopLevelMember(node.Parent?.Parent)) {
			return base.VisitAccessorDeclaration(node);
		}
		var newNode = AddNoInliningAttribute(node);
		return base.VisitAccessorDeclaration(newNode);
	}
	private bool HasNoInlining(SyntaxList<AttributeListSyntax> list) =>
		HasAttribute(list, "MethodImpl");
	private T AddNoInliningAttribute<T>(T node) where T : SyntaxNode {
		var attr = Attribute(ParseName("MethodImpl"))
			.WithArgumentList(
				AttributeArgumentList(SingletonSeparatedList(
					AttributeArgument(
						MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							IdentifierName("MethodImplOptions"),
							IdentifierName("NoInlining"))))));
		var attrList = AttributeList(SingletonSeparatedList(attr))
			.WithTrailingTrivia(ElasticCarriageReturnLineFeed);
		var leadingTrivia = node.GetLeadingTrivia();
		/*var (xmlTrivia, otherTrivia) = SplitTrivia(leadingTrivia);
        attrList = attrList.WithLeadingTrivia(xmlTrivia);*/
		var hasleadingTrivia = leadingTrivia.Count > 0;
		if (hasleadingTrivia)
			node = node.WithoutLeadingTrivia();
		if (node is MemberDeclarationSyntax member) {
			//var leadingTrivia = member.GetLeadingTrivia();
			var mm = member
				.WithAttributeLists(member.AttributeLists.Insert(0, attrList));
			if (hasleadingTrivia)
				mm = mm.WithLeadingTrivia(leadingTrivia);
			return (T)(SyntaxNode)mm;
		}
		if (node is AccessorDeclarationSyntax accessor) {
			var mm = accessor
				.WithAttributeLists(accessor.AttributeLists.Insert(0, attrList));
			if (hasleadingTrivia)
				mm = mm.WithLeadingTrivia(leadingTrivia);
			return (T)(SyntaxNode)mm;
			//return (T)(SyntaxNode)accessor.WithAttributeLists(accessor.AttributeLists.Insert(0, attrList)).WithLeadingTrivia(leadingTrivia);
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
