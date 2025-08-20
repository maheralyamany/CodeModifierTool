using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AdvancedEmptyLineRemover : CSharpSyntaxRewriter {
	private const int MaxAllowedEmptyLines = 3;
	private readonly SyntaxTree _syntaxTree;
	private readonly bool _preserveFileHeader;

	public AdvancedEmptyLineRemover(SyntaxTree syntaxTree, bool preserveFileHeader = true) {
		_syntaxTree = syntaxTree;
		_preserveFileHeader = preserveFileHeader;
	}

	public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node) {
		var text = _syntaxTree.GetText();
		var newText = RemoveExcessiveEmptyLines(text.ToString());

		// Parse the cleaned text and return new syntax tree
		return CSharpSyntaxTree.ParseText(newText).GetRoot();
	}

	private string RemoveExcessiveEmptyLines(string sourceCode) {
		var lines = sourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		var resultLines = new List<string>();
		var emptyLineCount = 0;
		var inFileHeader = true;

		for (int i = 0; i < lines.Length; i++) {
			var line = lines[i];
			var isWhitespaceOnly = string.IsNullOrWhiteSpace(line);
			var isComment = line.TrimStart().StartsWith("//") || line.TrimStart().StartsWith("/*");

			// Check if we're still in file header (preserve empty lines in header)
			if (_preserveFileHeader && inFileHeader) {
				if (isComment || isWhitespaceOnly) {
					resultLines.Add(line);
					emptyLineCount = isWhitespaceOnly ? emptyLineCount + 1 : 0;
					continue;
				} else {
					inFileHeader = false; // End of header
				}
			}

			if (isWhitespaceOnly) {
				emptyLineCount++;
				if (emptyLineCount <= MaxAllowedEmptyLines) {
					resultLines.Add(line);
				}
				// Else: skip this empty line
			} else {
				emptyLineCount = 0;
				resultLines.Add(line);
			}
		}

		return string.Join(Environment.NewLine, resultLines);
	}
}
