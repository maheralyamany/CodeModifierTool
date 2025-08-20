using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
public class MethodImplNoInliningRewriter : CSharpSyntaxRewriter {
	
	public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
		if (!IsTopLevelMember(node))
			return base.VisitMethodDeclaration(node);

		if (HasNoInlining(node.AttributeLists))
			return base.VisitMethodDeclaration(node);

		var newNode = node.WithAttributeLists(
			node.AttributeLists.Add(CreateNoInliningAttributeList())
		);

		return base.VisitMethodDeclaration(newNode);
	}
	

	public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node) {
		/*if (!IsTopLevelMember(node))
            return base.VisitConstructorDeclaration(node);*/

		if (HasNoInlining(node.AttributeLists))
			return base.VisitConstructorDeclaration(node);

		var newNode = node.WithAttributeLists(
			node.AttributeLists.Add(CreateNoInliningAttributeList())
		);

		return base.VisitConstructorDeclaration(newNode);
	}
	

	public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node) {
		// Skip auto-properties (no body)
		if (node.Body == null && node.ExpressionBody == null)
			return base.VisitAccessorDeclaration(node);

		if (!(node.Parent?.Parent is PropertyDeclarationSyntax))
			return base.VisitAccessorDeclaration(node);

		if (!IsTopLevelMember(node.Parent?.Parent))
			return base.VisitAccessorDeclaration(node);

		if (HasNoInlining(node.AttributeLists))
			return base.VisitAccessorDeclaration(node);

		var newNode = node.WithAttributeLists(
			node.AttributeLists.Add(CreateNoInliningAttributeList())
		);

		return base.VisitAccessorDeclaration(newNode);
	}
	

	private bool HasNoInlining(SyntaxList<AttributeListSyntax> attributeLists) {
		return attributeLists
			.SelectMany(list => list.Attributes)
			.Any(attr =>
				attr.Name.ToString().Contains("MethodImpl") &&
				attr.ArgumentList?.Arguments.ToString().Contains("NoInlining") == true
			);
	}
	

	private AttributeListSyntax CreateNoInliningAttributeList() {
		return AttributeList(SingletonSeparatedList(
			Attribute(IdentifierName("MethodImpl"))
			.WithArgumentList(AttributeArgumentList(SingletonSeparatedList(
				AttributeArgument(MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName("MethodImplOptions"),
					IdentifierName("NoInlining")
				))
			)))
		)).WithTrailingTrivia(ElasticCarriageReturnLineFeed);
	}
	

	private bool IsTopLevelMember(SyntaxNode node) {
		return node?.Parent is ClassDeclarationSyntax
			|| node?.Parent is StructDeclarationSyntax
			|| node?.Parent is RecordDeclarationSyntax;
	}
}
