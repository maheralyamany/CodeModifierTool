namespace CodeModifierTool {
	partial class OperationsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperationsForm));
			this.label1 = new System.Windows.Forms.Label();
			this.txtRootDirectory = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.BtnCancel = new BinarySmartButton.Controls.SmartButton();
			this.BtnProcess = new BinarySmartButton.Controls.SmartButton();
			this.BtnShow = new BinarySmartButton.Controls.SmartButton();
			this.txtPocoEditor = new System.Windows.Forms.RichTextBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.operationGroup = new System.Windows.Forms.GroupBox();
			this.btnCommentOptions = new BinarySmartButton.Controls.SmartButton();
			this.radComment = new System.Windows.Forms.RadioButton();
			this.radFormatCode = new System.Windows.Forms.RadioButton();
			this.radColumnAttribute = new System.Windows.Forms.RadioButton();
			this.radMethodImpl = new System.Windows.Forms.RadioButton();
			this.explorer = new OpetraViews.Controls.FileExplorerTreeView();
			this.progressBar1 = new OpetraViews.Controls.MProgressBar();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.operationGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Droid Sans Arabic", 8.25F);
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(836, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 15);
			this.label1.TabIndex = 29;
			this.label1.Text = "اختر المجلد";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtRootDirectory
			// 
			this.txtRootDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRootDirectory.Location = new System.Drawing.Point(417, 7);
			this.txtRootDirectory.Name = "txtRootDirectory";
			this.txtRootDirectory.Size = new System.Drawing.Size(413, 20);
			this.txtRootDirectory.TabIndex = 30;
			this.txtRootDirectory.TextChanged += new System.EventHandler(this.TxtRootDirectory_TextChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.progressBar1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 404);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(912, 34);
			this.panel1.TabIndex = 32;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.BtnCancel);
			this.panel2.Controls.Add(this.txtRootDirectory);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.BtnProcess);
			this.panel2.Controls.Add(this.BtnShow);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(912, 35);
			this.panel2.TabIndex = 33;
			// 
			// BtnCancel
			// 
			this.BtnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(116)))));
			this.BtnCancel.Enabled = false;
			this.BtnCancel.Location = new System.Drawing.Point(24, 5);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(59, 23);
			this.BtnCancel.TabIndex = 32;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// BtnProcess
			// 
			this.BtnProcess.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(116)))));
			this.BtnProcess.Location = new System.Drawing.Point(89, 6);
			this.BtnProcess.Name = "BtnProcess";
			this.BtnProcess.Size = new System.Drawing.Size(59, 23);
			this.BtnProcess.TabIndex = 31;
			this.BtnProcess.Text = "Process";
			this.BtnProcess.UseVisualStyleBackColor = true;
			this.BtnProcess.Click += new System.EventHandler(this.BtnProcess_Click);
			// 
			// BtnShow
			// 
			this.BtnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnShow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(116)))));
			this.BtnShow.Location = new System.Drawing.Point(352, 6);
			this.BtnShow.Name = "BtnShow";
			this.BtnShow.Size = new System.Drawing.Size(59, 23);
			this.BtnShow.TabIndex = 31;
			this.BtnShow.Text = "عرض";
			this.BtnShow.UseVisualStyleBackColor = true;
			this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
			// 
			// txtPocoEditor
			// 
			this.txtPocoEditor.BackColor = System.Drawing.Color.White;
			this.txtPocoEditor.DetectUrls = false;
			this.txtPocoEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPocoEditor.Font = new System.Drawing.Font("Consolas", 12F);
			this.txtPocoEditor.Location = new System.Drawing.Point(2, 45);
			this.txtPocoEditor.Name = "txtPocoEditor";
			this.txtPocoEditor.ReadOnly = true;
			this.txtPocoEditor.Size = new System.Drawing.Size(677, 322);
			this.txtPocoEditor.TabIndex = 34;
			this.txtPocoEditor.Text = "";
			this.txtPocoEditor.WordWrap = false;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.txtPocoEditor);
			this.panel3.Controls.Add(this.operationGroup);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 35);
			this.panel3.Name = "panel3";
			this.panel3.Padding = new System.Windows.Forms.Padding(2);
			this.panel3.Size = new System.Drawing.Size(681, 369);
			this.panel3.TabIndex = 36;
			// 
			// operationGroup
			// 
			this.operationGroup.Controls.Add(this.btnCommentOptions);
			this.operationGroup.Controls.Add(this.radComment);
			this.operationGroup.Controls.Add(this.radFormatCode);
			this.operationGroup.Controls.Add(this.radColumnAttribute);
			this.operationGroup.Controls.Add(this.radMethodImpl);
			this.operationGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.operationGroup.Location = new System.Drawing.Point(2, 2);
			this.operationGroup.Name = "operationGroup";
			this.operationGroup.Size = new System.Drawing.Size(677, 43);
			this.operationGroup.TabIndex = 36;
			this.operationGroup.TabStop = false;
			this.operationGroup.Text = "Operation";
			// 
			// btnCommentOptions
			// 
			this.btnCommentOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCommentOptions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(116)))));
			this.btnCommentOptions.Location = new System.Drawing.Point(591, 11);
			this.btnCommentOptions.Name = "btnCommentOptions";
			this.btnCommentOptions.Size = new System.Drawing.Size(79, 29);
			this.btnCommentOptions.TabIndex = 2;
			this.btnCommentOptions.Text = "Options";
			this.btnCommentOptions.Visible = false;
			this.btnCommentOptions.Click += new System.EventHandler(this.BtnCommentOptions_Click);
			// 
			// radComment
			// 
			this.radComment.AutoSize = true;
			this.radComment.ForeColor = System.Drawing.Color.Black;
			this.radComment.Location = new System.Drawing.Point(393, 18);
			this.radComment.Name = "radComment";
			this.radComment.Size = new System.Drawing.Size(114, 17);
			this.radComment.TabIndex = 1;
			this.radComment.Tag = "SummaryComment";
			this.radComment.Text = "SummaryComment";
			this.radComment.UseVisualStyleBackColor = true;
			this.radComment.CheckedChanged += new System.EventHandler(this.OperRadioButton_CheckedChanged);
			// 
			// radFormatCode
			// 
			this.radFormatCode.AutoSize = true;
			this.radFormatCode.Checked = true;
			this.radFormatCode.ForeColor = System.Drawing.Color.Black;
			this.radFormatCode.Location = new System.Drawing.Point(287, 18);
			this.radFormatCode.Name = "radFormatCode";
			this.radFormatCode.Size = new System.Drawing.Size(84, 17);
			this.radFormatCode.TabIndex = 1;
			this.radFormatCode.TabStop = true;
			this.radFormatCode.Tag = "FormatCode";
			this.radFormatCode.Text = "FormatCode";
			this.radFormatCode.UseVisualStyleBackColor = true;
			this.radFormatCode.CheckedChanged += new System.EventHandler(this.OperRadioButton_CheckedChanged);
			// 
			// radColumnAttribute
			// 
			this.radColumnAttribute.AutoSize = true;
			this.radColumnAttribute.ForeColor = System.Drawing.Color.Black;
			this.radColumnAttribute.Location = new System.Drawing.Point(142, 18);
			this.radColumnAttribute.Name = "radColumnAttribute";
			this.radColumnAttribute.Size = new System.Drawing.Size(103, 17);
			this.radColumnAttribute.TabIndex = 0;
			this.radColumnAttribute.Tag = "ColumnAttribute";
			this.radColumnAttribute.Text = "ColumnAttribute";
			this.radColumnAttribute.UseVisualStyleBackColor = true;
			this.radColumnAttribute.CheckedChanged += new System.EventHandler(this.OperRadioButton_CheckedChanged);
			// 
			// radMethodImpl
			// 
			this.radMethodImpl.AutoSize = true;
			this.radMethodImpl.ForeColor = System.Drawing.Color.Black;
			this.radMethodImpl.Location = new System.Drawing.Point(39, 18);
			this.radMethodImpl.Name = "radMethodImpl";
			this.radMethodImpl.Size = new System.Drawing.Size(81, 17);
			this.radMethodImpl.TabIndex = 0;
			this.radMethodImpl.Tag = "MethodImpl";
			this.radMethodImpl.Text = "MethodImpl";
			this.radMethodImpl.UseVisualStyleBackColor = true;
			this.radMethodImpl.CheckedChanged += new System.EventHandler(this.OperRadioButton_CheckedChanged);
			// 
			// explorer
			// 
			this.explorer.Dock = System.Windows.Forms.DockStyle.Right;
			this.explorer.ExcludedExtensions = ".Designer.cs";
			this.explorer.ExcludedFolders = resources.GetString("explorer.ExcludedFolders");
			this.explorer.FilterFilesExtensions = "cs";
			this.explorer.Location = new System.Drawing.Point(681, 35);
			this.explorer.MaxDirectoryLevel = 4;
			this.explorer.Name = "explorer";
			this.explorer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.explorer.RightToLeftLayout = true;
			this.explorer.RootPath = "";
			this.explorer.Size = new System.Drawing.Size(231, 369);
			this.explorer.TabIndex = 35;
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.DisplayMode = OpetraViews.Controls.ProgressTextDisplayMode.ValueOverMaximum;
			this.progressBar1.Location = new System.Drawing.Point(5, 7);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.progressBar1.Size = new System.Drawing.Size(902, 20);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 25;
			// 
			// OperationsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(185)))), ((int)(((byte)(240)))));
			this.ClientSize = new System.Drawing.Size(912, 438);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.explorer);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "OperationsForm";
			this.Text = "Form1";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.operationGroup.ResumeLayout(false);
			this.operationGroup.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtRootDirectory;
		private BinarySmartButton.Controls.SmartButton BtnShow;
		private BinarySmartButton.Controls.SmartButton BtnProcess;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RichTextBox txtPocoEditor;
		private OpetraViews.Controls.MProgressBar progressBar1;
		private OpetraViews.Controls.FileExplorerTreeView explorer;
		private BinarySmartButton.Controls.SmartButton BtnCancel;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.GroupBox operationGroup;
		private System.Windows.Forms.RadioButton radMethodImpl;
		private System.Windows.Forms.RadioButton radColumnAttribute;
		private System.Windows.Forms.RadioButton radFormatCode;
		private System.Windows.Forms.RadioButton radComment;
		private BinarySmartButton.Controls.SmartButton btnCommentOptions;
	}
}
