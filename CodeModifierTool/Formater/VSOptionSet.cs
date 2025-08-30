using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;

public static class VSOptionSet {
	private static OptionSet optionSet;
	private static OptionSet customOptionSet;
	private static Dictionary<OptionKey, object> customOptionSetValues = new Dictionary<OptionKey, object>();
	private static AdhocWorkspace workspace;
	static VSOptionSet() {
		workspace = new AdhocWorkspace();

		customOptionSet = GetCustomOptionSetInternal(workspace);
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

		//return customOptionSet;
		return optionSet ?? customOptionSet;
	}
	public static OptionSet GetCustomOptionSet() {

		return customOptionSet;

	}
	public static AdhocWorkspace GetAdhocWorkspace() {

		return workspace;

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


	private static readonly Dictionary<OptionKey, object> optionSetKeys = new Dictionary<OptionKey, object>() {
		{CSharpFormattingOptions.IndentBlock, true},
		{CSharpFormattingOptions.IndentSwitchSection, true},
		{CSharpFormattingOptions.IndentSwitchCaseSection, true},
		{CSharpFormattingOptions.IndentBraces, false},
		{CSharpFormattingOptions.IndentSwitchCaseSectionWhenBlock, false},
		{CSharpFormattingOptions.NewLinesForBracesInTypes, false},
		{CSharpFormattingOptions.NewLinesForBracesInMethods, false},
		{CSharpFormattingOptions.NewLinesForBracesInProperties, false},
		{CSharpFormattingOptions.NewLinesForBracesInAccessors, false},

		{CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods, false},
		{CSharpFormattingOptions.NewLinesForBracesInControlBlocks, false},
		{CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes, false},
		{CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers, false},
		{CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody, false},
		{CSharpFormattingOptions.SpaceBetweenEmptyMethodDeclarationParentheses, false},
		{CSharpFormattingOptions.SpaceBetweenEmptyMethodCallParentheses, false},
		{CSharpFormattingOptions.SpaceAfterCast, false},
		{CSharpFormattingOptions.SpaceAfterColonInBaseTypeDeclaration, false},
		{CSharpFormattingOptions.SpaceAfterComma, false},
		{CSharpFormattingOptions.SpaceAfterSemicolonsInForStatement, false},
		{CSharpFormattingOptions.SpaceBeforeColonInBaseTypeDeclaration, false},
		{CSharpFormattingOptions.SpaceBeforeSemicolonsInForStatement, false},
		{CSharpFormattingOptions.SpaceWithinExpressionParentheses, false},
		{CSharpFormattingOptions.SpaceWithinMethodDeclarationParenthesis, false},
		{CSharpFormattingOptions.SpaceWithinMethodCallParentheses, false},
		{CSharpFormattingOptions.SpaceWithinOtherParentheses, false},
		{CSharpFormattingOptions.SpaceWithinSquareBrackets, false},
		{CSharpFormattingOptions.SpacingAfterMethodDeclarationName, false},
		{CSharpFormattingOptions.SpaceAfterMethodCallName, false},
		{CSharpFormattingOptions.SpaceAfterControlFlowStatementKeyword, false},
		{CSharpFormattingOptions.SpaceWithinCastParentheses, false},
		{CSharpFormattingOptions.SpacesIgnoreAroundVariableDeclaration, false},
		{CSharpFormattingOptions.SpaceBeforeOpenSquareBracket, false},
		{CSharpFormattingOptions.SpaceBetweenEmptySquareBrackets, false},
		{CSharpFormattingOptions.SpaceAfterDot, false},
		{CSharpFormattingOptions.SpaceBeforeComma, false},
		{CSharpFormattingOptions.SpaceBeforeDot, false},
		{CSharpFormattingOptions.WrappingPreserveSingleLine, true},
		{CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, true},
		{CSharpFormattingOptions.NewLineForMembersInObjectInit, false},
		{CSharpFormattingOptions.NewLineForClausesInQuery, false},
		{CSharpFormattingOptions.NewLineForMembersInAnonymousTypes, false},
		{CSharpFormattingOptions.NewLineForElse, false},
		{CSharpFormattingOptions.NewLineForCatch, false},
		{CSharpFormattingOptions.NewLineForFinally, false},
	};


	public static Dictionary<OptionKey, (object Old, object Def, object New)> GetCustomOptionSetValues() {
		var options = GetCustomOptionSet();
		var docOptions = GetOptionSet();
		Dictionary<OptionKey, (object Old, object Def, object New)> values = new Dictionary<OptionKey, (object Old, object Def, object New)>();
		foreach (var it in optionSetKeys) {

			values.Add(it.Key, (customOptionSetValues.GetSafeValue(it.Key), docOptions.GetOption(it.Key), options.GetOption(it.Key)));

		}
		return values;

	}
	private static OptionSet GetCustomOptionSetInternal(AdhocWorkspace workspace) {


		var options = workspace.Options;

		/*Dictionary<string, bool> existsOptions = new Dictionary<string, bool>();
         OptionSet WithChangedOption<T>(OptionSet optionSet, Option<T> option, T value) {
            if (existsOptions.ContainsKey(option.Name))
                return optionSet;
            existsOptions.Add(option.Name, true);
            return optionSet.WithChangedOption(option, value);
        };*/
		customOptionSetValues.Clear();
		static OptionSet WithPerChangedOption<T>(OptionSet optionSet, PerLanguageOption<T> option, T value) {
			return optionSet.WithChangedOption(option, LanguageNames.CSharp, value);

		};
		// --- Indentation ---
		options = WithPerChangedOption(options, FormattingOptions.UseTabs, false);
		options = WithPerChangedOption(options, FormattingOptions.SmartIndent, FormattingOptions.IndentStyle.Smart);
		options = WithPerChangedOption(options, FormattingOptions.IndentationSize, 4);
		options = WithPerChangedOption(options, FormattingOptions.TabSize, 4);


		foreach (var it in optionSetKeys) {
			var v = options.GetOption(it.Key);
			customOptionSetValues.Add(it.Key, v);
			if (!v.Equals(it.Value))
				options = options.WithChangedOption(it.Key, it.Value);
		}


		/*options = WithChangedOption(options, CSharpFormattingOptions.IndentBlock, false);
        options = WithChangedOption(options, CSharpFormattingOptions.IndentSwitchSection, false);
        options = WithChangedOption(options, CSharpFormattingOptions.IndentSwitchCaseSection, false);
        options = WithChangedOption(options, CSharpFormattingOptions.IndentBraces, false);
        options = WithChangedOption(options, CSharpFormattingOptions.IndentSwitchCaseSectionWhenBlock, false);
        // --- Braces (Allman style) ---

        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInTypes, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInMethods, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInProperties, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLineForMembersInObjectInit, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInAccessors, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLineForClausesInQuery, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLineForMembersInAnonymousTypes, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInControlBlocks, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody, false);

        // --- Spacing ---
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceBetweenEmptyMethodDeclarationParentheses, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceBetweenEmptyMethodCallParentheses, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceAfterCast, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceAfterColonInBaseTypeDeclaration, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceAfterComma, true);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceAfterSemicolonsInForStatement, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceBeforeColonInBaseTypeDeclaration, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceBeforeSemicolonsInForStatement, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceWithinExpressionParentheses, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceWithinMethodDeclarationParenthesis, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceWithinMethodCallParentheses, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceWithinOtherParentheses, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceWithinSquareBrackets, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpacingAfterMethodDeclarationName, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceAfterMethodCallName, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceAfterControlFlowStatementKeyword, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceWithinCastParentheses, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpacesIgnoreAroundVariableDeclaration, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceBeforeOpenSquareBracket, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceBetweenEmptySquareBrackets, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceAfterDot, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceBeforeComma, false);
        options = WithChangedOption(options, CSharpFormattingOptions.SpaceBeforeDot, false);

        // --- Wrapping ---
        options = WithChangedOption(options, CSharpFormattingOptions.WrappingPreserveSingleLine, true);
        options = WithChangedOption(options, CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, true);

        // Other block formatting


        options = WithChangedOption(options, CSharpFormattingOptions.NewLineForElse, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLineForCatch, false);
        options = WithChangedOption(options, CSharpFormattingOptions.NewLineForFinally, false);*/



		return options;
	}
}
