public interface ICodeAnalyzerProgress {
	public void InitialProgressSteps(int stepsCount);
	public void OnProgressChanged(int progressPercentage);
	public void OnUserStateChanged(string message, CodeAnalyzerState state = CodeAnalyzerState.None);
	public void OnCompleted((bool State, string Msge) result);
	public void OnProcessCanceled();
}
