using System.Collections.Generic;
using System.Linq;
using System.Text;

using CodeModifierTool;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
public class AdvancedDocumentationRewriter : BaseCodeSyntaxRewriter {
	private ProcessingOptions options;
	private SmartCommentGenerator _commentGenerator;
	public AdvancedDocumentationRewriter(SemanticModel semanticModel, ProcessingOptions options = null) : base(semanticModel) {
		this.SetOptions(options);
	}
	public SmartCommentGenerator GetCommentGenerator() {
		if (_commentGenerator == null) {
			_commentGenerator = new SmartCommentGenerator(GetSemanticModel(), GetOptions().GenerateSmartComments);

		}
		return _commentGenerator;
	}
	public ProcessingOptions GetOptions() {
		if (options == null)
			options = new ProcessingOptions();
		return options;
	}

	public AdvancedDocumentationRewriter SetOptions(ProcessingOptions value) {
		if (value == null)
			value = new ProcessingOptions();
		if (options == null || (options != null && !options.Equals(value))) {
			options = value;
			_commentGenerator = null;
		}
		return this;
	}
	public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
		var visitedNode = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
		if (ShouldSkip(visitedNode) || !GetOptions().AddMethodSummaries || !IsTopLevelMember(node)) return visitedNode;
		return GenerateDocumentation(visitedNode);
	}
	public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node) {

		var visitedNode = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
		if (ShouldSkip(visitedNode) || !GetOptions().AddClassSummaries) return visitedNode;
		return GenerateDocumentation(visitedNode);
	}
	public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node) {
		var visitedNode = (StructDeclarationSyntax)base.VisitStructDeclaration(node);
		if (ShouldSkip(visitedNode) || !GetOptions().AddStructSummaries) return visitedNode;
		return GenerateDocumentation(visitedNode);
	}
	public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node) {
		var visitedNode = (EnumDeclarationSyntax)base.VisitEnumDeclaration(node);

		if (ShouldSkip(node) || !GetOptions().AddEnumSummaries) return visitedNode;
		return GenerateDocumentation(visitedNode);
	}
	public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node) {
		var visitedNode = (ConstructorDeclarationSyntax)base.VisitConstructorDeclaration(node);
		if (ShouldSkip(visitedNode) || !GetOptions().AddConstructorSummaries) return visitedNode;
		return GenerateDocumentation(visitedNode);
	}
	public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
		var visitedNode = (PropertyDeclarationSyntax)base.VisitPropertyDeclaration(node);
		if (ShouldSkip(visitedNode) || !GetOptions().AddPropertySummaries) return visitedNode;
		return GenerateDocumentation(visitedNode);
	}
	public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node) {
		var visitedNode = (InterfaceDeclarationSyntax)base.VisitInterfaceDeclaration(node);
		if (ShouldSkip(node) || !GetOptions().AddInterfaceSummaries) return visitedNode;
		return GenerateDocumentation(visitedNode);

	}

	public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node) {

		var visitedNode = (DelegateDeclarationSyntax)base.VisitDelegateDeclaration(node);
		if (ShouldSkip(node) || !GetOptions().AddDelegateSummaries) return visitedNode;
		return GenerateDocumentation(visitedNode);



	}
	public override SyntaxNode VisitEventDeclaration(EventDeclarationSyntax node) {

		var visitedNode = (EventDeclarationSyntax)base.VisitEventDeclaration(node);
		if (ShouldSkip(node) || !GetOptions().AddEventSummaries) return visitedNode;
		return GenerateDocumentation(visitedNode);



	}
	public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node) {
		var visitedNode = (FieldDeclarationSyntax)base.VisitFieldDeclaration(node);
		if (!GetOptions().AddFieldSummaries) {
			var existingTrivia = visitedNode.GetLeadingTrivia().ToList();
			if (existingTrivia.Count > 0) {
				var nonDocTrivia = existingTrivia
					.Where(t => !t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) &&
							   !t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
					.ToList();
				if (nonDocTrivia.Count > 0)
					return WithLeadingTrivia(node, nonDocTrivia);
			}

			return visitedNode.WithoutLeadingTrivia();
		} else if (ShouldSkip(node))
			return visitedNode;
		return GenerateDocumentation(visitedNode);
	}
	private bool ShouldSkip(MemberDeclarationSyntax node) {
		// Skip if already has documentation and we're configured to skip
		if (GetOptions().SkipExistingDocumentation && HasDocumentation(node))
			return true;
		// Skip generated files if configured
		if (GetOptions().SkipGeneratedFiles && IsGeneratedCode(node))
			return true;
		return false;
	}
	private T GenerateDocumentation<T>(T node) where T : MemberDeclarationSyntax {
		var documentation = GetCommentGenerator().GenerateDocumentation(node);
		if (documentation != null && documentation.Content.Count > 0)
			return node.WithLeadingTrivia(GetDocumentationTrivia(documentation, node))
					  .WithAdditionalAnnotations(Formatter.Annotation);
		return node;
	}
	private T WithLeadingTrivia<T>(T node, SyntaxTriviaList syntaxTrivias) where T : MemberDeclarationSyntax {
		return node.WithLeadingTrivia(syntaxTrivias)
				  .WithAdditionalAnnotations(Formatter.Annotation);
	}
	private T WithLeadingTrivia<T>(T node, List<SyntaxTrivia> nonDocTrivia) where T : MemberDeclarationSyntax {
		return WithLeadingTrivia(node, SyntaxFactory.TriviaList(nonDocTrivia));
	}
	private SyntaxTriviaList GetDocumentationTrivia(
		DocumentationCommentTriviaSyntax documentation,
		MemberDeclarationSyntax node) {
		var sb = new StringBuilder();
		foreach (var xmlNode in documentation.Content) {
			var xml = xmlNode.ToFullString();
			var trimdXml = xml.Trim();
			if (!trimdXml.StartsWith("///")) {
				/*
                    if (trimdXml.StartsWith("<summary>") && trimdXml.EndsWith("</summary>")) {
                        xml = xml.ReplaceAll(("<summary>", ""), ("</summary>", ""));
                        sb.AppendLine("/// <summary>");
                        sb.AppendLine($"/// {xml}");
                        sb.AppendLine("/// </summary>");
                    } else {
                        sb.AppendLine($"/// {xml}");
                    }*/
				sb.AppendLine($"/// {xml}");
			} else
				sb.AppendLine(xml);
		}
		var xmlComment = sb.ToString();
		var documentationTrivia = SyntaxFactory.ParseLeadingTrivia(xmlComment).ToList();
		var existingTrivia = node.GetLeadingTrivia();

		//var documentationTrivia = SyntaxFactory.Trivia(documentation);
		// Preserve any non-documentation leading trivia (like whitespace)
		var nonDocTrivia = existingTrivia
			.Where(t => !t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) &&
					   !t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
			.ToList();
		// Add new documentation and preserve existing formatting
		return SyntaxFactory.TriviaList(nonDocTrivia)
			.AddRange(documentationTrivia)
			.Add(SyntaxFactory.CarriageReturnLineFeed);
	}
}
