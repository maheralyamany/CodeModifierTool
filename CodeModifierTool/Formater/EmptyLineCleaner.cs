using Microsoft.CodeAnalysis.CSharp;

public class EmptyLineCleaner {
	public string RemoveExcessiveEmptyLines(string code) {
		// Parse the code
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var root = syntaxTree.GetRoot();

		// Create and apply the rewriter
		var remover = new EmptyLineRemover(syntaxTree);
		var newRoot = remover.Visit(root);

		return newRoot.ToFullString();
	}
}
