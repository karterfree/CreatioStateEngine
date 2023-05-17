namespace CreatioStateEngine
{
	using Terrasoft.Core;

	#region Class: ProcessColumnStateDto

	internal class ProcessColumnStateDto
	{
		public bool IsChanged { get; set; }
		public string Name { get; set; }
		public object Value { get; set; }

		public DataValueType DataValueType { get; set; }
	}

	#endregion
}
