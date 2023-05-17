namespace CreatioStateEngine
{
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	#region Interface: IStateEngineExecutor

	public interface IStateEngineExecutor
	{
		void ServerSideExecute(Entity entity);

		ClientStateDto ClientSideExecute(UserConnection userConnection, ClientStateDto clientStateDto);
	}

	#endregion
}
