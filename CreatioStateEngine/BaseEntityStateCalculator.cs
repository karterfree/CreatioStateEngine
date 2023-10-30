namespace CreatioStateEngine
{
	using System;
	using System.ComponentModel;
	using System.Linq;
	using ATF.Repository;
	using ATF.Repository.Providers;
	using Terrasoft.Core;

	#region Class: BaseEntityStateCalculator

	public abstract class BaseEntityStateCalculator: IEntityStateCalculator
	{

		#region Properties: Private

		private ProcessStateDto Dto { get; set; }
		
		private IDataProvider _dataProvider;
		private IDataProvider DataProvider {
			get => _dataProvider ??= new LocalDataProvider(UserConnection);
			set => _dataProvider = value;
		}

		#endregion

		#region Properties: Protected

		protected UserConnection UserConnection { get; set; }

		private IAppDataContext _appDataContext;
		protected IAppDataContext AppDataContext =>
			_appDataContext ??= AppDataContextFactory.GetAppDataContext(DataProvider);
		
		//protected IWorkCurrencyRateHelper WorkCurrencyRateHelper { get; set; }

		#endregion
		
		#region Methods: Private

		/*private IWorkCurrencyRateHelper CreateWorkCurrencyRateHelper() {
			return ClassFactory.Get<IWorkCurrencyRateHelper>(
				new ConstructorArgument("userConnection", UserConnection));
		}*/

		private ProcessColumnStateDto GetColumn(string name) {
			var column = Dto.Columns.FirstOrDefault(x => x.Name == name);
			if (column == null) {
				throw new ArgumentOutOfRangeException(name);
			}
			return column;
		}

		#endregion

		#region Methods: Protected

		/*protected decimal ConvertToPrimaryValue(Guid inputCurrencyId, DateTime rateOn, decimal value) {
			var primaryCurrency = AppDataContext.GetSysSettingValue<Guid>("PrimaryCurrency");
			var primaryCurrencyId = primaryCurrency.Value;
			return WorkCurrencyRateHelper.GetAmountConvertedToCurrency(inputCurrencyId,
				primaryCurrencyId,
				value, rateOn);
		}*/

		protected bool IsNew() {
			return Dto.IsNew;
		}

		protected int GetIteration() {
			return Dto.Iteration;
		}

		protected bool GetFeatureEnabled(string code) {
			return UserConnection.GetFeatureEnabled(code);
		}

		protected bool IsValueChanged(string name) {
			var column = GetColumn(name);
			return column.IsChanged;
		}

		protected void SetValue(string name, object value) {
			var column = GetColumn(name);
			column.IsChanged = column.IsChanged || !Equals(column.Value, value);
			column.Value = value;
		}

		protected object GetValue(string name) {
			var column = GetColumn(name);
			return column.Value;
		}

		protected T GetTypedValue<T>(string name) {
			var value = GetValue(name);
			if (value == null) {
				return default(T);
			}

			if (value is T typedValue) {
				return typedValue;
			}
			var converter = TypeDescriptor.GetConverter(typeof(T));
			if (!converter.CanConvertTo(typeof(T))) {
				return default(T);
			}

			return (T)converter.ConvertTo(value, typeof(T));
		}

		protected void TraceLog(string message) {
			Dto.Trace.Add(message);
		}

		#endregion

		#region Methods: Internal

		internal void Calculate(UserConnection userConnection, ProcessStateDto dto, IDataProvider dataProvider) {
			UserConnection = userConnection;
			Dto = dto;
			DataProvider = dataProvider;
			//WorkCurrencyRateHelper = CreateWorkCurrencyRateHelper();
			Calculate();
		}

		#endregion

		#region Methods: Public

		public abstract void Calculate();

		#endregion

	}

	#endregion
}
