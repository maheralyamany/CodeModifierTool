using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;

public static class RoslynProjectLoader {
	/// <summary>
	/// Loads all .cs files from a folder into an AdhocWorkspace project.
	/// </summary>
	public static async Task<List<BaseCodeSyntaxRewriter>> LoadAndCreateRewritersAsync(
		string folderPath) {
		var workspace = new AdhocWorkspace();
		var projectId = ProjectId.CreateNewId();
		var projectInfo = ProjectInfo.Create(
			projectId,
			VersionStamp.Create(),
			name: "TempProject",
			assemblyName: "TempAssembly",
			language: LanguageNames.CSharp
		);

		var project = workspace.AddProject(projectInfo);

		// Add all .cs files as Documents
		foreach (var file in Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories)) {
			string text = File.ReadAllText(file);
			var docId = DocumentId.CreateNewId(project.Id);


			var docInfo = DocumentLoader.CreateDocumentInfo(docId, file);

			workspace.AddDocument(docInfo);
		}

		project = workspace.CurrentSolution.GetProject(projectId)!;
		var rewriters = new List<BaseCodeSyntaxRewriter>();

		// Iterate all documents
		foreach (var doc in project.Documents) {
			var semanticModel = await doc.GetSemanticModelAsync();
			if (semanticModel == null) continue;

			// Create instance of your rewriter using InstanceFactory
			var rewriter = InstanceFactory.CreateInstance<BaseCodeSyntaxRewriter>(
				semanticModel); // optional param defaults to false
			rewriters.Add(rewriter);
		}

		return rewriters;
	}
}
