using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace CodeModifierTool {
	public partial class SummaryCommentForm : Form, ICodeAnalyzerProgress {


		private ProcessingOptions options = new ProcessingOptions();
		private MethodWorkerParams WorkerParams;


		public SummaryCommentForm() {
			InitializeComponent();
			WorkerParams = new MethodWorkerParams(this);
			/*Rectangle bounds = Screen.FromHandle(base.Handle).WorkingArea;
            base.MaximizedBounds = GetMaximizedBounds(bounds);
            this.WindowState = FormWindowState.Maximized;*/
		}
		public Rectangle GetMaximizedBounds(Rectangle bounds = default(Rectangle)) {
			if (bounds.IsEmpty) {
				bounds = Screen.FromHandle(base.Handle).WorkingArea;
			}

			int num = 1;
			return new Rectangle(bounds.X + num, bounds.Y + num, bounds.Width - num, bounds.Height - num);
		}
		private void BrowseButton_Click(object sender, EventArgs e) {
			using (CommonOpenFileDialog cfd = new CommonOpenFileDialog()) {
				cfd.Title = "إختار مسار المشروع";
				cfd.IsFolderPicker = true;
				cfd.Multiselect = false;
				cfd.AllowNonFileSystemItems = true;
				cfd.InitialDirectory = txtRootDirectory.Text;
				if (cfd.ShowDialog(this.Handle) == CommonFileDialogResult.Ok) {
					var sP = cfd.FileName;
					OnUserStateChanged("");
					txtRootDirectory.Text = sP;
				}
			}


			/*using (OpenFileDialog dlg = new OpenFileDialog()) {
                dlg.Filter = "Solution Files (*.sln)|*.sln";
                if (dlg.ShowDialog() == DialogResult.OK) {
                    txtRootDirectory.Text = dlg.FileName;
                }
            }*/
		}

		private void ProcessButton_Click(object sender, EventArgs e) {
			StartProcessing();

		}

		private void CancelButton_Click(object sender, EventArgs e) {

			if (WorkerParams.GetIsRunning() && !WorkerParams.CancellationPending) {
				if (Toast.Show("هل تريد تأكيد إلغاء العملية؟", "تأكيد", MessageBoxButtons.YesNo) == DialogResult.Yes) {
					SetStatus("Cancelling...");
					WorkerParams.Cancel();
				}
			}
		}
		private string rootDirectory = "";
		private void TxtRootDirectory_TextChanged(object sender, EventArgs e) {
			rootDirectory = txtRootDirectory.Text;
			if (!string.IsNullOrWhiteSpace(rootDirectory) && !Directory.Exists(rootDirectory)) {
				Toast.Show("المسار غير صالح");
				return;
			}


		}
		private void StartProcessing() {
			var solutionPath = txtRootDirectory.Text;
			if (string.IsNullOrEmpty(solutionPath) || !Directory.Exists(solutionPath)) {
				MessageBox.Show("Please select a valid solution file first.", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			WorkerParams = new MethodWorkerParams(this) {
				Directories = new List<string>() { solutionPath }
			};

			// Get options from UI
			options = new ProcessingOptions {
				AddClassSummaries = classesCheck.Checked,
				AddMethodSummaries = methodsCheck.Checked,
				AddPropertySummaries = propertiesCheck.Checked,
				AddParameterDocs = parametersCheck.Checked,
				AddReturnDocs = returnsCheck.Checked,
				SkipExistingDocumentation = skipExistingCheck.Checked,
				SkipGeneratedFiles = skipGeneratedCheck.Checked,
				GenerateSmartComments = useSmartComments.Checked,
				/*MethodTemplate = methodTemplate.Text,
                ParameterTemplate = paramTemplate.Text,
                ReturnTemplate = returnTemplate.Text*/
			};
			// Start processing
			var backgroundThread = new Thread(
				new ThreadStart(() => {
					StartProcess(options);
				}
			));
			backgroundThread.Start();



		}
		public void SetControlsEnable(bool state) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new MethodInvoker<bool>(this.SetControlsEnable), state);
			} else {
				txtRootDirectory.Enabled = state;
				browseButton.Enabled = processButton.Enabled = state;
				cancelButton.Enabled = !state;
			}
		}
		private void StartProcess(ProcessingOptions options) {

			SetControlsEnable(false);
			WorkerParams.SetIsRunning(true);
			InitialProgressSteps(100);
			// Reset state

			SetProgress(0);
			SetStatus("Initializing...");
			OnUserStateChanged("");
			DynamicGenerator.ProcessDocumentation(WorkerParams, options);

			//DynamicCommentGenerator.Process(WorkerParams, options);
		}



		public void InitialProgressSteps(int stepsCount) {
			if (progressBar.InvokeRequired) {
				progressBar.BeginInvoke(
					new Action(() => {
						progressBar.Value = 0;
						progressBar.Maximum = stepsCount;
					}
				));
			} else {
				progressBar.Value = 0;
				progressBar.Maximum = stepsCount;
			}
		}

		public void OnProgressChanged(int progressPercentage) {
			SetProgress(progressPercentage);
		}
		private void SetProgress(int value) {

			SafeInvoke(progressBar, () => {

				if (value + progressBar.Value <= progressBar.Maximum)
					progressBar.Value += value;


			});
		}

		public static readonly Color SuccColor = Color.FromArgb(0, 0, 255);

		public static readonly Color ErrorColor = Color.FromArgb(255, 0, 0);
		public void OnUserStateChanged(string message, CodeAnalyzerState state = CodeAnalyzerState.None) {

			void AppendText(string text, Color color) {
				if (string.IsNullOrEmpty(text) == false) {
					txtPocoEditor.Select(txtPocoEditor.TextLength, 0);
					txtPocoEditor.SelectionColor = color;
					txtPocoEditor.SelectedText = text;
					txtPocoEditor.SelectionColor = txtPocoEditor.ForeColor;
				}
			}
			void Write() {
				if (string.IsNullOrEmpty(message)) {
					txtPocoEditor.Clear();
				} else {
					if (state.Equals(CodeAnalyzerState.Start)) {
						txtPocoEditor.AppendText(Environment.NewLine);
						txtPocoEditor.AppendText(Environment.NewLine);
						var fileName = message.Replace(rootDirectory, "");
						txtPocoEditor.AppendText($"File : {fileName} , State : ");
					} else if (state.Equals(CodeAnalyzerState.Modified) || state.Equals(CodeAnalyzerState.NotModified)) {
						var modified = state.Equals(CodeAnalyzerState.Modified);
						AppendText(modified ? "Modified" : "Not modified", modified ? SuccColor : ErrorColor);
					}
				}
				txtPocoEditor.Refresh();
			}
			if (txtPocoEditor.InvokeRequired) {
				txtPocoEditor.BeginInvoke(
				new Action(() => {
					Write();
				}
					));
			} else {
				Write();
			}

		}
		public void OnProcessCanceled() {
			WorkerParams.Dispose();

			SetControlsEnable(true);
			Toast.Show("تم الغاء العملية");
		}
		public void OnCompleted((bool State, string Msge) result) {

			var rest = progressBar.Maximum - progressBar.Value;

			if (rest > 0)
				OnProgressChanged(rest);
			WorkerParams.Dispose();

			SetControlsEnable(true);
			Toast.Show(result.Msge);
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			if (WorkerParams.GetIsRunning()) {
				e.Cancel = true;
			}
			base.OnFormClosing(e);
		}


		public void SafeInvoke(Control control, Action action) {
			try {
				if (control.InvokeRequired) {
					control.BeginInvoke(
					new Action(() => {
						action();
					}
						));
				} else {
					action();
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
		private void SetStatus(string message) {
			SafeInvoke(statusLabel, () => {
				statusLabel.Text = message;
				statusLabel.Refresh();
			});
		}

	}
}
