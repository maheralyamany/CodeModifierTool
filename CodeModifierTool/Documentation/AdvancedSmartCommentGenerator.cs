
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SmartCommentOptions {
	public bool IncludeExamples { get; set; } = true;
	public bool IncludeExceptions { get; set; } = true;
	public bool UseInheritDocForOverrides { get; set; } = true;
	public bool IncludeSeeAlso { get; set; } = true;
	public bool IncludeRemarks { get; set; } = true;
}

public class AdvancedSmartCommentGenerator {
	private readonly SemanticModel _semanticModel;
	private readonly SmartCommentOptions _options;

	public AdvancedSmartCommentGenerator(SemanticModel semanticModel, SmartCommentOptions options = null) {
		_semanticModel = semanticModel ?? throw new ArgumentNullException(nameof(semanticModel));
		_options = options ?? new SmartCommentOptions();
	}

	public DocumentationCommentTriviaSyntax GenerateDocumentation(MemberDeclarationSyntax member) {
		switch (member) {
			case MethodDeclarationSyntax method:
				return GenerateMethodDocumentation(method);
			case ConstructorDeclarationSyntax ctor:
				return GenerateConstructorDocumentation(ctor);
			case PropertyDeclarationSyntax property:
				return GeneratePropertyDocumentation(property);
			case FieldDeclarationSyntax field:
				return GenerateFieldDocumentation(field);
			case ClassDeclarationSyntax cls:
				return GenerateClassDocumentation(cls);
			case StructDeclarationSyntax str:
				return GenerateStructDocumentation(str);
			case EnumDeclarationSyntax en:
				return GenerateEnumDocumentation(en);
			case InterfaceDeclarationSyntax iface:
				return GenerateInterfaceDocumentation(iface);
			case EventDeclarationSyntax ev:
				return GenerateEventDocumentation(ev);
			case DelegateDeclarationSyntax del:
				return GenerateDelegateDocumentation(del);
			default:
				return CreateSummaryOnlyDocumentation($"Documentation for {member.Kind()}");
		}
	}

	#region Member Generators

	private DocumentationCommentTriviaSyntax GenerateMethodDocumentation(MethodDeclarationSyntax method) {
		var symbol = _semanticModel.GetDeclaredSymbol(method);
		if (_options.UseInheritDocForOverrides && symbol is IMethodSymbol m && (m.IsOverride || m.ExplicitInterfaceImplementations.Length > 0)) {
			return SyntaxFactory.DocumentationCommentTrivia(
				SyntaxKind.SingleLineDocumentationCommentTrivia,
				SyntaxFactory.List<XmlNodeSyntax>(new[] { SyntaxFactory.XmlEmptyElement("inheritdoc") }));
		}

		var nodes = new List<XmlNodeSyntax>
		{
			SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText($"Performs the {method.Identifier.Text} operation."))
		};

		// Parameters
		foreach (var param in method.ParameterList.Parameters) {
			nodes.Add(SyntaxFactory.XmlParamElement(param.Identifier.Text,
				SyntaxFactory.List(new XmlNodeSyntax[] { SyntaxFactory.XmlText($"The {param.Identifier.Text} parameter.") })));
		}

		// Return
		if (!method.ReturnType.IsKind(SyntaxKind.VoidKeyword)) {
			nodes.Add(SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText("The result of the operation.")));
		}

		// Exceptions placeholder
		if (_options.IncludeExceptions) {
			foreach (var param in method.ParameterList.Parameters) {
				nodes.Add(CreateExceptionElement("System.ArgumentNullException", $"Thrown when {param.Identifier.Text} is null."));
			}
		}

		// Remarks
		if (_options.IncludeRemarks) {
			nodes.Add(SyntaxFactory.XmlRemarksElement(SyntaxFactory.XmlText($"Additional remarks for {method.Identifier.Text}.")));
		}

		// Example
		if (_options.IncludeExamples) {
			nodes.Add(SyntaxFactory.XmlExampleElement(
				SyntaxFactory.List(new XmlNodeSyntax[]
				{
					SyntaxFactory.XmlText($"Example usage:\nvar result = {method.Identifier.Text}(...);")
				})));
		}

		// SeeAlso
		if (_options.IncludeSeeAlso) {
			var type = symbol?.ContainingType;
			if (type != null) {
				nodes.Add(SyntaxFactory.XmlSeeAlsoElement(
					SyntaxFactory.NameMemberCref(SyntaxFactory.ParseName(type.Name))));
			}
		}

		return SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia, SyntaxFactory.List(nodes));
	}

	private DocumentationCommentTriviaSyntax GenerateConstructorDocumentation(ConstructorDeclarationSyntax ctor) {
		var nodes = new List<XmlNodeSyntax>
		{
			SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText($"Initializes a new instance of the {ctor.Identifier.Text} class."))
		};

		foreach (var param in ctor.ParameterList.Parameters) {
			nodes.Add(SyntaxFactory.XmlParamElement(param.Identifier.Text,
				SyntaxFactory.List(new XmlNodeSyntax[] { SyntaxFactory.XmlText($"The {param.Identifier.Text} parameter.") })));
		}

		return SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia, SyntaxFactory.List(nodes));
	}

	private DocumentationCommentTriviaSyntax GeneratePropertyDocumentation(PropertyDeclarationSyntax property) {
		var access = property.AccessorList?.Accessors.Any(a => a.Keyword.IsKind(SyntaxKind.SetKeyword)) == true ? "Gets or sets" : "Gets";
		return CreateSummaryOnlyDocumentation($"{access} the {property.Identifier.Text}.");
	}

	private DocumentationCommentTriviaSyntax GenerateFieldDocumentation(FieldDeclarationSyntax field) {
		var name = field.Declaration.Variables.FirstOrDefault()?.Identifier.Text ?? "field";
		return CreateSummaryOnlyDocumentation($"The {name} field.");
	}

	private DocumentationCommentTriviaSyntax GenerateClassDocumentation(ClassDeclarationSyntax cls) {
		return CreateSummaryOnlyDocumentation($"Represents the {cls.Identifier.Text} class.");
	}

	private DocumentationCommentTriviaSyntax GenerateStructDocumentation(StructDeclarationSyntax str) {
		return CreateSummaryOnlyDocumentation($"Represents the {str.Identifier.Text} struct.");
	}

	private DocumentationCommentTriviaSyntax GenerateEnumDocumentation(EnumDeclarationSyntax en) {
		return CreateSummaryOnlyDocumentation($"Defines the {en.Identifier.Text} enumeration.");
	}

	private DocumentationCommentTriviaSyntax GenerateInterfaceDocumentation(InterfaceDeclarationSyntax iface) {
		return CreateSummaryOnlyDocumentation($"Defines the {iface.Identifier.Text} interface.");
	}

	private DocumentationCommentTriviaSyntax GenerateEventDocumentation(EventDeclarationSyntax ev) {
		return CreateSummaryOnlyDocumentation($"Represents the {ev.Identifier.Text} event.");
	}

	private DocumentationCommentTriviaSyntax GenerateDelegateDocumentation(DelegateDeclarationSyntax del) {
		var nodes = new List<XmlNodeSyntax>
		{
			SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText($"Represents the {del.Identifier.Text} delegate."))
		};

		foreach (var param in del.ParameterList.Parameters) {
			nodes.Add(SyntaxFactory.XmlParamElement(param.Identifier.Text,
				SyntaxFactory.List(new XmlNodeSyntax[] { SyntaxFactory.XmlText($"The {param.Identifier.Text} parameter.") })));
		}

		return SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia, SyntaxFactory.List(nodes));
	}

	#endregion

	#region Helpers

	private DocumentationCommentTriviaSyntax CreateSummaryOnlyDocumentation(string summary) {
		return SyntaxFactory.DocumentationCommentTrivia(
			SyntaxKind.SingleLineDocumentationCommentTrivia,
			SyntaxFactory.List(new XmlNodeSyntax[] { SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText(summary)) }));
	}

	public XmlNodeSyntax CreateExceptionElement(string exceptionType, string description) {
		// <exception cref="exceptionType">description</exception>
		return SyntaxFactory.XmlElement(
			SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName("exception"))
				.WithAttributes(SyntaxFactory.SingletonList<XmlAttributeSyntax>(
					SyntaxFactory.XmlCrefAttribute(SyntaxFactory.NameMemberCref(SyntaxFactory.ParseName(exceptionType))))),
			SyntaxFactory.SingletonList<XmlNodeSyntax>(
				SyntaxFactory.XmlText(description)),
			SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName("exception"))
		);
	}

	#endregion
}
