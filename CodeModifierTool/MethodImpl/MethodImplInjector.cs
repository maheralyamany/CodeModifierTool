using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using CodeModifierTool;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;

public static class MethodImplInjector {




	public static void Process(MethodWorkerParams m_oWorker) {

		DynamicGenerator.ProcessMethodImpl(m_oWorker);
		/*
                if (m_oWorker == null)
                    throw new ArgumentNullException("MethodWorkerParams");

                if (m_oWorker.CsFiles.Count == 0) {
                    if (m_oWorker.Directories.Count == 0) {
                        m_oWorker?.OnCompleted((false, "لايوجد ملفات لمعالجتها"));
                        return;
                    }
                    var fileHelper = new ProjectFileHelper() {
                        BlockedFolders = m_oWorker.BlockedFolders,
                        ExcludedExtensions = m_oWorker.ExcludedExtensions
                    };
                    foreach (var rootDirectory in m_oWorker.Directories) {
                        var csFiles = fileHelper.DirectoryFiles(rootDirectory);
                        m_oWorker.CsFiles.AddListRange(csFiles);
                    }
                    m_oWorker.CsFiles = m_oWorker.CsFiles.LDistinct();
                }


                //ProcessAllVisitor(m_oWorker);
                ProcessAllFiles(m_oWorker);*/

	}


	public static void ProcessAllVisitor(MethodWorkerParams m_oWorker) {
		if (m_oWorker == null)
			throw new ArgumentNullException("MethodWorkerParams");
		if (m_oWorker.CsFiles.Count == 0) {
			m_oWorker?.OnCompleted((false, "لايوجد ملفات لمعالجتها"));
			return;
		}
		var attr = "[MethodImpl(MethodImplOptions.NoInlining)]";
		m_oWorker?.InitialProgressSteps(m_oWorker.CsFiles.Count);
		bool isCanceled = false;
		foreach (var file in m_oWorker.CsFiles) {
			if ((m_oWorker.IsCancellationRequested()) || isCanceled) {
				isCanceled = true;
				break;
			}
			bool hasUsing = false;
			var output = new List<string>();
			var modified = false;
			m_oWorker?.OnUserStateChanged(file, CodeAnalyzerState.Start);
			var originalCode = File.ReadAllText(file);
			var tree = CSharpSyntaxTree.ParseText(originalCode);
			var root = tree.GetRoot();
			var rewriter = new NoInliningVisitor(null);
			var newRoot = rewriter.Visit(root);
			var IsEquivalent = newRoot.IsEquivalentTo(root);
			var syntaxModels = rewriter.GetCodeSyntaxes();
			var syntaxMethods = syntaxModels.LWhere(s => s.NodeType == SyntaxNodeType.Method && s.IsTopLevel && !s.HasAttribute);
			var syntaxPropertys = syntaxModels.LWhere(s => s.NodeType == SyntaxNodeType.Property && s.IsTopLevel && !s.HasAttribute);
			var syntaxConstructors = syntaxModels.LWhere(s => s.NodeType == SyntaxNodeType.Constructor && s.IsTopLevel && !s.HasAttribute);
			var list = File.ReadAllLines(file);
			/*list.LSelect((line, i) => {
                var code = syntaxMethods.FirstOrDefault(m => line.StartsWith(m.FullDeclaration));
                return line

            })*/
			int braceDepth = 0;
			bool insideProperty = false;
			for (int i = 0; i < list.Length; i++) {
				var line = list[i];
				var trimmed = line.Trim();
				if (!hasUsing && Regex.IsMatch(trimmed, @"using\s+System\.Runtime\.CompilerServices\s*;"))
					hasUsing = true;
				var constructor = syntaxConstructors.FirstOrDefault(m => line.StartsWith(m.FullDeclaration));
				if (constructor != null) {
					string indent = BaseCodeAnalyzer.GetIndentation(line);
					output.Add($"{indent}{attr}");
					modified = true;
					output.Add(line);
					continue;
				}
				var method = syntaxMethods.FirstOrDefault(m => line.StartsWith(m.FullDeclaration));
				if (method != null) {
					string indent = BaseCodeAnalyzer.GetIndentation(line);
					output.Add($"{indent}{attr}");
					modified = true;
					output.Add(line);
					continue;
				}


				var property = syntaxPropertys.FirstOrDefault(m => line.StartsWith(m.FullDeclaration));
				if (property != null) {
					insideProperty = true;
					braceDepth = CountChar(trimmed, '{') - CountChar(trimmed, '}');


					/*string indent = BaseCodeAnalyzer.GetIndentation(line);
                    output.Add($"{indent}{attr}");
                    modified = true;
                    output.Add(line);
                    continue;*/
				}

				if (insideProperty && (trimmed.StartsWith("get ") || trimmed.StartsWith("set "))) {
					if (!BaseCodeAnalyzer.IsPreviousLineAttribute(output, "MethodImpl") &&
						(trimmed.Contains("{") || ((i + 1) < list.Length && list[i + 1].Contains("{")))) {
						string indent = BaseCodeAnalyzer.GetIndentation(line);
						output.Add($"{indent}{attr}");
						modified = true;
					}
				}
				output.Add(line);
				if (insideProperty) {
					braceDepth += CountChar(trimmed, '{');
					braceDepth -= CountChar(trimmed, '}');
					if (braceDepth <= 0)
						insideProperty = false;
				}
			}

			if (modified && !hasUsing /*!originalText.Contains("System.Runtime.CompilerServices")*/) {
				output.Insert(0, "using System.Runtime.CompilerServices;\r\n");
			}
			if (modified) {
				m_oWorker?.AddModifiedFile(file);
				File.WriteAllLines(file, output);
				//Console.WriteLine($"✅ Modified: {file}");
			}

			if (!isCanceled) {
				m_oWorker?.OnProgressChanged(1);
				m_oWorker?.OnUserStateChanged(file, modified ? CodeAnalyzerState.Modified : CodeAnalyzerState.NotModified);
				Thread.Sleep(100);
			}
		}
		if (!isCanceled)
			m_oWorker?.OnCompleted((true, "تمت العملية بنجاح"));
		else
			m_oWorker?.OnProcessCanceled();
	}
	public static void ProcessAllFiles(MethodWorkerParams m_oWorker) {
		if (m_oWorker == null)
			throw new ArgumentNullException("MethodWorkerParams");
		if (m_oWorker.CsFiles.Count == 0) {
			m_oWorker?.OnCompleted((false, "لايوجد ملفات لمعالجتها"));
			return;
		}
		m_oWorker?.InitialProgressSteps(m_oWorker.CsFiles.Count);
		bool isCanceled = false;
		using (var workspace = MSBuildWorkspace.Create()) {
			foreach (var file in m_oWorker.CsFiles) {
				if ((m_oWorker.IsCancellationRequested()) || isCanceled) {
					isCanceled = true;
					break;
				}
				var modified = false;
				m_oWorker?.OnUserStateChanged(file, CodeAnalyzerState.Start);
				var originalCode = File.ReadAllText(file);
				var tree = CSharpSyntaxTree.ParseText(originalCode);
				var root = tree.GetRoot();
				var rewriter = new NoInliningRewriter(null);
				var newRoot = rewriter.VisitSyntaxNode(root);
				//bool hasUsing = true;
				bool formatCode = false;

				if (!newRoot.IsEquivalentTo(root)) {
					if (!originalCode.Contains("System.Runtime.CompilerServices")) {
						//hasUsing = false;
						var cu = newRoot as Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax;
						newRoot = cu.AddUsings(
							SyntaxFactory.UsingDirective(
								SyntaxFactory.ParseName(" System.Runtime.CompilerServices")));
					}
					modified = true;
					var formattedNode = Formatter.Format(newRoot, workspace);
					var formattedCode = formattedNode.ToFullString();
					/*  string backupPath = filePath + ".bak";
                        File.Copy(filePath, backupPath, true);*/
					/*  if (!hasUsing)
                            newCode = "using System.Runtime.CompilerServices;\r\n" + newCode;*/
					File.WriteAllText(file, formattedCode);
					if (formatCode) {
						var list = File.ReadAllLines(file);
						var output = new List<string>();
						var attr = "[MethodImpl(MethodImplOptions.NoInlining)]";
						for (int index = 0; index < list.Length; index++) {
							var line = list[index];
							var trimdline = line.Trim();
							if (trimdline.Contains(attr)) {
								//"\t\tpublic bool Checked { [MethodImpl(MethodImplOptions.NoInlining)]"
								if (!trimdline.StartsWith(attr)) {
									string indent1 = BaseCodeAnalyzer.GetIndentation(line);
									if (indent1.Length == 0)
										indent1 = "\t\t";
									var tt = line.Replace(attr, "");
									output.Add(tt);
									line = $"{indent1} {attr}";
								}
								var nextIndex = index + 1;
								if (nextIndex < list.Length - 1) {
									var nextLine = list[nextIndex];
									string nindent = BaseCodeAnalyzer.GetIndentation(nextLine);
									if (nindent.Length == 0) {
										string indent = BaseCodeAnalyzer.GetIndentation(line);
										list[nextIndex] = indent + nextLine;
									}
								}
							}
							output.Add(line);
						}
						File.WriteAllLines(file, output);
					}
					m_oWorker?.AddModifiedFile(file);
					//
				}
				if (!isCanceled) {
					m_oWorker?.OnProgressChanged(1);
					m_oWorker?.OnUserStateChanged(file, modified ? CodeAnalyzerState.Modified : CodeAnalyzerState.NotModified);
					Thread.Sleep(100);
				}
			}
		}

		if (!isCanceled)
			m_oWorker?.OnCompleted((true, "تمت العملية بنجاح"));
		else
			m_oWorker?.OnProcessCanceled();
	}

	public static void ProcessAllFilesOld(MethodWorkerParams m_oWorker) {
		if (m_oWorker == null)
			throw new ArgumentNullException("MethodWorkerParams");
		if (m_oWorker.CsFiles.Count == 0) {
			m_oWorker?.OnCompleted((false, "لايوجد ملفات لمعالجتها"));
			return;
		}
		m_oWorker?.InitialProgressSteps(m_oWorker.CsFiles.Count);
		bool isCanceled = false;
		foreach (var file in m_oWorker.CsFiles) {
			if ((m_oWorker.IsCancellationRequested()) || isCanceled) {
				isCanceled = true;
				break;
			}
			m_oWorker?.OnUserStateChanged(file, CodeAnalyzerState.Start);
			//var lines = ;
			bool inBlockComment = false;
			var list = File.ReadAllLines(file).LSelect((line, index) => (new CodeLinesModel { Line = line, Index = index, IsCommentd = false }).CheckHasComment(ref inBlockComment).InitAll());
			var output = new List<string>();
			bool modified = false;
			bool insideProperty = false;
			bool hasUsing = false;
			int braceDepth = 0;
			MethodInfo methodInfo = null;
			foreach (var code in list) {
				if ((m_oWorker.IsCancellationRequested()) || isCanceled) {
					isCanceled = true;
					modified = false;
					break;
				}
				string line = code.Line;
				if (code.IsInValidLine) {
					output.Add(line);
					continue;
				}
				int index = code.Index;
				string trimmed = code.GetTrimmedLine();
				if (!hasUsing && Regex.IsMatch(trimmed, @"using\s+System\.Runtime\.CompilerServices\s*;"))
					hasUsing = true;
				// Detect class name (for constructor match)
				// ---------- [1] Constructor ----------
				if (code.IsConstructor()) {
					methodInfo = null;
					if (!BaseCodeAnalyzer.IsPreviousLineAttribute(output, "MethodImpl")) {
						string indent = BaseCodeAnalyzer.GetIndentation(line);
						output.Add($" {indent}");
						modified = true;
					}
					output.Add(line);
					continue;
				}
				var IsEndIndex = methodInfo?.IsEndIndex(index) ?? false;
				var IsNested = false;
				var methodMatch = code.MethodMatchDefinition();
				// ---------- [2] Method ----------
				if (methodMatch.Success) {
					var methodName = methodMatch.Groups["name"].Value;
					IsNested = (methodInfo != null && methodInfo.HasNestedMethod(methodName, index));
					if (methodInfo == null || (!IsNested || IsEndIndex)) {
						methodInfo = GetMethodInfo(list, methodName, index);
						if (methodInfo.IsNested)
							methodInfo = null;
					}
				} else if (IsEndIndex) {
					methodInfo = null;
				}
				if ((methodInfo != null && methodInfo.StartIndex == index && !IsNested) && !BaseCodeAnalyzer.IsPreviousLineAttribute(output, "MethodImpl")) {
					string indent = BaseCodeAnalyzer.GetIndentation(line);
					output.Add($" {indent}");
					modified = true;
				}
				// ---------- [3] Property with body ----------
				if (code.IsPropertyDefinition()) {
					insideProperty = true;
					braceDepth = CountChar(trimmed, '{') - CountChar(trimmed, '}');
				}
				if (insideProperty && (trimmed.StartsWith("get ") || trimmed.StartsWith("set "))) {
					if (!BaseCodeAnalyzer.IsPreviousLineAttribute(output, "MethodImpl") &&
						(trimmed.Contains("{") || (code.Index + 1 < list.Count && list[code.Index + 1].Line.Contains("{")))) {
						string indent = BaseCodeAnalyzer.GetIndentation(line);
						output.Add($" {indent}");
						modified = true;
					}
				}
				output.Add(line);
				if (insideProperty) {
					braceDepth += CountChar(trimmed, '{');
					braceDepth -= CountChar(trimmed, '}');
					if (braceDepth <= 0)
						insideProperty = false;
				}
			}
			// ---------- Add using if needed ----------
			//string originalText = File.ReadAllText(file);
			if (modified && !hasUsing /*!originalText.Contains("System.Runtime.CompilerServices")*/) {
				output.Insert(0, "using System.Runtime.CompilerServices;\r\n");
			}
			if (modified) {
				m_oWorker?.AddModifiedFile(file);
				File.WriteAllLines(file, output);
				//Console.WriteLine($"✅ Modified: {file}");
			}
			if (!isCanceled) {
				m_oWorker?.OnProgressChanged(1);
				m_oWorker?.OnUserStateChanged(file, modified ? CodeAnalyzerState.Modified : CodeAnalyzerState.NotModified);
			}
		}
		if (!isCanceled)
			m_oWorker?.OnCompleted((true, "تمت العملية بنجاح"));
		else
			m_oWorker?.OnProcessCanceled();
	}

	/// <summary>
	/// Gets the method information.
	/// </summary>
	/// <param name="lines">The lines.</param>
	/// <param name="name">The name.</param>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	/// <autogeneratedoc />
	private static MethodInfo GetMethodInfo(List<CodeLinesModel> lines, string name, int index) {
		int braceDepth = 0;
		int startIndex = index;
		int endIndex = -1;
		List<MethodInfo> nestedMethods = new List<MethodInfo>();
		for (int i = index; i < lines.Count; i++) {
			var code = lines[i];
			string trimmed = code.Line.Trim();
			if (code.IsInValidLine) {
				continue;
			}
			braceDepth += CountChar(trimmed, '{');
			braceDepth -= CountChar(trimmed, '}');
			if (code.Index != index) {
				var methodMatch = code.MethodMatchDefinition();
				if (methodMatch.Success) {
					var methodName = methodMatch.Groups["name"].Value;
					if (!methodName.MEquals(name)) {
						var info = GetMethodInfo(lines, methodName, code.Index);
						info.IsNested = true;
						nestedMethods.Add(info);
					}
				}
			}
			if (braceDepth == 0) {
				endIndex = code.Index;
				break;
			}
		}
		if (nestedMethods.Count > 0) {
			var nendIndex = nestedMethods.Last().EndIndex;
			if (endIndex < nendIndex)
				endIndex = nendIndex;
		}
		var methodInfo = new MethodInfo(name, startIndex, endIndex) { NestedMethods = nestedMethods };
		return methodInfo;
	}



	static int CountChar(string input, params char[] ch) {
		int count = 0;
		foreach (char c in input)
			if (ch.Any(g => g == c)) count++;
		return count;
	}
}
