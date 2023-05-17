namespace CreatioStateEngine
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	#region Class: ClientStateDto

	[DataContract]
	public class ClientStateDto
	{
		[DataMember(Name = "key")]
		public string Key { get; set; }

		[DataMember(Name = "columns")]
		public List<ClientColumnStateDto> Columns { get; set; }

		[DataMember(Name = "iteration")]
		public int Iteration { get; set; }

		[DataMember(Name = "isNew")]
		public bool IsNew { get; set; }

		[DataMember(Name = "trace")]
		public List<string> Trace { get; set; }
	}

	#endregion
}
