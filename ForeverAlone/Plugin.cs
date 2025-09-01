using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using GorillaNetworking;
using HarmonyLib;
using static ForeverAlone.HarmonyPatches;

namespace ForeverAlone
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Start() => ApplyHarmonyPatches();

        // __originalMethod needed so that the method signatures always match.
        // If it's not there, things will break.
        public static bool StubPatch(MethodBase __originalMethod) => false;

        public static void ApplyPatches(Harmony harmony)
        {
            var targetMethods = new List<MethodInfo>
            {
                GetMethodSafe(typeof(GorillaNetworkJoinTrigger), "OnBoxTriggered"),
                GetMethodSafe(typeof(GorillaNetworkLeaveRoomTrigger), "OnBoxTriggered"),
                GetMethodSafe(typeof(GorillaNetworkLeaveTutorialTrigger), "OnBoxTriggered"),
                GetMethodSafe(typeof(PhotonNetworkController), "AttemptToJoinSpecificRoomAsync")
            };
            var prefix = typeof(Plugin).GetMethod("StubPatch",
                BindingFlags.Static | BindingFlags.Public);

            foreach (var method in targetMethods)
            {
                harmony.Patch(original: method, prefix: new HarmonyMethod(prefix));
                Console.WriteLine($"patched {method.DeclaringType.FullName}.{method.Name}");
            }
        }
    }
}
