namespace CodeModifierTool {
	partial class SummaryCommentForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SummaryCommentForm));
			this.solutionGroup = new System.Windows.Forms.GroupBox();
			this.txtRootDirectory = new System.Windows.Forms.TextBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.optionsGroup = new System.Windows.Forms.GroupBox();
			this.classesCheck = new System.Windows.Forms.CheckBox();
			this.methodsCheck = new System.Windows.Forms.CheckBox();
			this.parametersCheck = new System.Windows.Forms.CheckBox();
			this.returnsCheck = new System.Windows.Forms.CheckBox();
			this.propertiesCheck = new System.Windows.Forms.CheckBox();
			this.skipExistingCheck = new System.Windows.Forms.CheckBox();
			this.skipGeneratedCheck = new System.Windows.Forms.CheckBox();
			this.useSmartComments = new System.Windows.Forms.CheckBox();
			this.templatesGroup = new System.Windows.Forms.GroupBox();
			this.methodTemplateLabel = new System.Windows.Forms.Label();
			this.methodTemplate = new System.Windows.Forms.TextBox();
			this.paramTemplateLabel = new System.Windows.Forms.Label();
			this.paramTemplate = new System.Windows.Forms.TextBox();
			this.returnTemplateLabel = new System.Windows.Forms.Label();
			this.returnTemplate = new System.Windows.Forms.TextBox();
			this.progressGroup = new System.Windows.Forms.GroupBox();
			this.txtPocoEditor = new System.Windows.Forms.RichTextBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.statusLabel = new System.Windows.Forms.Label();
			this.processButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.solutionGroup.SuspendLayout();
			this.optionsGroup.SuspendLayout();
			this.templatesGroup.SuspendLayout();
			this.progressGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// solutionGroup
			// 
			this.solutionGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
			this.solutionGroup.Controls.Add(this.txtRootDirectory);
			this.solutionGroup.Controls.Add(this.browseButton);
			this.solutionGroup.ForeColor = System.Drawing.Color.LightSkyBlue;
			this.solutionGroup.Location = new System.Drawing.Point(20, 4);
			this.solutionGroup.Name = "solutionGroup";
			this.solutionGroup.Size = new System.Drawing.Size(860, 56);
			this.solutionGroup.TabIndex = 0;
			this.solutionGroup.TabStop = false;
			this.solutionGroup.Text = "Solution File";
			// 
			// txtRootDirectory
			// 
			this.txtRootDirectory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(74)))));
			this.txtRootDirectory.ForeColor = System.Drawing.Color.White;
			this.txtRootDirectory.Location = new System.Drawing.Point(20, 20);
			this.txtRootDirectory.Name = "txtRootDirectory";
			this.txtRootDirectory.ReadOnly = true;
			this.txtRootDirectory.Size = new System.Drawing.Size(700, 20);
			this.txtRootDirectory.TabIndex = 0;
			this.txtRootDirectory.TextChanged += new System.EventHandler(this.TxtRootDirectory_TextChanged);
			// 
			// browseButton
			// 
			this.browseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.browseButton.FlatAppearance.BorderSize = 0;
			this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.browseButton.ForeColor = System.Drawing.Color.White;
			this.browseButton.Location = new System.Drawing.Point(730, 19);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(110, 22);
			this.browseButton.TabIndex = 1;
			this.browseButton.Text = "Browse...";
			this.browseButton.UseVisualStyleBackColor = false;
			this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// optionsGroup
			// 
			this.optionsGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
			this.optionsGroup.Controls.Add(this.classesCheck);
			this.optionsGroup.Controls.Add(this.methodsCheck);
			this.optionsGroup.Controls.Add(this.parametersCheck);
			this.optionsGroup.Controls.Add(this.returnsCheck);
			this.optionsGroup.Controls.Add(this.propertiesCheck);
			this.optionsGroup.Controls.Add(this.skipExistingCheck);
			this.optionsGroup.Controls.Add(this.skipGeneratedCheck);
			this.optionsGroup.Controls.Add(this.useSmartComments);
			this.optionsGroup.ForeColor = System.Drawing.Color.LightSkyBlue;
			this.optionsGroup.Location = new System.Drawing.Point(20, 71);
			this.optionsGroup.Name = "optionsGroup";
			this.optionsGroup.Size = new System.Drawing.Size(860, 91);
			this.optionsGroup.TabIndex = 1;
			this.optionsGroup.TabStop = false;
			this.optionsGroup.Text = "Processing Options";
			// 
			// classesCheck
			// 
			this.classesCheck.AutoSize = true;
			this.classesCheck.Checked = true;
			this.classesCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.classesCheck.ForeColor = System.Drawing.Color.White;
			this.classesCheck.Location = new System.Drawing.Point(20, 30);
			this.classesCheck.Name = "classesCheck";
			this.classesCheck.Size = new System.Drawing.Size(124, 17);
			this.classesCheck.TabIndex = 0;
			this.classesCheck.Text = "Add class summaries";
			// 
			// methodsCheck
			// 
			this.methodsCheck.AutoSize = true;
			this.methodsCheck.Checked = true;
			this.methodsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.methodsCheck.ForeColor = System.Drawing.Color.White;
			this.methodsCheck.Location = new System.Drawing.Point(654, 30);
			this.methodsCheck.Name = "methodsCheck";
			this.methodsCheck.Size = new System.Drawing.Size(137, 17);
			this.methodsCheck.TabIndex = 1;
			this.methodsCheck.Text = "Add method summaries";
			// 
			// parametersCheck
			// 
			this.parametersCheck.AutoSize = true;
			this.parametersCheck.Checked = true;
			this.parametersCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.parametersCheck.ForeColor = System.Drawing.Color.White;
			this.parametersCheck.Location = new System.Drawing.Point(20, 60);
			this.parametersCheck.Name = "parametersCheck";
			this.parametersCheck.Size = new System.Drawing.Size(172, 17);
			this.parametersCheck.TabIndex = 2;
			this.parametersCheck.Text = "Add parameter documentation";
			// 
			// returnsCheck
			// 
			this.returnsCheck.AutoSize = true;
			this.returnsCheck.Checked = true;
			this.returnsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.returnsCheck.ForeColor = System.Drawing.Color.White;
			this.returnsCheck.Location = new System.Drawing.Point(654, 60);
			this.returnsCheck.Name = "returnsCheck";
			this.returnsCheck.Size = new System.Drawing.Size(152, 17);
			this.returnsCheck.TabIndex = 3;
			this.returnsCheck.Text = "Add return documentation";
			// 
			// propertiesCheck
			// 
			this.propertiesCheck.AutoSize = true;
			this.propertiesCheck.Checked = true;
			this.propertiesCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.propertiesCheck.ForeColor = System.Drawing.Color.White;
			this.propertiesCheck.Location = new System.Drawing.Point(226, 30);
			this.propertiesCheck.Name = "propertiesCheck";
			this.propertiesCheck.Size = new System.Drawing.Size(143, 17);
			this.propertiesCheck.TabIndex = 4;
			this.propertiesCheck.Text = "Add property summaries";
			// 
			// skipExistingCheck
			// 
			this.skipExistingCheck.AutoSize = true;
			this.skipExistingCheck.Checked = true;
			this.skipExistingCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.skipExistingCheck.ForeColor = System.Drawing.Color.White;
			this.skipExistingCheck.Location = new System.Drawing.Point(226, 60);
			this.skipExistingCheck.Name = "skipExistingCheck";
			this.skipExistingCheck.Size = new System.Drawing.Size(159, 17);
			this.skipExistingCheck.TabIndex = 5;
			this.skipExistingCheck.Text = "Skip existing documentation";
			// 
			// skipGeneratedCheck
			// 
			this.skipGeneratedCheck.AutoSize = true;
			this.skipGeneratedCheck.Checked = true;
			this.skipGeneratedCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.skipGeneratedCheck.ForeColor = System.Drawing.Color.White;
			this.skipGeneratedCheck.Location = new System.Drawing.Point(432, 30);
			this.skipGeneratedCheck.Name = "skipGeneratedCheck";
			this.skipGeneratedCheck.Size = new System.Drawing.Size(120, 17);
			this.skipGeneratedCheck.TabIndex = 6;
			this.skipGeneratedCheck.Text = "Skip generated files";
			// 
			// useSmartComments
			// 
			this.useSmartComments.AutoSize = true;
			this.useSmartComments.Checked = true;
			this.useSmartComments.CheckState = System.Windows.Forms.CheckState.Checked;
			this.useSmartComments.ForeColor = System.Drawing.Color.White;
			this.useSmartComments.Location = new System.Drawing.Point(432, 60);
			this.useSmartComments.Name = "useSmartComments";
			this.useSmartComments.Size = new System.Drawing.Size(152, 17);
			this.useSmartComments.TabIndex = 7;
			this.useSmartComments.Text = "Generate smart comments";
			// 
			// templatesGroup
			// 
			this.templatesGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
			this.templatesGroup.Controls.Add(this.methodTemplateLabel);
			this.templatesGroup.Controls.Add(this.methodTemplate);
			this.templatesGroup.Controls.Add(this.paramTemplateLabel);
			this.templatesGroup.Controls.Add(this.paramTemplate);
			this.templatesGroup.Controls.Add(this.returnTemplateLabel);
			this.templatesGroup.Controls.Add(this.returnTemplate);
			this.templatesGroup.ForeColor = System.Drawing.Color.LightSkyBlue;
			this.templatesGroup.Location = new System.Drawing.Point(20, 169);
			this.templatesGroup.Name = "templatesGroup";
			this.templatesGroup.Size = new System.Drawing.Size(860, 120);
			this.templatesGroup.TabIndex = 2;
			this.templatesGroup.TabStop = false;
			this.templatesGroup.Text = "Summary Templates";
			// 
			// methodTemplateLabel
			// 
			this.methodTemplateLabel.ForeColor = System.Drawing.Color.White;
			this.methodTemplateLabel.Location = new System.Drawing.Point(20, 25);
			this.methodTemplateLabel.Name = "methodTemplateLabel";
			this.methodTemplateLabel.Size = new System.Drawing.Size(70, 20);
			this.methodTemplateLabel.TabIndex = 0;
			this.methodTemplateLabel.Text = "Method:";
			// 
			// methodTemplate
			// 
			this.methodTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(74)))));
			this.methodTemplate.ForeColor = System.Drawing.Color.White;
			this.methodTemplate.Location = new System.Drawing.Point(100, 25);
			this.methodTemplate.Name = "methodTemplate";
			this.methodTemplate.Size = new System.Drawing.Size(740, 20);
			this.methodTemplate.TabIndex = 1;
			this.methodTemplate.Text = "Performs operation: {methodName}";
			// 
			// paramTemplateLabel
			// 
			this.paramTemplateLabel.ForeColor = System.Drawing.Color.White;
			this.paramTemplateLabel.Location = new System.Drawing.Point(20, 55);
			this.paramTemplateLabel.Name = "paramTemplateLabel";
			this.paramTemplateLabel.Size = new System.Drawing.Size(70, 20);
			this.paramTemplateLabel.TabIndex = 2;
			this.paramTemplateLabel.Text = "Parameter:";
			// 
			// paramTemplate
			// 
			this.paramTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(74)))));
			this.paramTemplate.ForeColor = System.Drawing.Color.White;
			this.paramTemplate.Location = new System.Drawing.Point(100, 55);
			this.paramTemplate.Name = "paramTemplate";
			this.paramTemplate.Size = new System.Drawing.Size(740, 20);
			this.paramTemplate.TabIndex = 3;
			this.paramTemplate.Text = "The {paramName} to use";
			// 
			// returnTemplateLabel
			// 
			this.returnTemplateLabel.ForeColor = System.Drawing.Color.White;
			this.returnTemplateLabel.Location = new System.Drawing.Point(20, 85);
			this.returnTemplateLabel.Name = "returnTemplateLabel";
			this.returnTemplateLabel.Size = new System.Drawing.Size(70, 20);
			this.returnTemplateLabel.TabIndex = 4;
			this.returnTemplateLabel.Text = "Return:";
			// 
			// returnTemplate
			// 
			this.returnTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(74)))));
			this.returnTemplate.ForeColor = System.Drawing.Color.White;
			this.returnTemplate.Location = new System.Drawing.Point(100, 85);
			this.returnTemplate.Name = "returnTemplate";
			this.returnTemplate.Size = new System.Drawing.Size(740, 20);
			this.returnTemplate.TabIndex = 5;
			this.returnTemplate.Text = "The result of the operation";
			// 
			// progressGroup
			// 
			this.progressGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
			this.progressGroup.Controls.Add(this.txtPocoEditor);
			this.progressGroup.Controls.Add(this.progressBar);
			this.progressGroup.Controls.Add(this.statusLabel);
			this.progressGroup.ForeColor = System.Drawing.Color.LightSkyBlue;
			this.progressGroup.Location = new System.Drawing.Point(20, 295);
			this.progressGroup.Name = "progressGroup";
			this.progressGroup.Size = new System.Drawing.Size(860, 325);
			this.progressGroup.TabIndex = 3;
			this.progressGroup.TabStop = false;
			this.progressGroup.Text = "Progress";
			// 
			// txtPocoEditor
			// 
			this.txtPocoEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPocoEditor.BackColor = System.Drawing.Color.White;
			this.txtPocoEditor.DetectUrls = false;
			this.txtPocoEditor.Font = new System.Drawing.Font("Consolas", 12F);
			this.txtPocoEditor.Location = new System.Drawing.Point(28, 75);
			this.txtPocoEditor.Name = "txtPocoEditor";
			this.txtPocoEditor.ReadOnly = true;
			this.txtPocoEditor.Size = new System.Drawing.Size(812, 244);
			this.txtPocoEditor.TabIndex = 35;
			this.txtPocoEditor.Text = "";
			this.txtPocoEditor.WordWrap = false;
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(74)))));
			this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.progressBar.Location = new System.Drawing.Point(20, 20);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(820, 14);
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar.TabIndex = 0;
			// 
			// statusLabel
			// 
			this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.statusLabel.Location = new System.Drawing.Point(20, 39);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(820, 30);
			this.statusLabel.TabIndex = 1;
			this.statusLabel.Text = "Ready";
			this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// processButton
			// 
			this.processButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.processButton.FlatAppearance.BorderSize = 0;
			this.processButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.processButton.ForeColor = System.Drawing.Color.White;
			this.processButton.Location = new System.Drawing.Point(20, 626);
			this.processButton.Name = "processButton";
			this.processButton.Size = new System.Drawing.Size(180, 25);
			this.processButton.TabIndex = 4;
			this.processButton.Text = "Process Solution";
			this.processButton.UseVisualStyleBackColor = false;
			this.processButton.Click += new System.EventHandler(this.ProcessButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
			this.cancelButton.FlatAppearance.BorderSize = 0;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelButton.ForeColor = System.Drawing.Color.White;
			this.cancelButton.Location = new System.Drawing.Point(210, 626);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(180, 25);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = false;
			this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// SummaryCommentForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(884, 661);
			this.Controls.Add(this.solutionGroup);
			this.Controls.Add(this.optionsGroup);
			this.Controls.Add(this.templatesGroup);
			this.Controls.Add(this.progressGroup);
			this.Controls.Add(this.processButton);
			this.Controls.Add(this.cancelButton);
			this.ForeColor = System.Drawing.Color.White;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "SummaryCommentForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Enhanced XML Documentation Generator";
			this.solutionGroup.ResumeLayout(false);
			this.solutionGroup.PerformLayout();
			this.optionsGroup.ResumeLayout(false);
			this.optionsGroup.PerformLayout();
			this.templatesGroup.ResumeLayout(false);
			this.templatesGroup.PerformLayout();
			this.progressGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}



		#endregion
		private System.Windows.Forms.GroupBox solutionGroup;
		private System.Windows.Forms.TextBox txtRootDirectory;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.GroupBox optionsGroup;
		private System.Windows.Forms.CheckBox classesCheck;
		private System.Windows.Forms.CheckBox methodsCheck;
		private System.Windows.Forms.CheckBox propertiesCheck;
		private System.Windows.Forms.CheckBox skipExistingCheck;
		private System.Windows.Forms.CheckBox skipGeneratedCheck;
		private System.Windows.Forms.GroupBox progressGroup;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label statusLabel;
		private System.Windows.Forms.Button processButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.CheckBox parametersCheck;
		private System.Windows.Forms.CheckBox returnsCheck;

		private System.Windows.Forms.CheckBox useSmartComments;
		private System.Windows.Forms.TextBox methodTemplate;
		private System.Windows.Forms.TextBox paramTemplate;
		private System.Windows.Forms.TextBox returnTemplate;
		private System.Windows.Forms.GroupBox templatesGroup;
		private System.Windows.Forms.Label methodTemplateLabel;
		private System.Windows.Forms.Label paramTemplateLabel;
		private System.Windows.Forms.Label returnTemplateLabel;
		private System.Windows.Forms.RichTextBox txtPocoEditor;
	}
}
