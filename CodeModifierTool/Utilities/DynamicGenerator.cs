using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeModifierTool {
	public static class DynamicGenerator {

		public static T WithDocumentLoader<T>(Func<DocumentLoader, T> action) {
			return DocumentLoader.WithDocumentLoader(action);

		}
		public static T CreateInstance<T>(params (string Name, object Value)[] args) {
			var newArgs = args.IsNotValidParams() ? new Dictionary<string, object>() : args.ToMDictionary(s => s.Name, s => s.Value);
			return InstanceFactory.CreateInstance<T>(newArgs);

		}
		public static (SyntaxNode newRoot, bool IsEquivalent) VisitDocument<T>(string filePath, params (string Name, object Value)[] args) where T : BaseCodeSyntaxRewriter {
			var newArgs = args.IsNotValidParams() ? new Dictionary<string, object>() : args.ToMDictionary(s => s.Name, s => s.Value);
			return WithDocumentLoader((loader) => {
				Document document = loader.LoadDocumentFromPath(filePath);
				VSOptionSet.InitOptionSet(document);
				// Get semantic model
				var syntaxTree = document.GetSyntaxTreeAsync().Result;

				var semanticModel = document.GetSemanticModelAsync().Result;
				// Create and apply rewriter
				newArgs.AddIfNotExists("semanticModel", semanticModel);
				var rewriter = InstanceFactory.CreateInstance<T>(newArgs);
				var root = syntaxTree.GetRoot();
				var newRoot = rewriter.VisitEntireTree(root);
				var IsEquivalent = rewriter.IsEquivalentTo(root);
				var AllDeclarations = rewriter.AllDeclarations;
				return (newRoot, IsEquivalent);
			});
			/*using (var loader = new DocumentLoader()) {
                Document document = loader.LoadDocumentFromPath(filePath);
                // Get semantic model
                var semanticModel = document.GetSemanticModelAsync().Result;
                // Create and apply rewriter
                var rewriter = new AdvancedDocumentationRewriter(semanticModel);
                var newRoot = rewriter.Visit(document.GetSyntaxRootAsync().Result);
                // Format and save
                var formattedCode = FormatCode(newRoot);
                File.WriteAllText(filePath, formattedCode);
            }*/
		}

		public static Document LoadDocument(string filePath) {

			return WithDocumentLoader((loader) => {
				Document document = loader.LoadDocumentFromPath(filePath);
				return document;
			});

		}
		public static bool ProcessDocumentation(MethodWorkerParams m_oWorker, ProcessingOptions options = null) {
			if (options == null)
				options = new ProcessingOptions();
			return DynamicGenerator.Process<AdvancedDocumentationRewriter>(m_oWorker, SelectedOperation.SummaryComment, ("options", options));
		}

		public static bool ProcessOperation(MethodWorkerParams WorkerParams, SelectedOperation operation, ProcessingOptions options = null) {
			return operation switch
			{
				SelectedOperation.MethodImpl => DynamicGenerator.ProcessMethodImpl(WorkerParams),
				SelectedOperation.ColumnAttribute => DynamicGenerator.ProcessColumnAttribut(WorkerParams),
				SelectedOperation.FormatCode => DynamicGenerator.ProcessFormatCode(WorkerParams),
				SelectedOperation.SummaryComment => DynamicGenerator.ProcessDocumentation(WorkerParams, options ?? new ProcessingOptions()),
				_ => false
			};
		}
		public static bool ProcessMethodImpl(MethodWorkerParams m_oWorker) {
			return DynamicGenerator.Process<NoInliningRewriter>(m_oWorker, SelectedOperation.MethodImpl);
		}
		public static bool ProcessColumnAttribut(MethodWorkerParams m_oWorker) {
			return DynamicGenerator.Process<ColumnAttributeRewriter>(m_oWorker, SelectedOperation.ColumnAttribute);
		}
		public static bool ProcessFormatCode(MethodWorkerParams m_oWorker) {

			return DynamicGenerator.Process<FormatCodeRewriter>(m_oWorker, SelectedOperation.FormatCode);
		}

		public static bool Process<T>(MethodWorkerParams m_oWorker, SelectedOperation operation, params (string Name, object Value)[] args) where T : BaseCodeSyntaxRewriter {
			if (m_oWorker == null)
				throw new ArgumentNullException("MethodWorkerParams");
			if (!m_oWorker.ScanDirectoriesFiles())
				return false;
			m_oWorker?.InitialProgressSteps(m_oWorker.CsFiles.Count);

			bool canceled = WithDocumentLoader((loader) => {
				bool isCanceled = false;
				foreach (var documentPath in m_oWorker.CsFiles) {
					if ((m_oWorker.IsCancellationRequested()) || isCanceled) {
						isCanceled = true;
						return true;
					}
					var modified = false;
					var documentName = Path.GetFileName(documentPath);
					m_oWorker?.OnUserStateChanged(documentPath, CodeAnalyzerState.Start);
					try {

						var (newRoot, IsEquivalent) = VisitDocument<T>(documentPath, args);



						if (!IsEquivalent) {

							// Format the document

							var formattedCode = newRoot.GetFormattedCode();
							/*if (operation == SelectedOperation.FormatCode) {
                                var originalCode = File.ReadAllText(documentPath);
                                modified = !formattedCode.MEquals(originalCode);
                            } else {
                                modified = true;
                            }*/
							modified = true;

							if (modified) {
								File.WriteAllText(documentPath, formattedCode);
								m_oWorker?.AddModifiedFile(documentPath);
							}

							//worker.ReportProgress(CalculateProgress(), $"Modified: {documentName}");
						}
						if (!isCanceled) {
							m_oWorker?.OnProgressChanged(1);
							m_oWorker?.OnUserStateChanged(documentPath, modified ? CodeAnalyzerState.Modified : CodeAnalyzerState.NotModified);
							//  if (operation != SelectedOperation.SummaryComment)
							Thread.Sleep(100);
						}
					} catch (Exception ex) {
						m_oWorker?.OnProgressChanged(1);
						m_oWorker?.OnUserStateChanged($"Error in {documentName}: {ex.Message}", CodeAnalyzerState.Error);
					}
				}
				return isCanceled;
			});
			if (!canceled)
				m_oWorker?.OnCompleted((true, "تمت العملية بنجاح"));
			else
				m_oWorker?.OnProcessCanceled();
			return !canceled;
		}
		public static void Process<T>(MethodWorkerParams m_oWorker, Func<SemanticModel, T> func) where T : BaseCodeSyntaxRewriter {
			if (m_oWorker == null)
				throw new ArgumentNullException("MethodWorkerParams");
			if (!m_oWorker.ScanDirectoriesFiles())
				return;
			m_oWorker?.InitialProgressSteps(m_oWorker.CsFiles.Count);
			bool isCanceled = false;
			using (var workspace = MSBuildWorkspace.Create()) {
				foreach (var documentPath in m_oWorker.CsFiles) {
					if ((m_oWorker.IsCancellationRequested()) || isCanceled) {
						isCanceled = true;
						break;
					}
					var modified = false;
					var documentName = Path.GetFileName(documentPath);
					m_oWorker?.OnUserStateChanged(documentPath, CodeAnalyzerState.Start);
					try {
						var originalCode = File.ReadAllText(documentPath);
						var tree = CSharpSyntaxTree.ParseText(originalCode);
						Compilation compilation = CSharpCompilation.Create("Temp")
					.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location)).AddSyntaxTrees(tree);
						SemanticModel semanticModel = compilation.GetSemanticModel(tree);
						//var root = document.GetSyntaxRootAsync().Result;
						//  var semanticModel = document.GetSemanticModelAsync().Result;
						var rewriter = func.Invoke(semanticModel);
						var root = tree.GetRoot();
						var newRoot = rewriter.VisitSyntaxNode(root);

						var IsEquivalent = newRoot.IsEquivalentTo(root);
						if (!IsEquivalent) {
							// Format the document
							var formattedNode = Formatter.Format(newRoot, workspace);
							var formattedCode = formattedNode.ToFullString();
							File.WriteAllText(documentPath, formattedCode);
							m_oWorker?.AddModifiedFile(documentPath);
							modified = true;
							//worker.ReportProgress(CalculateProgress(), $"Modified: {documentName}");
						}
						if (!isCanceled) {
							m_oWorker?.OnProgressChanged(1);
							m_oWorker?.OnUserStateChanged(documentPath, modified ? CodeAnalyzerState.Modified : CodeAnalyzerState.NotModified);
							Thread.Sleep(100);
						}
					} catch (Exception ex) {
						m_oWorker?.OnProgressChanged(1);
						m_oWorker?.OnUserStateChanged($"Error in {documentName}: {ex.Message}", CodeAnalyzerState.Error);
					}
				}
			}
			if (!isCanceled)
				m_oWorker?.OnCompleted((true, "تمت العملية بنجاح"));
			else
				m_oWorker?.OnProcessCanceled();
		}
	}
}
