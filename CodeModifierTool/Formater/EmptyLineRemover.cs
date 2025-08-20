using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

public class EmptyLineRemover : CSharpSyntaxRewriter {
	private const int MaxAllowedEmptyLines = 3;
	private readonly SyntaxTree _syntaxTree;

	public EmptyLineRemover(SyntaxTree syntaxTree) {
		_syntaxTree = syntaxTree;
	}

	public override SyntaxToken VisitToken(SyntaxToken token) {
		// Only process end-of-line trivia
		if (!token.HasTrailingTrivia)
			return base.VisitToken(token);

		var newTrailingTrivia = ProcessTrivia(token.TrailingTrivia);
		return token.WithTrailingTrivia(newTrailingTrivia);
	}

	private SyntaxTriviaList ProcessTrivia(SyntaxTriviaList triviaList) {
		var newTrivia = new List<SyntaxTrivia>();
		var emptyLineCount = 0;
		var lastWasEndOfLine = false;

		foreach (var trivia in triviaList) {
			if (trivia.IsKind(SyntaxKind.EndOfLineTrivia)) {
				if (lastWasEndOfLine) {
					emptyLineCount++;
				} else {
					emptyLineCount = 1;
					lastWasEndOfLine = true;
				}

				// Keep only up to MaxAllowedEmptyLines consecutive empty lines
				if (emptyLineCount <= MaxAllowedEmptyLines) {
					newTrivia.Add(trivia);
				}
				// Else: skip this end-of-line (effectively removing the empty line)
			} else {
				// Reset counter for non-empty-line trivia
				emptyLineCount = 0;
				lastWasEndOfLine = false;
				newTrivia.Add(trivia);
			}
		}

		return SyntaxFactory.TriviaList(newTrivia);
	}
}
