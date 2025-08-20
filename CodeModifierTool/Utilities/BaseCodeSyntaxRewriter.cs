using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
public class ComprehensiveVisitor<T> : CSharpSyntaxWalker where T : BaseCodeSyntaxRewriter {

	private T _rewriter;
	public ComprehensiveVisitor(T rewriter, SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node)
		: base(depth) {
		_rewriter = rewriter;
	}

	public override void Visit(SyntaxNode node) {
		// This will visit every node in the tree
		base.Visit(node);

		// Your processing logic
		if (_rewriter != null) {
			_rewriter.Visit(node);
		}
	}


}

public class BaseCodeSyntaxRewriter : CSharpSyntaxRewriter {
	private readonly List<MemberDeclarationSyntax> _declarations = new List<MemberDeclarationSyntax>();
	private readonly SemanticModel _semanticModel;
	private List<string> usingDirectives;
	private bool _addedUsing;


	public BaseCodeSyntaxRewriter(bool visitIntoStructuredTrivia = false) : base(visitIntoStructuredTrivia) {

	}

	public BaseCodeSyntaxRewriter(SemanticModel semanticModel, bool visitIntoStructuredTrivia = false) : this(visitIntoStructuredTrivia) {
		_semanticModel = semanticModel;
	}
	public IReadOnlyList<MemberDeclarationSyntax> AllDeclarations => _declarations;
	public SemanticModel GetSemanticModel() {
		return _semanticModel;
	}

	public virtual bool IsEquivalentTo(SyntaxNode root) {
		var IsEquivalent = this.newRoot.IsEquivalentTo(rootNode ?? root);
		return IsEquivalent;
	}

	protected SyntaxNode rootNode;
	protected SyntaxNode newRoot;
	public SyntaxNode VisitEntireTree(SyntaxNode root) {
		rootNode = root;
		/*if (node != null) {
            if (node is ClassDeclarationSyntax classDeclaration)
                return VisitClassDeclaration(classDeclaration);
            if (node is MethodDeclarationSyntax method)
                return VisitMethodDeclaration(method);
            if (node is PropertyDeclarationSyntax property)
                return VisitPropertyDeclaration(property);
            if (node is ClassDeclarationSyntax @class)
                return VisitClassDeclaration(@class);
            if (node is StructDeclarationSyntax structDecl)
                return VisitStructDeclaration(structDecl);
            if (node is EnumDeclarationSyntax enumDecl)
                return VisitEnumDeclaration(enumDecl);
            if (node is ConstructorDeclarationSyntax ctor)
                return VisitConstructorDeclaration(ctor);
            if (node is InterfaceDeclarationSyntax interfaceDecl)
                return VisitInterfaceDeclaration(interfaceDecl);
            if (node is DelegateDeclarationSyntax delegateDecl)
                return VisitDelegateDeclaration(delegateDecl);
            if (node is EventDeclarationSyntax eventDecl)
                return VisitEventDeclaration(eventDecl);
            if (node is FieldDeclarationSyntax fieldDecl)
                return VisitFieldDeclaration(fieldDecl);
        }*/
		this.newRoot = base.Visit(root);
		return this.newRoot;
	}
	public virtual SyntaxNode VisitSyntaxNode(SyntaxNode node) {
		return VisitEntireTree(node);
	}
	#region Type Declarations

	public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitClassDeclaration(node);
	}

	public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitStructDeclaration(node);
	}

	public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitInterfaceDeclaration(node);
	}

	public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitEnumDeclaration(node);
	}

	public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitDelegateDeclaration(node);
	}

	#endregion

	#region Member Declarations

	public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitMethodDeclaration(node);
	}

	public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitConstructorDeclaration(node);
	}

	public override SyntaxNode VisitDestructorDeclaration(DestructorDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitDestructorDeclaration(node);
	}

	public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitPropertyDeclaration(node);
	}

	public override SyntaxNode VisitEventDeclaration(EventDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitEventDeclaration(node);
	}

	public override SyntaxNode VisitEventFieldDeclaration(EventFieldDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitEventFieldDeclaration(node);
	}

	public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitFieldDeclaration(node);
	}

	public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitIndexerDeclaration(node);
	}

	public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitOperatorDeclaration(node);
	}

	public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitConversionOperatorDeclaration(node);
	}

	#endregion

	#region Other Declarations

	public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node) {
		_declarations.Add(node);
		return base.VisitNamespaceDeclaration(node);
	}

	public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node) {
		//_declarations.Add(node);
		return base.VisitUsingDirective(node);
	}

	public override SyntaxNode VisitAttributeList(AttributeListSyntax node) {
		// _declarations.Add(node);
		return base.VisitAttributeList(node);
	}

	public override SyntaxNode VisitParameter(ParameterSyntax node) {
		//_declarations.Add(node);
		return base.VisitParameter(node);
	}

	public override SyntaxNode VisitTypeParameter(TypeParameterSyntax node) {
		// _declarations.Add(node);
		return base.VisitTypeParameter(node);
	}
	public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node) {
		return base.VisitAccessorDeclaration(node);
	}
	#endregion

	#region Compilation Unit


	public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node) {
		GetUsingDirectives();

		if (!_addedUsing && usingDirectives.Count > 0) {
			_addedUsing = true;
			foreach (var item in usingDirectives) {
				if (!node.Usings.Any(u => u.Name.ToString() == item)) {
					node = node.AddUsings(
						SyntaxFactory.UsingDirective(
							SyntaxFactory.ParseName(item)));
				}

			}
		}
		// Visit all members including usings and attributes
		var visitedNode = base.VisitCompilationUnit(node);

		// You could process the entire compilation unit here if needed
		return visitedNode;
	}

	#endregion

	public List<string> GetUsingDirectives() {
		if (usingDirectives == null)
			usingDirectives = new List<string>();
		return usingDirectives;
	}

	public virtual BaseCodeSyntaxRewriter SetUsingDirectives(List<string> usings) {
		_addedUsing = false;
		usingDirectives = usings;
		return this;
	}
	public virtual BaseCodeSyntaxRewriter SetUsingDirectives(params string[] usings) {
		if (usings.IsNotValidParams())
			return this;
		return SetUsingDirectives(usings.ToList());
	}



	public bool IsTopLevelMember(SyntaxNode node) {
		return node?.Parent is ClassDeclarationSyntax
			|| node?.Parent is StructDeclarationSyntax
			|| node?.Parent is RecordDeclarationSyntax;
	}
	/*public bool HasDocumentation(MemberDeclarationSyntax node) {
        var hasdoc = node.HasStructuredTrivia ||
               node.GetLeadingTrivia().Any(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
        return hasdoc;
    }*/
	public bool HasDocumentation(MemberDeclarationSyntax node) {
		return node.GetLeadingTrivia().Any(t =>
			t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
			t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));
	}
	public bool IsGeneratedCode(MemberDeclarationSyntax node) {
		var filePath = node.SyntaxTree?.FilePath ?? "";
		return filePath.Contains(".g.cs") ||
			   filePath.Contains(".generated.cs") ||
			   filePath.Contains("TemporaryGeneratedFile");
	}
	public static IEnumerable<T> FindNodesOfType<T>(SyntaxNode root) where T : SyntaxNode {
		return root.DescendantNodesAndSelf().OfType<T>();
	}
	public IEnumerable<MethodDeclarationSyntax> GetAllMethods(SyntaxNode node) {
		var methods = node.DescendantNodes().OfType<MethodDeclarationSyntax>();
		return methods;
	}
	public bool HasBody(AccessorDeclarationSyntax node) => !(node.Body == null && node.ExpressionBody == null);
	public bool IsTypeDeclaration(SyntaxNode node) => ((node.Parent is TypeDeclarationSyntax));
	public bool IsPropertyDeclaration(AccessorDeclarationSyntax node, out PropertyDeclarationSyntax property) {
		if (!(node.Parent?.Parent is PropertyDeclarationSyntax prop) || prop == null) {
			property = null;
			return false;
		}
		property = prop;
		return true;
	}
	public bool HasAttribute(SyntaxList<AttributeListSyntax> list, string attrName) =>
	list.SelectMany(a => a.Attributes).Any(attr =>
		attr.Name.ToString().Contains(attrName) /*&&
            attr.ArgumentList?.Arguments.Any(arg => arg.ToString().Contains("NoInlining")) == true*/);

	public bool IsVoidMethod(MethodDeclarationSyntax method) {
		return method.ReturnType is PredefinedTypeSyntax predefinedType &&
			   predefinedType.Keyword.IsKind(SyntaxKind.VoidKeyword);
	}
}
