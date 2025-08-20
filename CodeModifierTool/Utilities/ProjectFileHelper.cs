using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public class ProjectFileHelper {
	private List<string> excludedExtensions = new List<string>() { ".Designer.cs" };
	private List<string> blockedFolders = new List<string>() { "Properties", "bin", "obj", "Resources", "packages" };
	public List<string> BlockedFolders {
		get {
			if (blockedFolders == null)
				blockedFolders = new List<string>();
			return blockedFolders;
		}
		set {
			blockedFolders = value;
		}
	}
	public List<string> ExcludedExtensions {
		get {
			if (excludedExtensions == null)
				excludedExtensions = new List<string>();
			return excludedExtensions;
		}
		set {
			excludedExtensions = value;
		}
	}
	public ProjectFileHelper() {

	}


	public List<string> FindProjectsPath(string rootDirectory) {
		if (rootDirectory.Trim().IsEmptyOrWhiteSpace())
			return null;
		//var projectName = Path.GetFileName(rootDirectory);
		var dir = rootDirectory;
		if (!Directory.Exists(dir)) {
			return new List<string>();
		}
		int level = 0;
		while (dir != null && level < 4) {
			var projectFiles = Directory.GetFiles(dir, "*.csproj", SearchOption.AllDirectories);
			if (projectFiles.Length > 0) {

				return projectFiles.ToList();
			}
			level++;
			var prnt = Directory.GetParent(dir);
			dir = prnt?.FullName ?? null;
		}
		return new List<string>();
	}



	private List<string> GetCompileFilesManual(string csprojPath) {
		var result = new List<string>();

		if (!File.Exists(csprojPath))
			return result;

		var projectDir = Path.GetDirectoryName(csprojPath);
		var doc = XDocument.Load(csprojPath);
		XNamespace ns = doc.Root.Name.Namespace;

		// Handle <Compile Include="...">
		var compileItems = doc.Descendants(ns + "Compile")
							  .Select(x => x.Attribute("Include")?.Value)
							  .Where(v => !string.IsNullOrEmpty(v))
							  .ToList();

		foreach (var include in compileItems) {
			if (include.Contains("*")) // Handle wildcard
			{
				var fullWildcardPath = Path.Combine(projectDir, include);
				var baseDir = Path.GetDirectoryName(fullWildcardPath);
				var pattern = Path.GetFileName(fullWildcardPath);

				if (Directory.Exists(baseDir)) {
					var matchedFiles = Directory.GetFiles(baseDir, pattern, SearchOption.AllDirectories);
					result.AddRange(matchedFiles);
				}
			} else if (!BlockedFolders.Any(b => include.MStartsWith(b)) && !IsExcludedExtension(include) && include.MEndsWith(".cs")) {
				var fullPath = Path.GetFullPath(Path.Combine(projectDir, include));
				if (File.Exists(fullPath)) {
					result.Add(fullPath);
				}
			}
		}

		return result.Distinct().ToList();
	}


	public List<string> DirectoryFiles(string rootDirectory) {
		var allCsFiles = DirectoryFilesInternal(rootDirectory);
		var csFiles = Directory.GetFiles(rootDirectory, "*.cs", SearchOption.AllDirectories);
		return allCsFiles.LWhere(f => csFiles.Contains(f));
	}

	private List<string> DirectoryFilesInternal(string rootDirectory) {
		var projectsPath = FindProjectsPath(rootDirectory);
		if (projectsPath.Count > 0) {
			List<string> alllines = new List<string>();
			foreach (var projectPath in projectsPath) {
				var lines = GetCompileFilesManual(projectPath);

				if (lines.Count > 0)
					alllines.AddRange(lines);
			}
			if (alllines.Count > 0)
				return alllines.LOrderBy(s => s);
		}
		//ExcludedExtensions
		var blocked = BlockedFolders.LSelect(f => Path.Combine(rootDirectory, f));
		var csFiles = Directory.GetFiles(rootDirectory, "*.cs", SearchOption.AllDirectories).LWhere(f => !blocked.Any(b => f.MStartsWith(b)) && !IsExcludedExtension(f)).LOrderBy(s => s);
		return csFiles;
	}

	private bool IsExcludedExtension(string file) {
		if (ExcludedExtensions.IsNotValidCollection())
			return false;


		return ExcludedExtensions.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
	}
}
