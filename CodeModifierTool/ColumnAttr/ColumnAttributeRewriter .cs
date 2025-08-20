
using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
public class ColumnAttributeRewriter : BaseCodeSyntaxRewriter {

	public ColumnAttributeRewriter(SemanticModel semanticModel) : base(semanticModel) {
		this.SetUsingDirectives("System.ComponentModel.DataAnnotations.Schema");
	}


	public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
		// Check if we need to add the attribute
		if (!HasAttribute(node.AttributeLists, "Column")) {
			return AddColumnAttributeToProperty(node);
		}

		return base.VisitPropertyDeclaration(node);
	}
	/*public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node) {
        // First process the class to add using if needed
        var compilationUnit = node.FirstAncestorOrSelf<CompilationUnitSyntax>();
        if (compilationUnit != null && !_addedUsing) {
            if (!compilationUnit.Usings.Any(u => u.Name.ToString() == "System.ComponentModel.DataAnnotations.Schema")) {
                var usingDirective = UsingDirective(
                    ParseName("System.ComponentModel.DataAnnotations.Schema"));
                compilationUnit = compilationUnit.AddUsings(usingDirective);
                _addedUsing = true;
            }
        }
        // Process properties in this class
        //var properties = node.Members.OfType<PropertyDeclarationSyntax>().ToList();
        //var newMembers = node.Members;
        var newMembers = new List<MemberDeclarationSyntax>();
        foreach (var member in node.Members) {
            if (member is PropertyDeclarationSyntax property) {
                if (!PropertyHasColumnAttribute(property)) {
                    // Modify this property by adding the attribute
                    var newProperty = AddColumnAttributeToProperty(property);
                    newMembers.Add(newProperty);
                } else {
                    // Keep the property as is
                    newMembers.Add(property);
                }
            } else {
                // If it's not a property, keep it as is
                newMembers.Add(member);
            }
        }
        // Create a new class node with the updated members
        return node.WithMembers(List(newMembers));
    }*/

	private bool PropertyHasColumnAttribute(PropertyDeclarationSyntax property) {
		try {
			return property.AttributeLists
				.SelectMany(al => al.Attributes)
				.Any(attr =>
					GetSemanticModel().GetTypeInfo(attr.Name).Type?.ToString() == "System.ComponentModel.DataAnnotations.Schema.ColumnAttribute");
		} catch (System.Exception ex) {
			Console.WriteLine(ex.Message);
			return HasAttribute(property.AttributeLists, "Column");
		}
	}
	private PropertyDeclarationSyntax AddColumnAttributeToProperty(PropertyDeclarationSyntax property) {
		try {
			// Get column name (convert to snake_case if you want)
			//  string columnName = ConvertToSnakeCase(property.Identifier.ValueText);
			string columnName = property.Identifier.ValueText;

			// Create [Column("column_name")] attribute
			var attribute = Attribute(
				ParseName("Column"),
				AttributeArgumentList(
					SingletonSeparatedList(
						AttributeArgument(
							LiteralExpression(
								SyntaxKind.StringLiteralExpression,
								Literal(columnName))))));

			var attributeList = AttributeList(
				SingletonSeparatedList(attribute))
				.WithTrailingTrivia(ElasticCarriageReturnLineFeed);

			// Add attribute to the property
			return property.AddAttributeLists(attributeList);
		} catch (System.Exception ex) {
			Console.WriteLine(ex.Message);
		}
		return property;
	}

	private string ConvertToSnakeCase(string input) {
		// Simple snake_case conversion
		return string.Concat(input.Select((c, i) =>
			i > 0 && char.IsUpper(c) ? "_" + c.ToString() : c.ToString()))
			.ToLower();
	}




}
