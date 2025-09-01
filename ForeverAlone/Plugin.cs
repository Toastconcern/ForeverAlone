using System;
using BepInEx;
using UnityEngine;

namespace ForeverAlone
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
        GameObject friends;
        GameObject joins;

        void Start()
		{
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }

		void OnGameInitialized()
		{
            NetworkSystem.Instance.OnMultiplayerStarted += OnJoin;
            friends = GameObject.Find("FriendsJoining_Prefab");
            joins = GameObject.Find("JoinRoomTriggers_Prefab");
            friends.Destroy();
            joins.Destroy();
        }

		public void OnJoin() => NetworkSystem.Instance.ReturnToSinglePlayer();
    }
}
