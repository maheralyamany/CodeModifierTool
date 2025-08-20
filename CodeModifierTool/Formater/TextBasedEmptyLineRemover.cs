using System;
using System.Collections.Generic;

public class TextBasedEmptyLineRemover {
    private const int MaxAllowedEmptyLines = 3;

    public string RemoveExcessiveEmptyLines(string sourceCode) {
        var lines = sourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var resultLines = new List<string>();
        var emptyLineCount = 0;

        foreach (var line in lines) {
            if (string.IsNullOrWhiteSpace(line)) {
                emptyLineCount++;
                if (emptyLineCount <= MaxAllowedEmptyLines) {
                    resultLines.Add(line);
                }
            } else {
                emptyLineCount = 0;
                resultLines.Add(line);
            }
        }

        return string.Join(Environment.NewLine, resultLines);
    }

    public string RemoveExcessiveEmptyLinesPreserveStructure(string sourceCode) {
        var lines = sourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var resultLines = new List<string>();
        var emptyLineCount = 0;
        var lastNonEmptyLineWasBrace = false;

        for (int i = 0; i < lines.Length; i++) {
            var line = lines[i];
            var trimmedLine = line.Trim();
            var isWhitespaceOnly = string.IsNullOrWhiteSpace(line);
            var isBraceLine = trimmedLine == "{" || trimmedLine == "}";

            if (isWhitespaceOnly) {
                emptyLineCount++;

                // Special handling for lines around braces
                if (lastNonEmptyLineWasBrace && emptyLineCount == 1) {
                    // Keep one empty line after brace
                    resultLines.Add(line);
                } else if (emptyLineCount <= MaxAllowedEmptyLines) {
                    resultLines.Add(line);
                }
            } else {
                emptyLineCount = 0;
                resultLines.Add(line);
                lastNonEmptyLineWasBrace = isBraceLine;
            }
        }

        return string.Join(Environment.NewLine, resultLines);
    }
}