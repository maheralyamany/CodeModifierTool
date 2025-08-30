using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
namespace CodeModifierTool {
    public partial class OperationsForm : Form, ICodeAnalyzerProgress {
        private SelectedOperation selectedOperation = SelectedOperation.None;
        private MethodWorkerParams WorkerParams;
        private ProcessingOptions options = new ProcessingOptions();
        public OperationsForm() {
            InitializeComponent();
            WorkerParams = new MethodWorkerParams(this);
            Rectangle bounds = Screen.FromHandle(base.Handle).WorkingArea;
            base.MaximizedBounds = GetMaximizedBounds(bounds);
            this.WindowState = FormWindowState.Maximized;
        }
        public Rectangle GetMaximizedBounds(Rectangle bounds = default(Rectangle)) {
            if (bounds.IsEmpty) {
                bounds = Screen.FromHandle(base.Handle).WorkingArea;
            }
            int num = 1;
            return new Rectangle(bounds.X + num, bounds.Y + num, bounds.Width - num, bounds.Height - num);
        }
        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            //txtRootDirectory.Text = defSolutionPath;

        }
        private string rootDirectory = "";
        private string defSolutionPath = @"D:\OMaxSystem\M10_08_2022\OpetraERPSys\OpetraProErpSys.sln";
        private Thread backgroundThread;
        private void TxtRootDirectory_TextChanged(object sender, EventArgs e) {
            var solutionPath = txtRootDirectory.Text;
            rootDirectory = !string.IsNullOrWhiteSpace(solutionPath) ? Path.GetDirectoryName(solutionPath) : "";

            if (string.IsNullOrWhiteSpace(rootDirectory) || !Directory.Exists(rootDirectory)) {
                Toast.Show("المسار غير صالح");
                txtRootDirectory.Text = defSolutionPath;
                return;
            }
            var Projects = DocumentLoader.LoadSolutionProjectsNamesCore(solutionPath);
            var directories = Directory.GetDirectories(rootDirectory).LSelectWhere(d => new DirectoryInfo(d).Name, d => {

                return !Projects.Any(s => s.Directory == d);
            });
            explorer.SetExcludedFoldersList(directories);


            explorer.RootPath = rootDirectory;
        }
        private void BtnProcess_Click(object sender, EventArgs e) {
            GetSelectedOperation();
            //
            if (selectedOperation == SelectedOperation.None) {
                Toast.Show("يجب تحديد العملية");
                return;
            }
            var checkedNodes = explorer.GetCheckedFileTreeNodes();
            if (checkedNodes.Count == 0) {
                Toast.Show("يجب تحديد على الاقل مجلد");
                return;
            }
            var csFiles = checkedNodes.LWhereSelect(s => s != null && s.IsFileType, s => s.Path);
            WorkerParams = new MethodWorkerParams(this) {
                CsFiles = csFiles
            };
            if (csFiles.Count == 0) {
                var checkedFolders = checkedNodes.LWhere(s => s != null && !s.IsFileType).ToMDictionary(s => s.Index, s => s);
                List<string> directories = new List<string>();
                foreach (var p in checkedFolders) {
                    if (!checkedFolders.ContainsKey(p.Value.ParentId)) {
                        directories.Add(p.Value.Path);
                    }
                }
                //var checkedDirectories = checkedFolders.LWhereSelect(s => !s.IsFileType, s => s.Path);
                WorkerParams.Directories = directories;
            }
            backgroundThread = new Thread(
                   new ThreadStart(delegate { StartProcess(); })) {
                IsBackground = true
            };
            backgroundThread.SetApartmentState(ApartmentState.STA);
            backgroundThread.Start();
        }
        private void GetSelectedOperation() {
            selectedOperation = SelectedOperation.None;
            try {
                var chkRad = operationGroup.Controls.OfType<RadioButton>().FirstOrDefault(s => s.Checked);
                if (chkRad != null) {
                    var tag = chkRad.Tag.ToNullString();
                    if (!tag.IsNullOrEmpty()) {
                        selectedOperation = tag switch
                        {
                            "MethodImpl" => SelectedOperation.MethodImpl,
                            "SummaryComment" => SelectedOperation.SummaryComment,
                            "ColumnAttribute" => SelectedOperation.ColumnAttribute,
                            "FormatCode" => SelectedOperation.FormatCode,
                            _ => SelectedOperation.None,
                        };
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
        private void StartProcess() {
            SetControlsEnable(false);
            WorkerParams.SetIsRunning(true);
            InitialProgressSteps(100);
            OnUserStateChanged("");
            DynamicGenerator.ProcessOperation(WorkerParams, selectedOperation, this.options);
        }
        public void SetControlsEnable(bool state) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new MethodInvoker<bool>(this.SetControlsEnable), state);
            } else {
                txtRootDirectory.Enabled = state;
                BtnShow.Enabled = BtnProcess.Enabled = state;
                BtnCancel.Enabled = !state;
            }
        }
        private void BtnShow_Click(object sender, EventArgs e) {
            /*using (CommonOpenFileDialog cfd = new CommonOpenFileDialog()) {
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
            }*/

            using (OpenFileDialog dlg = new OpenFileDialog()) {
                dlg.Filter = "Solution Files (*.sln)|*.sln";
                var path = txtRootDirectory.Text;
                //dlg.InitialDirectory = Path.GetDirectoryName(path);
                dlg.FileName = path;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    OnUserStateChanged("");
                    txtRootDirectory.Text = dlg.FileName;
                }
            }
        }
        public void InitialProgressSteps(int stepsCount) {
            if (progressBar1.InvokeRequired) {
                progressBar1.BeginInvoke(
                    new Action(() => {
                        progressBar1.Value = 0;
                        progressBar1.Maximum = stepsCount;
                    }
                ));
            } else {
                progressBar1.Value = 0;
                progressBar1.Maximum = stepsCount;
            }
        }
        public void OnProgressChanged(int progressPercentage) {
            if (progressBar1.InvokeRequired) {
                progressBar1.BeginInvoke(
                    new Action(() => {
                        SetProgressValue(progressPercentage);
                    }
                ));
            } else {
                SetProgressValue(progressPercentage);
            }
        }
        private void SetProgressValue(int progressPercentage) {
            if (progressPercentage + progressBar1.Value <= progressBar1.Maximum)
                progressBar1.Value += progressPercentage;
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
                txtPocoEditor.ScrollToCaret();
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
            AbortThread();
            Toast.Show("تم الغاء العملية");
        }
        public void AbortThread() {
            try {
                if (backgroundThread != null && backgroundThread.IsAlive)
                    backgroundThread.Abort();
                backgroundThread = null;
            } catch (Exception ex) {
                backgroundThread = null;
                Console.WriteLine(ex.Message);
            }
        }
        public void OnCompleted((bool State, string Msge) result) {
            var rest = progressBar1.Maximum - progressBar1.Value;
            if (rest > 0)
                OnProgressChanged(rest);
            WorkerParams.Dispose();
            SetControlsEnable(true);
            AbortThread();
            Toast.Show(result.Msge);
        }
        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (WorkerParams.GetIsRunning()) {
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
        private void BtnCancel_Click(object sender, EventArgs e) {
            if (WorkerParams.GetIsRunning() && !WorkerParams.CancellationPending) {
                if (Toast.Show("هل تريد تأكيد إلغاء العملية؟", "تأكيد", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    WorkerParams.Cancel();
            }
        }
        private void OperRadioButton_CheckedChanged(object sender, EventArgs e) {
            var btn = (RadioButton)sender;
            /*
            if (btn.Checked) {
                //selectedOperation = SelectedOperation.None;
                if (btn == radColumnAttribute)
                    selectedOperation = SelectedOperation.ColumnAttribute;
                else if (btn == radMethodImpl)
                    selectedOperation = SelectedOperation.MethodImpl;
            }*/
            btnCommentOptions.Visible = btn == radComment;
        }

        private void BtnCommentOptions_Click(object sender, EventArgs e) {
            //
            CommentOptionsForm form = new CommentOptionsForm(options, (o) => options = o);
            form.ShowDialog(this);
        }
    }
    public delegate void MethodInvoker<T>(T parameter1);
}
