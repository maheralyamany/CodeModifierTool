using System.Collections.Generic;

public class MethodWorkerParams {
	public bool CancellationPending { get; private set; }
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
	private bool isRunning;


	public bool GetIsRunning() {
		return isRunning;
	}


	public void SetIsRunning(bool value) {
		if (value) {
			complatedSteps = 0;
			ProgressSteps = 0;
		}
		isRunning = value;
	}
	public bool ScanDirectoriesFiles() {
		if (this.CsFiles.Count == 0) {
			if (this.Directories.Count == 0) {
				this?.OnCompleted((false, "لايوجد ملفات لمعالجتها"));
				return false;
			}
			var fileHelper = new ProjectFileHelper() {
				BlockedFolders = this.BlockedFolders,
				ExcludedExtensions = this.ExcludedExtensions
			};
			foreach (var rootDirectory in this.Directories) {
				var csFiles = fileHelper.DirectoryFiles(rootDirectory);
				this.CsFiles.AddListRange(csFiles);
			}
			this.CsFiles = this.CsFiles.LDistinct();
		}
		return true;
	}

	List<string> modifiedFiles = new List<string>();
	private List<string> csFiles = new List<string>();
	private List<string> directories = new List<string>();
	private int complatedSteps = 0;


	public int GetComplatedSteps() {
		return complatedSteps;
	}


	private void SetComplatedSteps(int value) {
		complatedSteps = value;
		if (complatedSteps > ProgressSteps)
			complatedSteps = ProgressSteps;
	}

	public int ProgressSteps { get; private set; } = 0;
	public List<string> Directories {

		get {
			if (directories == null)
				directories = new List<string>();
			return directories;
		}

		set { directories = value; }
	}
	public List<string> CsFiles {

		get {
			if (csFiles == null)
				csFiles = new List<string>();
			return csFiles;
		}

		set { csFiles = value; }
	}
	public ICodeAnalyzerProgress Progress { get; set; }




	public MethodWorkerParams() {

	}


	public MethodWorkerParams(ICodeAnalyzerProgress progress) : this() {
		Progress = progress;
	}


	public void Cancel() {
		CancellationPending = true;
	}

	public bool IsCancellationRequested() {
		//if (CancellationPending)

		return CancellationPending;
	}

	public void OnProgressChanged(int percentProgress) {
		SetComplatedSteps(GetComplatedSteps() + percentProgress);
		if (Progress != null && !CancellationPending) {
			Progress.OnProgressChanged(percentProgress);
		}
	}

	public int GetRestStepsCount() {
		var rest = ProgressSteps - GetComplatedSteps();
		if (rest > 0)
			return (int)rest;
		return 0;
	}

	public void InitialProgressSteps(int stepsCount) {
		ProgressSteps = stepsCount;
		if (Progress != null && !CancellationPending)
			Progress.InitialProgressSteps(stepsCount);
	}

	public void OnUserStateChanged(string message, CodeAnalyzerState state = CodeAnalyzerState.None) {
		if (Progress != null && !CancellationPending)
			Progress.OnUserStateChanged(message, state);
	}

	public void OnCompleted((bool State, string Msge) result) {
		if (Progress != null && !CancellationPending)
			Progress.OnCompleted(result);
	}

	public void OnProcessCanceled() {
		if (Progress != null)
			Progress.OnProcessCanceled();
	}

	public List<string> GetModifiedFiles() {
		if (modifiedFiles == null)
			modifiedFiles = new List<string>();
		return modifiedFiles;
	}

	public void SetModifiedFiles(List<string> value) {
		modifiedFiles = value;
	}

	public void AddDirectory(string directory) {
		Directories.Add(directory);
	}

	public void AddCsFile(string file) {
		CsFiles.Add(file);
	}

	public void AddModifiedFile(string file) {
		GetModifiedFiles().Add(file);
	}

	public void Dispose() {
		CancellationPending = false;
		SetIsRunning(false);
		modifiedFiles = new List<string>();


	}
}
