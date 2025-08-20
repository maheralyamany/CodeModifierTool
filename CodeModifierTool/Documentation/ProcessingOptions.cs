namespace CodeModifierTool {
	public class ProcessingOptions {
		public bool AddClassSummaries { get; set; } = true;
		public bool AddConstructorSummaries { get; set; } = true;
		public bool AddMethodSummaries { get; set; } = true;
		public bool AddPropertySummaries { get; set; } = true;
		public bool AddAutoPropertySummaries { get; set; } = false;
		public bool AddInterfaceSummaries { get; set; } = true;
		public bool AddStructSummaries { get; set; } = true;
		public bool AddEnumSummaries { get; set; } = true;
		public bool AddEventSummaries { get; set; } = true;
		public bool AddDelegateSummaries { get; set; } = true;
		public bool AddFieldSummaries { get; set; } = false;
		public bool AddParameterDocs { get; set; } = true;
		public bool AddReturnDocs { get; set; } = true;
		public bool SkipExistingDocumentation { get; set; } = false;
		public bool SkipGeneratedFiles { get; set; } = true;
		public bool GenerateSmartComments { get; set; } = true;
		/*public string MethodTemplate { get; set; } = "Performs operation: {methodName}";
        public string ParameterTemplate { get; set; } = "The {paramName} to use";
        public string ReturnTemplate { get; set; } = "The result of the operation";*/
	}
}
