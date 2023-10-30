using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;

namespace CreatioStateEngine
{

	using ATF.Repository.Providers;

	#region Class: StateEngineExecutor

	internal class StateEngineExecutor: IStateEngineExecutor
	{
		
		#region Properties: Private

		private IDataProvider DataProvider { get; set; }
		
		#endregion
		
		#region Constructors: Public

		public StateEngineExecutor() {
		}

		public StateEngineExecutor(IDataProvider dataProvider) {
			DataProvider = dataProvider;
		}
		
		#endregion
		
		#region Methods: Private

		private void Execute(UserConnection userConnection, ProcessStateDto processStateDto) {
			var calculator = ClassFactory.Get<IEntityStateCalculator>(processStateDto.Key);
			if (calculator is BaseEntityStateCalculator baseStateCalculator) {
				baseStateCalculator.Calculate(userConnection, processStateDto, DataProvider);
			}
		}
		
		#endregion

		#region Methods: Public

		public void ServerSideExecute(Entity entity) {
			var converter = new ServerStateConverter();
			var processStateDto = converter.ConvertToProcessState(entity);
			Execute(entity.UserConnection, processStateDto);
			converter.ApplyToEntity(entity, processStateDto);
		}
		
		public ClientStateDto ClientSideExecute(UserConnection userConnection, ClientStateDto clientStateDto) {
			var converter = new ClientStateConverter();
			var processStateDto = converter.ConvertToProcessState(userConnection, clientStateDto);
			Execute(userConnection, processStateDto);
			var response = converter.ConvertToClientState(userConnection, processStateDto);
			return response;
		}

		#endregion

	}

	#endregion
}
