using System;
using System.Windows.Forms;

namespace CodeModifierTool {

	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//RunOperation(SelectedOperation.FormatCode);

			Application.Run(new OperationsForm());
			//RunApplication();

		}



		private static void RunOperation(SelectedOperation operation) {
			var WorkerParams = new MethodWorkerParams() {
				CsFiles = new System.Collections.Generic.List<string>() { @"D:\OMaxSystem\M10_08_2022\OpetraERPSys\OpetraProErpSys\AN\ItemMovArgs.cs" }
			};
			WorkerParams.SetIsRunning(true);
			DynamicGenerator.ProcessOperation(WorkerParams, operation);

		}

	}
}
