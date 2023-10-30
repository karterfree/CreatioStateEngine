namespace CreatioStateEngine
{

	using ATF.Repository.Providers;

	public static class StateEngineExecutorFactory
	{
		
		public static IStateEngineExecutor GetExecutor() {
			return new StateEngineExecutor();
		}
		
		public static IStateEngineExecutor GetExecutor(IDataProvider dataProvider) {
			return new StateEngineExecutor(dataProvider);
		}
		
	}
}
