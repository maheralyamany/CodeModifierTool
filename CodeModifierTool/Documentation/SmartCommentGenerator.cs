using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
public class SmartCommentGenerator {
	private readonly SemanticModel _semanticModel;
	private readonly bool _useAdvancedAnalysis;
	public SmartCommentGenerator(SemanticModel semanticModel, bool useAdvancedAnalysis = true) {
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
		if (member is StructDeclarationSyntax structDecl)
			return GenerateStructDocumentation(structDecl);
		if (member is EnumDeclarationSyntax enumDecl)
			return GenerateEnumDocumentation(enumDecl);
		if (member is ConstructorDeclarationSyntax ctor)
			return GenerateConstructorDocumentation(ctor);
		if (member is InterfaceDeclarationSyntax interfaceDecl)
			return GenerateInterfaceDocumentation(interfaceDecl);
		if (member is DelegateDeclarationSyntax delegateDecl)
			return GenerateDelegateDocumentation(delegateDecl);
		if (member is EventDeclarationSyntax eventDecl)
			return GenerateEventDocumentation(eventDecl);
		if (member is FieldDeclarationSyntax fieldDecl)
			return GenerateFieldDocumentation(fieldDecl);
		return GenerateDefaultDocumentation(member);
	}
	public DocumentationCommentTriviaSyntax GenerateMethodDocumentation(MethodDeclarationSyntax method) {
		var summary = GenerateMethodSummary(method);
		var parameters = GenerateParameterComments(method).ToList();
		var returns = GenerateReturnComment(method);
		var exceptions = GenerateExceptionComments(method).ToList();
		var xmlNodes = new List<XmlNodeSyntax> { CreateSummaryElement(summary) };
		xmlNodes.AddRange(parameters);
		if (returns != null) xmlNodes.Add(returns);
		xmlNodes.AddRange(exceptions);
		return CreateDocumentationComment(xmlNodes);
	}
	public string GenerateMethodSummary(MethodDeclarationSyntax method) {
		if (!_useAdvancedAnalysis)
			return $"Performs the {method.Identifier.Text} operation";
		var methodSymbol = _semanticModel.GetDeclaredSymbol(method);
		var methodName = method.Identifier.Text;
		var containingType = methodSymbol?.ContainingType?.Name ?? "object";
		string action;
		if (StartsWithIgnoreCase(methodName, "get"))
			action = "Gets";
		else if (StartsWithIgnoreCase(methodName, "set"))
			action = "Sets";
		else if (StartsWithIgnoreCase(methodName, "is") || StartsWithIgnoreCase(methodName, "has"))
			action = "Determines whether";
		else if (StartsWithIgnoreCase(methodName, "create"))
			action = "Creates a new";
		else if (StartsWithIgnoreCase(methodName, "delete"))
			action = "Deletes";
		else if (StartsWithIgnoreCase(methodName, "update"))
			action = "Updates";
		else if (StartsWithIgnoreCase(methodName, "find") || StartsWithIgnoreCase(methodName, "search"))
			action = "Finds";
		else if (StartsWithIgnoreCase(methodName, "calculate"))
			action = "Calculates";
		else if (StartsWithIgnoreCase(methodName, "validate"))
			action = "Validates";
		else
			action = "Performs";
		string target;
		if (StartsWithIgnoreCase(methodName, "get") || StartsWithIgnoreCase(methodName, "set"))
			target = SplitCamelCase(Substring(methodName, 3));
		else if (StartsWithIgnoreCase(methodName, "is"))
			target = SplitCamelCase(Substring(methodName, 2));
		else if (StartsWithIgnoreCase(methodName, "has"))
			target = SplitCamelCase(Substring(methodName, 3));
		else
			target = SplitCamelCase(methodName);
		return $"{action} {target}";
	}
	public IEnumerable<XmlNodeSyntax> GenerateParameterComments(ParameterListSyntax parameterList) {
		foreach (var parameter in parameterList.Parameters) {
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
	public IEnumerable<XmlNodeSyntax> GenerateParameterComments(BaseMethodDeclarationSyntax method) {
		return GenerateParameterComments(method.ParameterList);
	}
	public string GenerateSmartParameterDescription(string paramName, string paramType, IParameterSymbol symbol) {
		string description;
		if (EndsWithIgnoreCase(paramName, "id"))
			description = $"The ID of the {Substring(paramName, 0, paramName.Length - 2)}";
		else if (EndsWithIgnoreCase(paramName, "name"))
			description = $"The name of the {Substring(paramName, 0, paramName.Length - 4)}";
		else if (EndsWithIgnoreCase(paramName, "count"))
			description = $"The number of {Substring(paramName, 0, paramName.Length - 5)} items";
		else if (EndsWithIgnoreCase(paramName, "date") || EndsWithIgnoreCase(paramName, "time"))
			description = $"The date/time when {Substring(paramName, 0, paramName.Length - 4)} occurs";
		else if (StartsWithIgnoreCase(paramName, "is"))
			description = $"Whether {ToLowerFirstChar(Substring(paramName, 2))} exists/is true";
		else if (StartsWithIgnoreCase(paramName, "has"))
			description = $"Whether {ToLowerFirstChar(Substring(paramName, 3))} exists";
		else if (StartsWithIgnoreCase(paramName, "enable"))
			description = $"Whether to enable {ToLowerFirstChar(Substring(paramName, 6))}";
		else if (StartsWithIgnoreCase(paramName, "use"))
			description = $"Whether to use {ToLowerFirstChar(Substring(paramName, 3))}";
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
	public XmlNodeSyntax GenerateReturnComment(MethodDeclarationSyntax method) {
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
	public string GenerateSmartReturnDescription(string methodName, ITypeSymbol returnSymbol) {
		var returnTypeName = returnSymbol?.Name ?? "object";
		var isCollection = IsCollectionType(returnSymbol);
		var isTask = returnSymbol?.Name == "Task";
		var isNullable = returnSymbol?.NullableAnnotation == NullableAnnotation.Annotated;
		string action;
		if (StartsWithIgnoreCase(methodName, "get"))
			action = "The retrieved";
		else if (StartsWithIgnoreCase(methodName, "is") || StartsWithIgnoreCase(methodName, "has"))
			action = "Whether";
		else if (StartsWithIgnoreCase(methodName, "create"))
			action = "The newly created";
		else if (StartsWithIgnoreCase(methodName, "find") || StartsWithIgnoreCase(methodName, "search"))
			action = "The found";
		else if (StartsWithIgnoreCase(methodName, "calculate"))
			action = "The calculated";
		else if (StartsWithIgnoreCase(methodName, "validate"))
			action = "Whether the validation";
		else
			action = "The result of the";
		string target;
		if (StartsWithIgnoreCase(methodName, "get"))
			target = SplitCamelCase(Substring(methodName, 3));
		else if (StartsWithIgnoreCase(methodName, "is"))
			target = SplitCamelCase(Substring(methodName, 2));
		else if (StartsWithIgnoreCase(methodName, "has"))
			target = SplitCamelCase(Substring(methodName, 3));
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
	public IEnumerable<XmlNodeSyntax> GenerateExceptionComments(MethodDeclarationSyntax method) {
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
	public DocumentationCommentTriviaSyntax GeneratePropertyDocumentation(PropertyDeclarationSyntax property) {
		var accessType = property.AccessorList?.Accessors.Any(a => a.Keyword.IsKind(SyntaxKind.SetKeyword)) == true
			? "Gets or sets"
			: "Gets";

		var summary = $"{accessType}: {SplitCamelCase(property.Identifier.Text)}";
		return CreateSummaryOnlyDocumentation(summary);
	}
	public DocumentationCommentTriviaSyntax GenerateClassDocumentation(ClassDeclarationSyntax @class) {

		var summary = $"Represents: {SplitCamelCase(@class.Identifier.Text)}";
		return CreateSummaryOnlyDocumentation(summary);
	}
	public DocumentationCommentTriviaSyntax GenerateStructDocumentation(StructDeclarationSyntax structDecl) {

		var summary = $"Represents struct: {SplitCamelCase(structDecl.Identifier.Text)}";
		return CreateSummaryOnlyDocumentation(summary);
	}
	public DocumentationCommentTriviaSyntax GenerateEnumDocumentation(EnumDeclarationSyntax enumDecl) {

		var summary = $"Defines enumeration: { SplitCamelCase(enumDecl.Identifier.Text)}";
		return CreateSummaryOnlyDocumentation(summary);
	}
	public DocumentationCommentTriviaSyntax GenerateInterfaceDocumentation(InterfaceDeclarationSyntax interfaceDecl) {

		var summary = $"Defines interface: {SplitCamelCase(interfaceDecl.Identifier.Text)}";
		return CreateSummaryOnlyDocumentation(summary);
	}
	public DocumentationCommentTriviaSyntax GenerateDelegateDocumentation(DelegateDeclarationSyntax delegateDecl) {
		var target = SplitCamelCase(delegateDecl.Identifier.Text);
		var summary = $"Represents delegate: {target}";
		var parameters = GenerateParameterComments(delegateDecl.ParameterList).ToList();
		var xmlNodes = new List<XmlNodeSyntax> { CreateSummaryElement(summary) };
		xmlNodes.AddRange(parameters);
		return CreateDocumentationComment(xmlNodes);
	}
	public DocumentationCommentTriviaSyntax GenerateEventDocumentation(EventDeclarationSyntax eventDecl) {
		var summary = $"Represents event: {SplitCamelCase(eventDecl.Identifier.Text)}";
		return CreateSummaryOnlyDocumentation(summary);
	}
	public DocumentationCommentTriviaSyntax GenerateFieldDocumentation(FieldDeclarationSyntax fieldDecl) {
		var variable = fieldDecl.Declaration.Variables.FirstOrDefault();
		var fieldName = variable?.Identifier.Text ?? "";
		var summary = $"The {SplitCamelCase(fieldName)} field";
		return CreateSummaryOnlyDocumentation(summary);
	}
	public DocumentationCommentTriviaSyntax GenerateConstructorDocumentation(ConstructorDeclarationSyntax ctor) {
		var parameters = GenerateParameterComments(ctor).ToList();


		//Initializes a new instance of the <see cref="FileExplorerRootPathEditor"/> class.
		var summary = $"Initializes a new instance of the <see cref=\"{ GetContainingTypeName(ctor)}\"/>  class";
		var xmlNodes = new List<XmlNodeSyntax> { CreateSummaryElement(summary) };
		xmlNodes.AddRange(parameters);
		return CreateDocumentationComment(xmlNodes);
	}
	public DocumentationCommentTriviaSyntax GenerateDefaultDocumentation(MemberDeclarationSyntax member) {
		var summary = $"Documentation for {member.Kind()}";
		return CreateSummaryOnlyDocumentation(summary);
	}
	public DocumentationCommentTriviaSyntax CreateSummaryOnlyDocumentation(string summaryText) {
		return CreateDocumentationComment(new List<XmlNodeSyntax> { CreateSummaryElement(summaryText) });
	}
	public DocumentationCommentTriviaSyntax CreateDocumentationComment(List<XmlNodeSyntax> xmlNodes) {
		return SyntaxFactory.DocumentationCommentTrivia(
			SyntaxKind.SingleLineDocumentationCommentTrivia,
			SyntaxFactory.List(xmlNodes));
	}
	public XmlNodeSyntax CreateSummaryElement(string summaryText) {
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
	#region Helper Methods
	public string GetContainingTypeName(BaseMethodDeclarationSyntax method) {
		return (method.Parent as TypeDeclarationSyntax)?.Identifier.Text ?? "object";
	}
	public static bool StartsWithIgnoreCase(string str, string value) {
		return str?.StartsWith(value, System.StringComparison.OrdinalIgnoreCase) ?? false;
	}
	private static bool EndsWithIgnoreCase(string str, string value) {
		return str?.EndsWith(value, System.StringComparison.OrdinalIgnoreCase) ?? false;
	}
	private static string Substring(string str, int startIndex) {
		return str?.Substring(startIndex) ?? string.Empty;
	}
	private static string Substring(string str, int startIndex, int length) {
		if (str == null || startIndex > str.Length)
			return string.Empty;
		if (startIndex + length > str.Length)
			length = str.Length - startIndex;
		return str.Substring(startIndex, length);
	}
	private static string SplitCamelCase(string input) {


		return CodeFormatter.SplitCamelCase(input);
	}
	private static string ToLowerFirstChar(string input) {
		if (string.IsNullOrEmpty(input))
			return input;
		return char.ToLower(input[0]) + input.Substring(1);
	}
	private static bool IsCollectionType(ITypeSymbol typeSymbol) {
		if (typeSymbol == null)
			return false;
		var SpecialType = typeSymbol.SpecialType;
		var isCollection = (SpecialType == SpecialType.System_Collections_Generic_ICollection_T || SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T || SpecialType == SpecialType.System_Collections_Generic_IEnumerator_T || SpecialType == SpecialType.System_Collections_Generic_IList_T || SpecialType == SpecialType.System_Collections_IEnumerable || SpecialType == SpecialType.System_Collections_IEnumerator);
		//var isCollection = typeSymbol.AllInterfaces.Any(i =>i.Name == "IEnumerable" || i.Name == "ICollection");
		return isCollection;
	}
	private static bool IsDictionaryType(ITypeSymbol typeSymbol) {
		return typeSymbol?.AllInterfaces.Any(i =>
			i.Name == "IDictionary") ?? false;
	}
	#endregion
}
