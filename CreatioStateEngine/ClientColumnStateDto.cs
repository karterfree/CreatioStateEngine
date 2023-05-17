namespace CreatioStateEngine
{
	using System.Runtime.Serialization;
	using Terrasoft.Nui.ServiceModel.DataContract;

	[DataContract]
	public class ClientColumnStateDto
	{
		[DataMember(Name = "isChanged")]
		public bool IsChanged { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "value")]
		public object Value { get; set; }

		[DataMember(Name = "dataValueType")]
		public DataValueType DataValueType { get; set; }
	}
}
