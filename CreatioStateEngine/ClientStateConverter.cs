namespace CreatioStateEngine
{
	using System;
	using System.Linq;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Nui.ServiceModel.DataContract;
	using Terrasoft.Nui.ServiceModel.Extensions;
	using DataValueType = Terrasoft.Nui.ServiceModel.DataContract.DataValueType;

	#region Class: ClientStateConverter

	internal class ClientStateConverter
	{
		#region Methods: Private

		private ProcessColumnStateDto ConvertToProcessStateColumn(UserConnection userConnection,
			ClientColumnStateDto clientColumnState) {
			var value = ConvertToProcessStateValue(userConnection, clientColumnState.Value,
				clientColumnState.DataValueType);
			return new ProcessColumnStateDto() {
				Name = clientColumnState.Name,
				Value = value,
				IsChanged = clientColumnState.IsChanged,
				DataValueType =  clientColumnState.DataValueType.ToDataValueType(userConnection)
			};
		}

		private object GetParameterValue(UserConnection userConnection, object rawValue, DataValueType dataValueType) {
			var parameter = new Parameter() {
				Value = rawValue,
				DataValueType = dataValueType
			};

			return parameter.GetValue(userConnection);
		}

		private T GetPureTypedValue<T>(object value) {
			return DataTypeUtilities.ValueAsType<T>(value);
		}

		private Type GetTypeFromDataValueType(UserConnection userConnection, DataValueType dataValueType) {
			var coreDataValueType = dataValueType.ToDataValueType(userConnection);
			return coreDataValueType?.ValueType;
		}

		private object ConvertToProcessStateValue(UserConnection userConnection, object rawValue,
			DataValueType dataValueType) {
			var parameterValue = GetParameterValue(userConnection, rawValue, dataValueType);
			var type = GetTypeFromDataValueType(userConnection, dataValueType);
			var convertMethod = StateEngineUtilities.GetGenericMethod(GetType(), "GetPureTypedValue", type);
			var value = convertMethod.Invoke(this, new object[] { parameterValue });
			return value;
		}

		private ClientColumnStateDto ConvertToClientStateColumn(UserConnection userConnection, ProcessColumnStateDto column) {
			return new ClientColumnStateDto() {
				Name = column.Name,
				DataValueType = column.DataValueType.ToEnum(),
				IsChanged = column.IsChanged,
				Value = ConvertToClientStateValue(userConnection, column)
			};
		}

		private object ConvertToClientStateValue(UserConnection userConnection, ProcessColumnStateDto column) {
			string serializedColumnValue = null;
			if ((column.DataValueType.IsLookup || column.DataValueType is GuidDataValueType) && column.Value is Guid guidValue) {
				serializedColumnValue = !guidValue.IsEmpty()
					? guidValue.ToString()
					: string.Empty;
			}

			if ((column.DataValueType is DateDataValueType || column.DataValueType is DateTimeDataValueType ||
			     column.DataValueType is TimeDataValueType) && column.Value != null) {
				if (column.Value is DateTime dateTime && dateTime != DateTime.MinValue) {
					var parsedValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
					serializedColumnValue = DateTimeDataValueType.Serialize(parsedValue, TimeZoneInfo.Utc);
				} else {
					serializedColumnValue = string.Empty;
				}
			}

			if (column.DataValueType.IsBinary) {
				if (column.Value is byte[] valueArray) {
					serializedColumnValue = Encoding.UTF8.GetString(valueArray, 0, valueArray.Length);
				}
			}

			return serializedColumnValue ?? column.Value ?? string.Empty;
		}

		#endregion

		#region Methods: Public

		public ProcessStateDto ConvertToProcessState(UserConnection userConnection, ClientStateDto clientStateDto) {
			var processStateDto = new ProcessStateDto {
				Key = clientStateDto.Key,
				Iteration = clientStateDto.Iteration,
				IsNew = clientStateDto.IsNew,
				Columns = clientStateDto.Columns.Select(column => ConvertToProcessStateColumn(userConnection, column))
					.ToList()
			};
			return processStateDto;
		}

		public ClientStateDto ConvertToClientState(UserConnection userConnection, ProcessStateDto processStateDto) {
			var clientStateDto = new ClientStateDto() {
				Key = processStateDto.Key,
				Columns = processStateDto.Columns.Select(column => ConvertToClientStateColumn(userConnection, column)).ToList(),
				Trace = processStateDto.Trace.Select(x=>x).ToList()
			};
			return clientStateDto;
		}

		#endregion
	}

	#endregion
}
