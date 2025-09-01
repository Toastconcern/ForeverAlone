using System;
using System.Reflection;
using HarmonyLib;

namespace ForeverAlone
{
	/// <summary>
	/// This class handles applying harmony patches to the game.
	/// You should not need to modify this class.
	/// </summary>
	public class HarmonyPatches
	{
		private static Harmony instance;

		public static bool IsPatched { get; private set; }
		public const string InstanceId = PluginInfo.GUID;

		internal static void ApplyHarmonyPatches()
		{
			if (!IsPatched)
			{
				if (instance == null)
				{
					instance = new Harmony(InstanceId);
				}

				instance.PatchAll(Assembly.GetExecutingAssembly());
				Plugin.ApplyPatches(instance);
				IsPatched = true;
			}
		}

		internal static void RemoveHarmonyPatches()
		{
			if (instance != null && IsPatched)
			{
				instance.UnpatchSelf();
				IsPatched = false;
			}
		}
		
		/// <summary>
		/// Helper util to always return usable MethodInfos regardless of their publicity or ambiguity.
		/// </summary>
		/// <param name="type">Class that the Method is contained within.</param>
		/// <param name="methodString">Exact name of the Method as a string.</param>
		/// <param name="parameters">Parameters of that Method, if any, to resolve ambiguous matches.</param>
		/// <returns></returns>
		public static MethodInfo GetMethodSafe(Type type, string methodString, Type[] parameters = null)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

			try
			{
				MethodInfo method;
				if (parameters == null)
					method = type.GetMethod(methodString, flags);
				else
					method = type.GetMethod(methodString, flags, null, parameters, null);

				if (method == null)
					Console.WriteLine("could not find method name " + methodString);
            
				return method;
			}
			catch (AmbiguousMatchException)
			{
				Console.WriteLine("ambiguous match for " + methodString);
				return null;
			}
		}
	}
}
