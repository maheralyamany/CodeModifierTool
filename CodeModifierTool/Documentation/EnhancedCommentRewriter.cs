using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace CodeModifierTool {
	public class EnhancedCommentRewriter : BaseCodeSyntaxRewriter {
		private ProcessingOptions options;
		private SmartCommentGenerator _commentGenerator;
		public EnhancedCommentRewriter(SemanticModel semanticModel, ProcessingOptions options = null) : base(semanticModel) {
			this.SetOptions(options);

		}



		public SmartCommentGenerator GetCommentGenerator() {
			if (_commentGenerator == null)
				_commentGenerator = new SmartCommentGenerator(GetSemanticModel(), GetOptions().GenerateSmartComments);
			return _commentGenerator;
		}
		public ProcessingOptions GetOptions() {
			if (options == null)
				options = new ProcessingOptions();
			return options;
		}


		public EnhancedCommentRewriter SetOptions(ProcessingOptions value) {
			if (value == null)
				value = new ProcessingOptions();
			if (!options.Equals(value)) {
				options = value;
				_commentGenerator = null;
			}

			return this;
		}
		public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
			if (!GetOptions().AddMethodSummaries || !IsTopLevelMember(node) || (GetOptions().SkipExistingDocumentation && HasDocumentation(node)))
				return base.VisitMethodDeclaration(node);



			// Generate full XML documentation
			var xmlComment = GenerateMethodXmlComment(node);

			var summaryTrivia = SyntaxFactory.ParseLeadingTrivia(xmlComment);

			return node.WithLeadingTrivia(summaryTrivia);
		}

		public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node) {
			if (!GetOptions().AddClassSummaries)
				return base.VisitClassDeclaration(node);

			if (GetOptions().SkipExistingDocumentation && HasDocumentation(node))
				return base.VisitClassDeclaration(node);

			string summaryText = $"Represents: {node.Identifier.Text}";
			return AddSummaryComment(node, summaryText);
		}
		public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node) {
			if (GetOptions().SkipExistingDocumentation && HasDocumentation(node))
				return base.VisitStructDeclaration(node);
			return AddSummaryComment(node, $"Struct: {node.Identifier.Text}");
		}

		public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node) {
			if (GetOptions().SkipExistingDocumentation && HasDocumentation(node))
				return base.VisitEnumDeclaration(node);
			return AddSummaryComment(node, $"Enumeration: {node.Identifier.Text}");
		}
		public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node) {
			if (GetOptions().SkipExistingDocumentation && HasDocumentation(node))
				return base.VisitConstructorDeclaration(node);
			if (!(node.Parent is TypeDeclarationSyntax))
				return base.VisitConstructorDeclaration(node);

			//return base.VisitConstructorDeclaration(node);
			return AddSummaryComment(node, $"Initializes a new instance of the <see cref=\"{node.Identifier.Text}\"/> class.");
		}
		/*public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node) {
            try {
                if (node.Body == null && node.ExpressionBody == null)
                    return base.VisitAccessorDeclaration(node);
                if (!(node.Parent?.Parent is PropertyDeclarationSyntax property) || property == null)
                    return base.VisitAccessorDeclaration(node);
                if (!GetOptions().AddPropertySummaries)
                    return base.VisitAccessorDeclaration(node);
                if (GetOptions().SkipExistingDocumentation && HasDocumentation(property))
                    return base.VisitAccessorDeclaration(node);
                string accessType = property.AccessorList?.Accessors.Any(a => a.Keyword.IsKind(SyntaxKind.SetKeyword)) == true
                    ? "Gets or sets"
                    : "Gets";
                string summaryText = $"{accessType}: {property.Identifier.Text}";
                return AddSummaryComment(node, summaryText);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return base.VisitAccessorDeclaration(node);
        }*/
		public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
			if (!GetOptions().AddPropertySummaries)
				return base.VisitPropertyDeclaration(node);
			if (GetOptions().SkipExistingDocumentation && HasDocumentation(node))
				return base.VisitPropertyDeclaration(node);
			string accessType = node.AccessorList?.Accessors.Any(a => a.Keyword.IsKind(SyntaxKind.SetKeyword)) == true
				? "Gets or sets"
				: "Gets";
			string summaryText = $"{accessType}: {node.Identifier.Text}";
			return AddSummaryComment(node, summaryText);
		}



		private string GenerateMethodXmlComment(MethodDeclarationSyntax method) {
			var sb = new StringBuilder();

			/*// Generate summary section
            string methodName = method.Identifier.Text;
            string summaryText = GetOptions().MethodTemplate
                .Replace("{methodName}", methodName)
                .Replace("{className}", (method.Parent as ClassDeclarationSyntax)?.Identifier.Text ?? "");
            

            sb.AppendLine("/// <summary>");
            //sb.AppendLine($"/// {accessibility}");
            sb.AppendLine($"/// {summaryText}");
            sb.AppendLine("/// </summary>");



            // Generate parameter documentation
            if (GetOptions().AddParameterDocs) {
                foreach (var param in method.ParameterList.Parameters) {
                    string paramName = param.Identifier.Text;
                    string paramType = param.Type?.ToString() ?? "object";
                    string paramDesc = GetOptions().GenerateSmartComments
                        ? GenerateSmartParamDescription(paramName, paramType)
                        : GetOptions().ParameterTemplate
                            .Replace("{paramName}", paramName)
                            .Replace("{paramType}", paramType);

                    sb.AppendLine($"/// <param name=\"{paramName}\">{paramDesc}</param>");
                }
            }

            // Generate return documentation
            if (GetOptions().AddReturnDocs && !IsVoidMethod(method)) {
                string returnType = method.ReturnType.ToString();
                string returnDesc = GetOptions().GenerateSmartComments
                    ? GenerateSmartReturnDescription(methodName, returnType)
                    : GetOptions().ReturnTemplate
                        .Replace("{methodName}", methodName)
                        .Replace("{returnType}", returnType);

                sb.AppendLine($"/// <returns>{returnDesc}</returns>");
            }
*/
			return sb.ToString();
		}

		private string GenerateSmartParamDescription(string paramName, string paramType) {
			// AI-style smart descriptions based on parameter name and type
			string description = paramName.ToLower() switch
			{
				var s when s.Contains("id") => $"The unique identifier for the {paramName.Replace("Id", "")}",
				var s when s.Contains("name") => $"The name of the {paramName.Replace("Name", "")}",
				var s when s.Contains("count") => $"The number of {paramName.Replace("Count", "items")}",
				var s when s.Contains("date") || s.Contains("time") => $"The date/time value for the {paramName}",
				var s when s.Contains("is") || s.Contains("has") => $"Indicates whether {paramName.Replace("Is", "").Replace("Has", "")}",
				_ => $"The {paramName} value"
			};

			// Add type-specific information
			if (paramType.Contains("List") || paramType.Contains("Collection") || paramType.Contains("[]"))
				return $"{description} collection";
			if (paramType.Contains("Dictionary"))
				return $"{description} dictionary";

			return description;
		}

		private string GenerateSmartReturnDescription(string methodName, string returnType) {
			// AI-style smart descriptions based on method name and return type
			string description = methodName.ToLower() switch
			{
				var s when s.StartsWith("get") => $"The retrieved {returnType} value",
				var s when s.StartsWith("is") || s.StartsWith("has") => $"True if {methodName.Substring(2).ToLower()}, otherwise false",
				var s when s.StartsWith("create") => $"The newly created {returnType} instance",
				var s when s.StartsWith("find") || s.StartsWith("search") => $"The found {returnType} instance or null",
				var s when s.StartsWith("calculate") => $"The calculated {returnType} value",
				_ => $"The result of the {methodName} operation"
			};

			// Add type-specific information
			if (returnType.Contains("List") || returnType.Contains("Collection") || returnType.Contains("[]"))
				return $"A collection of {description.ToLower()}";
			if (returnType.Contains("Task"))
				return $"A task representing the asynchronous operation. {description}";

			return description;
		}



		private T AddSummaryComment<T>(T node, string summaryText) where T : MemberDeclarationSyntax {

			var summaryTrivia = SyntaxFactory.ParseLeadingTrivia(
			$"/// <summary>\n/// {summaryText}\n/// </summary>\n"
				);

			/*var summaryTrivia = SyntaxFactory.ParseLeadingTrivia(
                $"""
                /// <summary>
                /// {summaryText}
                /// </summary>
                """
            );*/

			return node.WithLeadingTrivia(
				node.GetLeadingTrivia()
					.InsertRange(0, summaryTrivia)
			);
		}
	}
}
