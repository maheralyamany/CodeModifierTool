using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SmartCommentGenerator2 {
	private readonly SemanticModel _semanticModel;
	private readonly bool _useAdvancedAnalysis;

	public SmartCommentGenerator2(SemanticModel semanticModel, bool useAdvancedAnalysis = true) {
		_semanticModel = semanticModel;
		_useAdvancedAnalysis = useAdvancedAnalysis;
	}

	public DocumentationCommentTriviaSyntax GenerateDocumentation(MemberDeclarationSyntax member) {
		if (member is MethodDeclarationSyntax method)
			return GenerateMethodDocumentation(method);
		if (member is PropertyDeclarationSyntax property)
			return GeneratePropertyDocumentation(property);
		if (member is ClassDeclarationSyntax @class)
			return GenerateClassDocumentation(@class);

		return GenerateDefaultDocumentation(member);
	}

	private DocumentationCommentTriviaSyntax GenerateMethodDocumentation(MethodDeclarationSyntax method) {
		var summary = GenerateMethodSummary(method);
		var parameters = GenerateParameterComments(method).ToList();
		var returns = GenerateReturnComment(method);
		var exceptions = GenerateExceptionComments(method).ToList();

		var xmlNodes = new List<XmlNodeSyntax>
		{
			CreateSummaryElement(summary)
		};

		xmlNodes.AddRange(parameters);
		if (returns != null) xmlNodes.Add(returns);
		xmlNodes.AddRange(exceptions);

		return SyntaxFactory.DocumentationCommentTrivia(
			SyntaxKind.SingleLineDocumentationCommentTrivia,
			SyntaxFactory.List(xmlNodes)
		);
	}

	private string GenerateMethodSummary(MethodDeclarationSyntax method) {
		var methodSymbol = _semanticModel.GetDeclaredSymbol(method);
		var methodName = method.Identifier.Text;
		var containingType = methodSymbol?.ContainingType?.Name ?? "object";

		if (!_useAdvancedAnalysis)
			return $"Performs the {methodName} operation";

		// Traditional switch statement for .NET Framework compatibility
		string action;
		if (methodName.StartsWith("get", System.StringComparison.OrdinalIgnoreCase))
			action = "Gets";
		else if (methodName.StartsWith("set", System.StringComparison.OrdinalIgnoreCase))
			action = "Sets";
		else if (methodName.StartsWith("is", System.StringComparison.OrdinalIgnoreCase) ||
				 methodName.StartsWith("has", System.StringComparison.OrdinalIgnoreCase))
			action = "Determines whether";
		else if (methodName.StartsWith("create", System.StringComparison.OrdinalIgnoreCase))
			action = "Creates a new";
		else if (methodName.StartsWith("delete", System.StringComparison.OrdinalIgnoreCase))
			action = "Deletes";
		else if (methodName.StartsWith("update", System.StringComparison.OrdinalIgnoreCase))
			action = "Updates";
		else if (methodName.StartsWith("find", System.StringComparison.OrdinalIgnoreCase) ||
				 methodName.StartsWith("search", System.StringComparison.OrdinalIgnoreCase))
			action = "Finds";
		else if (methodName.StartsWith("calculate", System.StringComparison.OrdinalIgnoreCase))
			action = "Calculates";
		else if (methodName.StartsWith("validate", System.StringComparison.OrdinalIgnoreCase))
			action = "Validates";
		else
			action = "Performs";

		string target;
		if (methodName.StartsWith("get", System.StringComparison.OrdinalIgnoreCase) ||
			methodName.StartsWith("set", System.StringComparison.OrdinalIgnoreCase))
			target = SplitCamelCase(methodName.Substring(3));
		else if (methodName.StartsWith("is", System.StringComparison.OrdinalIgnoreCase))
			target = SplitCamelCase(methodName.Substring(2));
		else if (methodName.StartsWith("has", System.StringComparison.OrdinalIgnoreCase))
			target = SplitCamelCase(methodName.Substring(3));
		else
			target = SplitCamelCase(methodName);

		return $"{action} {target}";
	}

	private IEnumerable<XmlNodeSyntax> GenerateParameterComments(MethodDeclarationSyntax method) {
		foreach (var parameter in method.ParameterList.Parameters) {
			var paramSymbol = _semanticModel.GetDeclaredSymbol(parameter);
			var paramName = parameter.Identifier.Text;
			var paramType = paramSymbol?.Type?.Name ?? "object";

			var description = _useAdvancedAnalysis
				? GenerateSmartParameterDescription(paramName, paramType, paramSymbol)
				: $"The {paramName} parameter";

			yield return SyntaxFactory.XmlParamElement(
				parameter.Identifier.Text,
				SyntaxFactory.List(new XmlNodeSyntax[]
				{
					SyntaxFactory.XmlText(
						SyntaxFactory.TokenList(
							SyntaxFactory.XmlTextLiteral(
								SyntaxFactory.TriviaList(),
								description,
								description,
								SyntaxFactory.TriviaList())))
				}));
		}
	}

	private string GenerateSmartParameterDescription(string paramName, string paramType, IParameterSymbol symbol) {
		string description;

		// Traditional if-else for .NET Framework compatibility
		if (paramName.EndsWith("id", System.StringComparison.OrdinalIgnoreCase))
			description = $"The ID of the {paramName.Substring(0, paramName.Length - 2)}";
		else if (paramName.EndsWith("name", System.StringComparison.OrdinalIgnoreCase))
			description = $"The name of the {paramName.Substring(0, paramName.Length - 4)}";
		else if (paramName.EndsWith("count", System.StringComparison.OrdinalIgnoreCase))
			description = $"The number of {paramName.Substring(0, paramName.Length - 5)} items";
		else if (paramName.EndsWith("date", System.StringComparison.OrdinalIgnoreCase) ||
				 paramName.EndsWith("time", System.StringComparison.OrdinalIgnoreCase))
			description = $"The date/time when {paramName.Substring(0, paramName.Length - 4)} occurs";
		else if (paramName.StartsWith("is", System.StringComparison.OrdinalIgnoreCase))
			description = $"Whether {ToLowerFirstChar(paramName.Substring(2))} exists/is true";
		else if (paramName.StartsWith("has", System.StringComparison.OrdinalIgnoreCase))
			description = $"Whether {ToLowerFirstChar(paramName.Substring(3))} exists";
		else if (paramName.StartsWith("enable", System.StringComparison.OrdinalIgnoreCase))
			description = $"Whether to enable {ToLowerFirstChar(paramName.Substring(6))}";
		else if (paramName.StartsWith("use", System.StringComparison.OrdinalIgnoreCase))
			description = $"Whether to use {ToLowerFirstChar(paramName.Substring(3))}";
		else
			description = $"The {ToLowerFirstChar(paramName)}";

		// Enhance with type information
		if (symbol?.Type is INamedTypeSymbol namedType) {
			if (namedType.TypeArguments.Length > 0) {
				description += $" of type {string.Join(", ", namedType.TypeArguments.Select(t => t.Name))}";
			} else if (IsCollectionType(namedType)) {
				description += " collection";
			} else if (IsDictionaryType(namedType)) {
				description += " dictionary";
			}
		}

		// Add nullability information
		if (symbol?.NullableAnnotation == NullableAnnotation.Annotated) {
			description += " (optional)";
		}

		return description;
	}

	private XmlNodeSyntax GenerateReturnComment(MethodDeclarationSyntax method) {
		if (method.ReturnType is PredefinedTypeSyntax predefinedType &&
			predefinedType.Keyword.IsKind(SyntaxKind.VoidKeyword)) {
			return null;
		}

		var returnSymbol = _semanticModel.GetDeclaredSymbol(method)?.ReturnType;
		var methodName = method.Identifier.Text;

		var description = _useAdvancedAnalysis
			? GenerateSmartReturnDescription(methodName, returnSymbol)
			: $"The result of the {methodName} operation";

		return SyntaxFactory.XmlReturnsElement(
			SyntaxFactory.List(new XmlNodeSyntax[]
			{
				SyntaxFactory.XmlText(
					SyntaxFactory.TokenList(
						SyntaxFactory.XmlTextLiteral(
							SyntaxFactory.TriviaList(),
							description,
							description,
							SyntaxFactory.TriviaList())))
			}));
	}

	private string GenerateSmartReturnDescription(string methodName, ITypeSymbol returnSymbol) {
		var returnTypeName = returnSymbol?.Name ?? "object";
		var isCollection = IsCollectionType(returnSymbol);
		var isTask = returnSymbol?.Name == "Task";
		var isNullable = returnSymbol?.NullableAnnotation == NullableAnnotation.Annotated;

		string action;
		if (methodName.StartsWith("get", System.StringComparison.OrdinalIgnoreCase))
			action = "The retrieved";
		else if (methodName.StartsWith("is", System.StringComparison.OrdinalIgnoreCase) ||
				 methodName.StartsWith("has", System.StringComparison.OrdinalIgnoreCase))
			action = "Whether";
		else if (methodName.StartsWith("create", System.StringComparison.OrdinalIgnoreCase))
			action = "The newly created";
		else if (methodName.StartsWith("find", System.StringComparison.OrdinalIgnoreCase) ||
				 methodName.StartsWith("search", System.StringComparison.OrdinalIgnoreCase))
			action = "The found";
		else if (methodName.StartsWith("calculate", System.StringComparison.OrdinalIgnoreCase))
			action = "The calculated";
		else if (methodName.StartsWith("validate", System.StringComparison.OrdinalIgnoreCase))
			action = "Whether the validation";
		else
			action = "The result of the";

		string target;
		if (methodName.StartsWith("get", System.StringComparison.OrdinalIgnoreCase))
			target = SplitCamelCase(methodName.Substring(3));
		else if (methodName.StartsWith("is", System.StringComparison.OrdinalIgnoreCase))
			target = SplitCamelCase(methodName.Substring(2));
		else if (methodName.StartsWith("has", System.StringComparison.OrdinalIgnoreCase))
			target = SplitCamelCase(methodName.Substring(3));
		else
			target = SplitCamelCase(methodName);

		var description = $"{action} {target}";

		if (isCollection) {
			description += " collection";
		} else if (isTask) {
			description = $"A task representing the asynchronous operation. {description}";
		}

		if (isNullable) {
			description += " (or null if not found)";
		}

		return description;
	}

	private IEnumerable<XmlNodeSyntax> GenerateExceptionComments(MethodDeclarationSyntax method) {
		if (!_useAdvancedAnalysis)
			yield break;

		foreach (var param in method.ParameterList.Parameters) {
			var paramSymbol = _semanticModel.GetDeclaredSymbol(param);
			if (paramSymbol == null) continue;

			if (paramSymbol.Type.NullableAnnotation == NullableAnnotation.NotAnnotated) {
				yield return CreateExceptionElement(
					"System.ArgumentNullException",
					$"Thrown when {param.Identifier.Text} is null");
			}
		}

		if (method.ParameterList.Parameters.Any(p =>
			p.Type?.ToString().IndexOf("IEnumerable", System.StringComparison.Ordinal) >= 0)) {
			yield return CreateExceptionElement(
				"System.ArgumentException",
				"Thrown when the input collection is empty");
		}
	}
	private XmlNodeSyntax CreateExceptionElement(string exceptionType, string description) {
		var xmlText = $"/// <exception cref=\"{exceptionType}\">{description}</exception>";
		return SyntaxFactory.ParseLeadingTrivia(xmlText)
			.First()
			.GetStructure()
			.ChildNodes()
			.First() as XmlNodeSyntax;
	}

	/*private XmlNodeSyntax CreateExceptionElement(string exceptionType, string description) {
        return SyntaxFactory.XmlExceptionElement(
            SyntaxFactory.XmlName("cref"),
            SyntaxFactory.ParseName(exceptionType),
            null,
            SyntaxFactory.List(new XmlNodeSyntax[]
            {
                SyntaxFactory.XmlText(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.XmlTextLiteral(
                            SyntaxFactory.TriviaList(),
                            description,
                            description,
                            SyntaxFactory.TriviaList())))
            }));
    }*/
	private XmlElementSyntax CreateExceptionElement2(string exceptionType, string description) {
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
	private XmlNodeSyntax CreateSummaryElement(string summaryText) {
		return SyntaxFactory.XmlSummaryElement(
			SyntaxFactory.List(new XmlNodeSyntax[]
			{
				SyntaxFactory.XmlText(
					SyntaxFactory.TokenList(
						SyntaxFactory.XmlTextLiteral(
							SyntaxFactory.TriviaList(),
							summaryText,
							summaryText,
							SyntaxFactory.TriviaList())))
			}));
	}

	private DocumentationCommentTriviaSyntax GeneratePropertyDocumentation(PropertyDeclarationSyntax property) {
		var accessType = property.AccessorList?.Accessors.Any(a => a.Keyword.IsKind(SyntaxKind.SetKeyword)) == true
			? "Gets or sets"
			: "Gets";

		var summary = $"{accessType}: {property.Identifier.Text}";
		return SyntaxFactory.DocumentationCommentTrivia(
			SyntaxKind.SingleLineDocumentationCommentTrivia,
			SyntaxFactory.List(new XmlNodeSyntax[]
			{
				CreateSummaryElement(summary)
			})
		);
	}

	private DocumentationCommentTriviaSyntax GenerateClassDocumentation(ClassDeclarationSyntax @class) {
		var summary = $"Represents: {@class.Identifier.Text}";
		return SyntaxFactory.DocumentationCommentTrivia(
			SyntaxKind.SingleLineDocumentationCommentTrivia,
			SyntaxFactory.List(new XmlNodeSyntax[]
			{
				CreateSummaryElement(summary)
			})
		);
	}

	private DocumentationCommentTriviaSyntax GenerateDefaultDocumentation(MemberDeclarationSyntax member) {
		var summary = $"Documentation for {member.Kind()}";
		return SyntaxFactory.DocumentationCommentTrivia(
			SyntaxKind.SingleLineDocumentationCommentTrivia,
			SyntaxFactory.List(new XmlNodeSyntax[]
			{
				CreateSummaryElement(summary)
			})
		);
	}

	// Helper methods
	private static string SplitCamelCase(string input) {
		if (string.IsNullOrEmpty(input))
			return input;

		var result = new StringBuilder();
		result.Append(char.ToLower(input[0]));

		for (int i = 1; i < input.Length; i++) {
			if (char.IsUpper(input[i])) {
				result.Append(' ');
				result.Append(char.ToLower(input[i]));
			} else {
				result.Append(input[i]);
			}
		}

		return result.ToString();
	}

	private static string ToLowerFirstChar(string input) {
		if (string.IsNullOrEmpty(input))
			return input;

		return char.ToLower(input[0]) + input.Substring(1);
	}

	private static bool IsCollectionType(ITypeSymbol typeSymbol) {
		return typeSymbol?.AllInterfaces.Any(i =>
			i.Name == "IEnumerable" || i.Name == "ICollection") ?? false;
	}

	private static bool IsDictionaryType(ITypeSymbol typeSymbol) {
		return typeSymbol?.AllInterfaces.Any(i =>
			i.Name == "IDictionary") ?? false;
	}
}
