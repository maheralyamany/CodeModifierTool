public class CodeSyntaxModel {
	public CodeSyntaxModel(string name, SyntaxNodeType nodeType) {
		Name = name;
		NodeType = nodeType;
	}

	public CodeSyntaxModel(string name, SyntaxNodeType nodeType, bool hasAttribute) : this(name, nodeType) {
		HasAttribute = hasAttribute;
	}

	public CodeSyntaxModel(string name, SyntaxNodeType nodeType, bool hasAttribute, bool isTopLevel) : this(name, nodeType, hasAttribute) {
		IsTopLevel = isTopLevel;
	}

	public string Name { get; set; }
	public string Text { get; set; }
	public string Modifier { get; set; }
	public string ReturnType { get; set; }
	public string FullString { get; set; }
	public SyntaxNodeType NodeType { get; set; }
	public bool HasAttribute { get; set; }
	public bool IsTopLevel { get; set; } = true;
	public string FullDeclaration => $"{Modifier}{ReturnType}{Name}";

}
