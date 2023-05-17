namespace CreatioStateEngine
{
	public static class StateEngineExecutorFactory
	{
		public static IStateEngineExecutor GetExecutor() {
			return new StateEngineExecutor();
		}
	}
}
