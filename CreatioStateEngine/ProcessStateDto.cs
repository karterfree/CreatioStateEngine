using System.Collections.Generic;

namespace CreatioStateEngine
{
	#region Class: ProcessStateDto

	internal class ProcessStateDto
	{
		public string Key { get; set; }

		public int Iteration { get; set; }

		public bool IsNew { get; set; }

		public List<ProcessColumnStateDto> Columns { get; set; }

		public List<string> Trace { get; set; }

		internal ProcessStateDto() {
			Trace = new List<string>();
		}

	}

	#endregion
}
