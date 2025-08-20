
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
public class FormatCodeRewriter : BaseCodeSyntaxRewriter {


	public FormatCodeRewriter(SemanticModel semanticModel) : base(semanticModel) {

	}
	public override bool IsEquivalentTo(SyntaxNode root) {
		return false;
	}
	public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node) {
		return base.VisitClassDeclaration(node);
	}
}
