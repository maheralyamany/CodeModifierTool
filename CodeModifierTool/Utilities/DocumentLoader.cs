using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
public class DocumentLoader : IDisposable {
	//private MSBuildWorkspace _workspace;
	private Solution _solution;

	public DocumentLoader() {
		// Register MSBuild instances
		/*if (MSBuildLocator.CanRegister && !MSBuildLocator.IsRegistered)
            MSBuildLocator.RegisterDefaults();
        _workspace = MSBuildWorkspace.Create();*/
	}

	public Document LoadDocumentFromPath(string filePath) {
		try {
			// Check if file exists
			if (!File.Exists(filePath)) {
				throw new FileNotFoundException($"File not found: {filePath}");
			}

			// Check if it's a .cs file
			if (!filePath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)) {
				throw new ArgumentException("Only .cs files are supported");
			}

			// Try to find the document in an existing solution
			if (_solution != null) {
				var documentInSolution = _solution.Projects
					.SelectMany(p => p.Documents)
					.FirstOrDefault(d => string.Equals(d.FilePath, filePath, StringComparison.OrdinalIgnoreCase));

				if (documentInSolution != null) {
					return documentInSolution;
				}
			}

			// If not part of a solution, create a single-file document
			return CreateDocumentForSingleFile(filePath);
		} catch (Exception ex) {
			throw new ApplicationException($"Failed to load document from path: {filePath}", ex);
		}
	}

	public async Task<Solution> LoadSolutionAsync(string solutionPath) {
		try {
			if (!MSBuildLocator.IsRegistered && MSBuildLocator.CanRegister)
				MSBuildLocator.RegisterDefaults();
			using var workspace = MSBuildWorkspace.Create();
			_solution = await workspace.OpenSolutionAsync(solutionPath);
			return _solution;
		} catch (Exception ex) {
			throw new ApplicationException($"Failed to load solution: {solutionPath}", ex);
		}

	}
	public List<(string Directory, string Name)> LoadSolutionProjectsNames(string solutionPath) {
		if (string.IsNullOrWhiteSpace(solutionPath) || !File.Exists(solutionPath))
			return new List<(string Directory, string Name)>();
		try {

			var lines = File.ReadAllLines(solutionPath).LWhere(l => l.ToLower().StartsWith("project(")).LSelect(l => {
				string Directory = null;
				string Name = null;
				var r = l.Split("\", \"").FirstOrDefault(s => s.Contains(".csproj"));
				if (r != null) {
					var sp = r.Replace(".csproj", "").Split("\\");
					Directory = sp.First();
					Name = sp.Last();


				}
				return (Directory, Name);
			}).LWhere(s => s.Directory != null && s.Name != null);





			/*var solution = LoadSolutionAsync(solutionPath).Result;
            if (solution != null) {
                return solution.Projects.Select(p => p.Name).ToList();
            }*/
			return lines;
		} catch (Exception ex) {
			Console.WriteLine(ex.Message);
		}
		return new List<(string Directory, string Name)>();
	}
	public static List<(string Directory, string Name)> LoadSolutionProjectsNamesCore(string solutionPath) {

		using (var loader = new DocumentLoader()) {
			return loader.LoadSolutionProjectsNames(solutionPath);
		}
	}

	private Document CreateDocumentForSingleFile(string filePath) {


		// Create a project with basic references
		var projectId = ProjectId.CreateNewId();
		var documentId = DocumentId.CreateNewId(projectId);
		var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
		var projectInfo = ProjectInfo.Create(
			projectId,
			VersionStamp.Create(),
			"SingleFileProject",
			"SingleFileProject",
			LanguageNames.CSharp,
			filePath: null,
			compilationOptions: compilationOptions,
			metadataReferences: GetDefaultReferences());
		/* var documentInfo1 = DocumentInfo.Create(
             documentId,
             Path.GetFileName(filePath),
             filePath: filePath,
             loader: Microsoft.CodeAnalysis.TextLoader.From(
                 Microsoft.CodeAnalysis.Text.TextAndVersion.Create(
                     sourceText,
                     VersionStamp.Create())));*/
		var documentInfo = CreateDocumentInfo(documentId, filePath);
		// Create a new solution with this single document
		var solution = new AdhocWorkspace()
			.CurrentSolution
			.AddProject(projectInfo)
			.AddDocument(documentInfo);
		return solution.GetDocument(documentId);
	}
	public static DocumentInfo CreateDocumentInfo(DocumentId documentId, string filePath) {
		// Read file content
		var sourceText = SourceText.From(File.ReadAllText(filePath));

		var textAndVersion = TextAndVersion.Create(sourceText, VersionStamp.Create(), filePath);

		return DocumentInfo.Create(
			documentId,
			Path.GetFileName(filePath),
			filePath: filePath,
			loader: TextLoader.From(textAndVersion));
	}
	private MetadataReference[] GetDefaultReferences() {
		// Add basic .NET Framework references
		var references = new[]
		{
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location)
		};

		// Try to add common framework assemblies
		var frameworkPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
		var additionalReferences = new[]
		{
			"System.Core.dll",
			"System.dll",
			"mscorlib.dll",
			"System.Runtime.dll"
		};

		return additionalReferences
			.Select(r => Path.Combine(frameworkPath, r))
			.Where(File.Exists)
			.Select(r => MetadataReference.CreateFromFile(r))
			.Concat(references)
			.ToArray();
	}
	public static T WithDocumentLoader<T>(Func<DocumentLoader, T> action) {
		using (var loader = new DocumentLoader()) {
			return action.Invoke(loader);
		}
	}
	public void Dispose() {
		//_workspace?.Dispose();
	}
}
