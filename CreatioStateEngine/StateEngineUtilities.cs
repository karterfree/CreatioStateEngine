namespace CreatioStateEngine
{
	using System;
	using System.Linq;
	using System.Reflection;

	#region StateEngineUtilities

	internal class StateEngineUtilities
	{

		#region Methods: Public

		public static MethodInfo GetGenericMethod(Type type, string methodName, params Type[] genericTypes) {
			MethodInfo response = type
				.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
				.FirstOrDefault(method => method.Name == methodName && method.ContainsGenericParameters);
			return response?.MakeGenericMethod(genericTypes);
		}

		#endregion

	}

	#endregion
}
