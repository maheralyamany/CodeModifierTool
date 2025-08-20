using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;

public static class VSOptionSet {
	private static OptionSet optionSet;
	private static readonly OptionSet customOptionSet;
	static VSOptionSet() {
		customOptionSet = GetCustomOptionSetInternal();
	}
	/// <summary>
	/// Returns a fully populated C# OptionSet based on the current document's Visual Studio settings.
	/// </summary>
	public static void InitOptionSet(Document document) {
		if (optionSet == null) {
			optionSet = GetFullCSharpOptionSetAsync(document);
		}

	}
	public static OptionSet GetOptionSet(SyntaxNode root) {
		if (optionSet == null) {
			return GetOptionSet(root.SyntaxTree?.FilePath ?? "");
		}
		return GetOptionSet();
	}
	public static OptionSet GetOptionSet(string filePath) {
		if (optionSet == null) {

			if (!string.IsNullOrWhiteSpace(filePath)) {
				Document document = DocumentLoader.WithDocumentLoader((loader) => {
					return loader.LoadDocumentFromPath(filePath);
				});
				optionSet = GetFullCSharpOptionSetAsync(document);
			}

		}
		return GetOptionSet();
	}
	public static OptionSet GetOptionSet() {

		return optionSet ?? customOptionSet;
	}
	/// <summary>
	/// Returns a fully populated C# OptionSet based on the current document's Visual Studio settings.
	/// </summary>
	public static OptionSet GetFullCSharpOptionSetAsync(Document document) {
		if (document == null)
			return null;
		if (document == null)
			throw new ArgumentNullException(nameof(document));

		var docOptions = document.GetOptionsAsync().Result;
		var workspace = new AdhocWorkspace();
		var optionSet = workspace.Options;

		void WithPerChangedOption<T>(PerLanguageOption<T> option) {
			optionSet = optionSet.WithChangedOption(option, LanguageNames.CSharp, docOptions.GetOption(option, LanguageNames.CSharp));
		};
		Dictionary<string, bool> existsOptions = new Dictionary<string, bool>();
		void WithChangedOption<T>(Option<T> option) {

			if (existsOptions.ContainsKey(option.Name))
				return;
			existsOptions.Add(option.Name, true);
			optionSet = optionSet.WithChangedOption(option, docOptions.GetOption(option));
		};

		// Indentation
		WithPerChangedOption(FormattingOptions.IndentationSize);
		WithPerChangedOption(FormattingOptions.TabSize);
		WithPerChangedOption(FormattingOptions.UseTabs);
		WithChangedOption(CSharpFormattingOptions.IndentBraces);
		WithChangedOption(CSharpFormattingOptions.IndentSwitchCaseSectionWhenBlock);
		// New Lines for Braces
		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes);
		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods);

		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties);
		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAccessors);
		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods);
		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInControlBlocks);
		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody);
		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes);
		WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers);
		WithChangedOption(CSharpFormattingOptions.NewLineForMembersInObjectInit);
		WithChangedOption(CSharpFormattingOptions.NewLineForMembersInAnonymousTypes);
		WithChangedOption(CSharpFormattingOptions.NewLineForClausesInQuery);
		// Spacing
		//WithChangedOption(CSharpFormattingOptions.SpaceWithinParentheses);
		//WithChangedOption(CSharpFormattingOptions.SpaceWithinBrackets);
		WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptyMethodCallParentheses);
		WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptyMethodDeclarationParentheses);
		WithChangedOption(CSharpFormattingOptions.SpaceAfterCast);
		WithChangedOption(CSharpFormattingOptions.SpaceAfterColonInBaseTypeDeclaration);
		WithChangedOption(CSharpFormattingOptions.SpaceAfterComma);
		WithChangedOption(CSharpFormattingOptions.SpaceAfterSemicolonsInForStatement);
		WithChangedOption(CSharpFormattingOptions.SpaceBeforeColonInBaseTypeDeclaration);
		WithChangedOption(CSharpFormattingOptions.SpaceBeforeSemicolonsInForStatement);
		WithChangedOption(CSharpFormattingOptions.SpaceWithinExpressionParentheses);
		WithChangedOption(CSharpFormattingOptions.SpaceWithinMethodDeclarationParenthesis);
		WithChangedOption(CSharpFormattingOptions.SpaceWithinMethodCallParentheses);
		WithChangedOption(CSharpFormattingOptions.SpaceWithinOtherParentheses);
		WithChangedOption(CSharpFormattingOptions.SpaceWithinSquareBrackets);
		WithChangedOption(CSharpFormattingOptions.SpacingAfterMethodDeclarationName);
		WithChangedOption(CSharpFormattingOptions.SpaceAfterMethodCallName);
		WithChangedOption(CSharpFormattingOptions.SpaceAfterControlFlowStatementKeyword);
		WithChangedOption(CSharpFormattingOptions.SpaceWithinCastParentheses);
		WithChangedOption(CSharpFormattingOptions.SpacesIgnoreAroundVariableDeclaration);
		WithChangedOption(CSharpFormattingOptions.SpaceBeforeOpenSquareBracket);
		WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptySquareBrackets);
		WithChangedOption(CSharpFormattingOptions.SpaceAfterDot);
		WithChangedOption(CSharpFormattingOptions.SpaceBeforeComma);
		WithChangedOption(CSharpFormattingOptions.SpaceBeforeDot);
		// Wrapping
		WithChangedOption(CSharpFormattingOptions.WrappingPreserveSingleLine);
		WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine);
		// Other block formatting
		WithChangedOption(CSharpFormattingOptions.IndentBlock);
		WithChangedOption(CSharpFormattingOptions.IndentSwitchSection);
		WithChangedOption(CSharpFormattingOptions.IndentSwitchCaseSection);
		WithChangedOption(CSharpFormattingOptions.LabelPositioning);
		WithChangedOption(CSharpFormattingOptions.NewLineForElse);
		WithChangedOption(CSharpFormattingOptions.NewLineForCatch);
		WithChangedOption(CSharpFormattingOptions.NewLineForFinally);


		return optionSet;
	}


	private static OptionSet GetCustomOptionSetInternal() {

		var workspace = new AdhocWorkspace();
		var options = workspace.Options;

		// --- Indentation ---
		options = options.WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, false);
		options = options.WithChangedOption(FormattingOptions.IndentationSize, LanguageNames.CSharp, 4);
		options = options.WithChangedOption(FormattingOptions.TabSize, LanguageNames.CSharp, 4);

		// --- Braces (Allman style) ---
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, true);
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, true);
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties, true);
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAccessors, true);
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods, true);
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInControlBlocks, true);
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes, true);
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers, true);
		options = options.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody, true);

		// --- Spacing ---
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceAfterCast, false);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceAfterColonInBaseTypeDeclaration, true);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceAfterComma, true);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceAfterSemicolonsInForStatement, true);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceBeforeColonInBaseTypeDeclaration, true);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceBeforeSemicolonsInForStatement, false);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceWithinExpressionParentheses, false);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceWithinMethodDeclarationParenthesis, false);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceWithinMethodCallParentheses, false);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceWithinOtherParentheses, false);
		options = options.WithChangedOption(CSharpFormattingOptions.SpaceWithinSquareBrackets, false);

		// --- Wrapping ---
		options = options.WithChangedOption(CSharpFormattingOptions.WrappingPreserveSingleLine, true);
		options = options.WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, true);

		return options;
	}
}
