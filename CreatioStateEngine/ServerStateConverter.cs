namespace CreatioStateEngine
{
	using System;
	using System.Linq;
	using Terrasoft.Common;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.Entities;

	#region Class: ServerStateConverter

	internal class ServerStateConverter
	{
		#region Methods: Private

		private object ConvertedForEntityValue(EntitySchemaColumn column, object value) {
			if (column.IsLookupType && value is Guid guidValue && guidValue == Guid.Empty) {
				return null;
			}
			if (column.DataValueType.IsDateTime && value is DateTime dateTimeValue && dateTimeValue == DateTime.MinValue) {
				return null;
			}
			return value;
		}

		private ProcessColumnStateDto ConvertToProcessStateColumnDto(Entity entity, EntitySchemaColumn column) {
			return new ProcessColumnStateDto() {
				Name = column.Name,
				DataValueType = column.DataValueType,
				IsChanged = GetIsEntityColumnChanged(entity, column),
				Value = ConvertToClientStateValue(entity, column)
			};
		}

		private bool GetIsEntityColumnChanged(Entity entity, EntitySchemaColumn column) {
			return entity.GetChangedColumnValues().Any(columnValue => columnValue.Column == column);
		}

		private T GetEntityTypedColumnValue<T>(Entity entity, EntitySchemaColumn column) {
			return entity.GetTypedColumnValue<T>(column);
		}

		private object ConvertToClientStateValue(Entity entity, EntitySchemaColumn column) {
			var convertMethod = StateEngineUtilities.GetGenericMethod(GetType(), "GetEntityTypedColumnValue",
				column.DataValueType.ValueType);
			var value = convertMethod.Invoke(this, new object[] { entity, column });
			return value;
		}

		#endregion

		#region Methods: Public

		public ProcessStateDto ConvertToProcessState(Entity entity) {
			var processStateDto = new ProcessStateDto() {
				Key = entity.Schema.Name,
				Iteration = 1,
				IsNew = entity.StoringState == StoringObjectState.New,
				Columns = entity.Schema.Columns.Select(column => ConvertToProcessStateColumnDto(entity, column)).ToList()
			};
			return processStateDto;
		}

		public void ApplyToEntity(Entity entity, ProcessStateDto processStateDto) {
			if (entity is null) {
				throw new ArgumentNullException(nameof(entity));
			}
			if (processStateDto is null) {
				throw new ArgumentNullException(nameof(processStateDto));
			}
			if (processStateDto.Columns is null) {
				throw new ArgumentNullException("ProcessStateDto.Columns");
			}
			if (processStateDto.Columns.IsEmpty()) {
				throw new ArgumentEmptyException("ProcessStateDto.Columns");
			}
			processStateDto.Columns.ForEach(processStateColumnDto => {
				if (!processStateColumnDto.IsChanged) {
					return;
				}
				var column = entity.Schema.Columns.FindByName(processStateColumnDto.Name);
				if (column == null) {
					return;
				}
				var currentValue = entity.GetColumnValue(column);
				if (!Equals(currentValue, processStateColumnDto.Value)) {
					entity.SetColumnValue(column, ConvertedForEntityValue(column, processStateColumnDto.Value));
				}
			});
		}

		#endregion
	}

	#endregion
}
