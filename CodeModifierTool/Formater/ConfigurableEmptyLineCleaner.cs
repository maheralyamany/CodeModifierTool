using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ConfigurableEmptyLineCleaner {
	private readonly int _maxEmptyLines;
	private readonly bool _preserveHeader;
	private readonly bool _preserveAroundBraces;

	public ConfigurableEmptyLineCleaner(int maxEmptyLines = 2, bool preserveHeader = true, bool preserveAroundBraces = true) {
		_maxEmptyLines = maxEmptyLines;
		_preserveHeader = preserveHeader;
		_preserveAroundBraces = preserveAroundBraces;
	}

	public string CleanCode(string sourceCode) {
		if (_preserveAroundBraces) {
			return CleanWithBraceAwareness(sourceCode);
		} else {
			return CleanSimple(sourceCode);
		}
	}

	private string CleanSimple(string sourceCode) {
		var lines = sourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		var resultLines = new List<string>();
		var emptyLineCount = 0;
		var inHeader = _preserveHeader;

		foreach (var line in lines) {
			if (string.IsNullOrWhiteSpace(line)) {
				emptyLineCount++;
				if (emptyLineCount <= _maxEmptyLines && !(inHeader && emptyLineCount == 1)) {
					resultLines.Add(line);
				}
			} else {
				emptyLineCount = 0;
				resultLines.Add(line);

				// Check if we're exiting header region
				if (inHeader && !line.TrimStart().StartsWith("//") && !line.TrimStart().StartsWith("/*")) {
					inHeader = false;
				}
			}
		}

		return string.Join(Environment.NewLine, resultLines);
	}
	public static string GetIndentation(string line) {
		// Regex to detect spaces or tabs at the beginning of a line
		var indentMatch = Regex.Match(line, @"^\s*");
		if (indentMatch.Success) {
			return indentMatch.Value; // Return the exact indentation (spaces/tabs)
		}
		return string.Empty; // No indentation
	}
	private string GetRegionLine(string line, int index, string[] lines) {
		if (line.StartsWith("#region") || line.StartsWith("#endregion")) {
			int nextIndex = -1;
			if (index + 1 < lines.Length) {
				nextIndex = index + 1;
			} else if (index > 0)
				nextIndex = index - 1;
			if (nextIndex > -1) {
				var nextline = lines[nextIndex] ?? "";
				string indent = GetIndentation(nextline);
				return $"{indent}{line}";
			}
		}
		return line;
	}
	private string CleanWithBraceAwareness(string sourceCode) {
		var lines = sourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		var resultLines = new List<string>();
		var emptyLineCount = 0;
		var inHeader = _preserveHeader;
		var lastNonEmptyLine = "";

		for (int i = 0; i < lines.Length; i++) {
			var line = lines[i];
			var trimmedLine = line.Trim();
			var isWhitespaceOnly = string.IsNullOrWhiteSpace(line);

			if (isWhitespaceOnly) {
				emptyLineCount++;

				// Special handling for lines around braces
				var shouldPreserve = (lastNonEmptyLine == "{" || lastNonEmptyLine == "}") && emptyLineCount == 1;

				if ((emptyLineCount <= _maxEmptyLines || shouldPreserve) && !(inHeader && emptyLineCount == 1)) {
					resultLines.Add(line);
				}
			} else {
				line = GetRegionLine(line, i, lines);
				emptyLineCount = 0;
				resultLines.Add(line);
				lastNonEmptyLine = trimmedLine;

				// Check if we're exiting header region
				if (inHeader && !trimmedLine.StartsWith("//") && !trimmedLine.StartsWith("/*")) {
					inHeader = false;
				}
			}
		}

		return string.Join(Environment.NewLine, resultLines);
	}
}
