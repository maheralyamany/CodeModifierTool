using System;
using System.IO;

public class FileProcessor {
	public void CleanFileEmptyLines(string filePath) {
		try {
			// Read the file
			var code = File.ReadAllText(filePath);

			// Remove excessive empty lines
			var cleaner = new EmptyLineCleaner();
			var cleanedCode = cleaner.RemoveExcessiveEmptyLines(code);

			// Write back to file
			File.WriteAllText(filePath, cleanedCode);

			Console.WriteLine($"Cleaned empty lines in: {filePath}");
		} catch (Exception ex) {
			Console.WriteLine($"Error processing {filePath}: {ex.Message}");
		}
	}

	public void ProcessDirectory(string directoryPath) {
		var csFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

		foreach (var file in csFiles) {
			CleanFileEmptyLines(file);
		}
	}
}
