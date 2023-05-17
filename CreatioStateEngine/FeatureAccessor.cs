namespace CreatioStateEngine
{
	using Terrasoft.Core;
	using Terrasoft.Core.DB;

	#region Class: FeatureAccessor

	internal static class FeatureAccessor
	{
		#region Methods: Public

		public static int GetFeatureState(this UserConnection userConnection, string code) {
			var select = (Select)new Select(userConnection).Top(1)
				.Column("AdminUnitFeatureState", "FeatureState")
				.From("AdminUnitFeatureState")
				.InnerJoin("Feature").On("Feature", "Id").IsEqual("AdminUnitFeatureState", "FeatureId")
				.InnerJoin("SysAdminUnitInRole").On("SysAdminUnitInRole", "SysAdminUnitRoleId")
				.IsEqual("AdminUnitFeatureState", "SysAdminUnitId")
				.Where("Feature", "Code").IsEqual(Column.Parameter(code))
				.And("SysAdminUnitInRole", "SysAdminUnitId").IsEqual(Column.Parameter(userConnection.CurrentUser.Id))
				.OrderByDesc("AdminUnitFeatureState", "FeatureState");
			return select.ExecuteScalar<int>();
		}

		public static bool GetFeatureEnabled(this UserConnection userConnection, string code) {
			return userConnection.GetFeatureState(code) == 1;
		}

		#endregion
	}

	#endregion
}
