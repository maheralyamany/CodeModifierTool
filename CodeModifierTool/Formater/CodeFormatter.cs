using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;

public static class CodeFormatter {
	public static string GetFormattedCode(this SyntaxNode root) {

		var formattedCode = CodeFormatter.FormatCodeWithOptions(root);
		//
		return formattedCode;
	}
	public static SyntaxNode FormatWithCustomOptions(SyntaxNode root) {

		var code = FormatCodeWithOptions(root);
		// Parse again to get clean SyntaxNode
		return CSharpSyntaxTree.ParseText(code).GetRoot();
	}
	public static string NormalizeCode(string code, bool useWindowsLineEndings = false) {
		// Regex: replace 4+ consecutive newlines with just 3
		//code = Regex.Replace(code, @"(\r?\n){4,}", match => new string('\n', 3));
		// Normalize all line endings first
		code = Regex.Replace(code, @"\r\n|\r|\n", "\n");

		// Collapse >3 blank lines into exactly 3
		code = Regex.Replace(code, @"(\n){4,}", "\n\n\n");

		// Trim leading blank lines
		code = Regex.Replace(code, @"^(\s*\n)+", string.Empty);

		// Trim trailing blank lines
		code = Regex.Replace(code, @"(\n\s*)+$", string.Empty);

		// Ensure exactly one newline at EOF
		code += "\n";

		// Convert line endings to requested style
		if (useWindowsLineEndings)
			code = code.Replace("\n", "\r\n");

		return code;

	}
	public static string CleanCode(string code) {
		var cleaner = new ConfigurableEmptyLineCleaner(maxEmptyLines: 2, preserveHeader: true, preserveAroundBraces: true);
		var cleanedCode = cleaner.CleanCode(code);

		return cleanedCode;

	}
	public static string SplitCamelCase(string input) {
		if (string.IsNullOrEmpty(input))
			return input;
		var result = new StringBuilder();
		result.Append(char.ToLower(input[0]));
		for (int i = 1; i < input.Length; i++) {
			if (char.IsUpper(input[i])) {
				result.Append(' ');
				result.Append(char.ToLower(input[i]));
			} else {
				result.Append(input[i]);
			}
		}
		return result.ToString();
	}
	public static string ToTitle(string input) {

		return ToUpperFirstChar(SplitCamelCase(input));
	}
	public static string ToUpperFirstChar(string input) {
		if (string.IsNullOrEmpty(input))
			return input;
		return char.ToUpper(input[0]) + input.Substring(1);
	}
	public static string FormatCodeWithOptions(SyntaxNode root) {
		// Create custom option set
		//OptionSet optionSet = VSOptionSet.GetOptionSet(root);
		OptionSet optionSet = null;
		// Apply formatting
		var formatted = Formatter.Format(root, new AdhocWorkspace(), optionSet);
		// Convert to string
		string code = formatted.NormalizeWhitespace().ToFullString();
		code = CleanCode(code);
		return code;
	}
	public static string FormatCodeWithOptions(string code) {
		// Parse the code
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var root = syntaxTree.GetRoot();


		return FormatCodeWithOptions(root);
	}
	public static OptionSet CreateCustomOptionSet() {
		var workspace = new AdhocWorkspace();
		var optionSet = workspace.Options;

		// Common formatting options
		optionSet = optionSet.WithChangedOption(FormattingOptions.IndentationSize, LanguageNames.CSharp, 4);
		optionSet = optionSet.WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, false);
		optionSet = optionSet.WithChangedOption(FormattingOptions.NewLine, LanguageNames.CSharp, "\r\n");

		// C# specific options
		/* optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.IndentationSize, 4);
         optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.UseTabs, false);*/
		optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, true);
		optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, true);
		optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.SpaceAfterControlFlowStatementKeyword, true);
		optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.SpaceAfterSemicolonsInForStatement, true);
		optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.SpaceBeforeOpenSquareBracket, false);
		optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptyMethodDeclarationParentheses, false);
		optionSet = optionSet.WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptyMethodCallParentheses, false);

		return optionSet;
	}

}
