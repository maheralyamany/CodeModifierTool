using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;

public class SolutionProcessorWithNoInlining {
	private readonly SmartCommentOptions _options;

	public SolutionProcessorWithNoInlining(SmartCommentOptions options = null) {
		_options = options ?? new SmartCommentOptions();
	}

	public async Task ProcessSolutionAsync(string solutionPath) {
		if (!File.Exists(solutionPath))
			throw new FileNotFoundException("Solution not found", solutionPath);

		using var workspace = MSBuildWorkspace.Create();
		var solution = await workspace.OpenSolutionAsync(solutionPath);

		foreach (var project in solution.Projects) {
			Console.WriteLine($"Processing project: {project.Name}");
			foreach (var document in project.Documents) {
				if (!document.FilePath.EndsWith(".cs")) continue;
				if (document.FilePath.EndsWith(".Designer.cs") ||
					document.FilePath.EndsWith(".g.cs") ||
					document.FilePath.EndsWith(".Generated.cs"))
					continue; // skip generated files

				var syntaxRoot = await document.GetSyntaxRootAsync();
				var semanticModel = await document.GetSemanticModelAsync();
				var generator = new AdvancedSmartCommentGenerator(semanticModel, _options);

				var newRoot = syntaxRoot.ReplaceNodes(
					syntaxRoot.DescendantNodes().OfType<MemberDeclarationSyntax>()
						.Where(n => IsTopLevelUserCode(n)),
					(oldNode, _) => {
						var nodeWithDocs = AddOrUpdateDocumentation(oldNode, generator);
						return AddNoInlining(nodeWithDocs);
					});

				var formattedRoot = Formatter.Format(newRoot, workspace);
				File.WriteAllText(document.FilePath, formattedRoot.ToFullString());
			}
		}
	}

	private bool IsTopLevelUserCode(MemberDeclarationSyntax member) {
		// Skip nested members
		if (member.Parent is TypeDeclarationSyntax parentType && !(member is TypeDeclarationSyntax))
			return false;

		// Optional: skip partial types
		// if (member is TypeDeclarationSyntax type && type.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
		//     return false;

		return true;
	}

	private MemberDeclarationSyntax AddOrUpdateDocumentation(MemberDeclarationSyntax member, AdvancedSmartCommentGenerator generator) {
		if (member.GetLeadingTrivia().Any(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)))
			return member;

		var docComment = generator.GenerateDocumentation(member);
		var trivia = SyntaxFactory.Trivia(docComment);
		return member.WithLeadingTrivia(SyntaxFactory.TriviaList(trivia).AddRange(member.GetLeadingTrivia()));
	}

	private MemberDeclarationSyntax AddNoInlining(MemberDeclarationSyntax member) {
		// Only apply to top-level methods, constructors, or properties
		if (member is MethodDeclarationSyntax method)
			return AddAttributeIfMissing(method);
		if (member is ConstructorDeclarationSyntax ctor)
			return AddAttributeIfMissing(ctor);
		if (member is PropertyDeclarationSyntax prop)
			return AddAttributeIfMissing(prop);

		return member;
	}

	private T AddAttributeIfMissing<T>(T member) where T : MemberDeclarationSyntax {
		var hasAttr = member.AttributeLists
			.SelectMany(a => a.Attributes)
			.Any(a => a.Name.ToString().Contains("MethodImpl"));

		if (hasAttr)
			return member;

		var attribute = SyntaxFactory.Attribute(
			SyntaxFactory.ParseName("MethodImpl"),
			SyntaxFactory.AttributeArgumentList(
				SyntaxFactory.SingletonSeparatedList(
					SyntaxFactory.AttributeArgument(
						SyntaxFactory.MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							SyntaxFactory.IdentifierName("MethodImplOptions"),
							SyntaxFactory.IdentifierName("NoInlining"))))));

		var attrList = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(attribute))
			.WithTrailingTrivia(SyntaxFactory.TriviaList(SyntaxFactory.LineFeed));

		return (T)member.AddAttributeLists(attrList);
	}
}
