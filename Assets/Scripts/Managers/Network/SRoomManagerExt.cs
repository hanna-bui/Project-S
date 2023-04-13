using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

namespace Managers.Network
{
    [AddComponentMenu("")]
    
    public class SRoomManagerExt : NetworkRoomManager
    {
        // [Header("Spawner Setup")]
        // [Tooltip("Reward Prefab for the Spawner")]
        // public GameObject rewardPrefab;

        public static new SRoomManagerExt singleton { get; private set; }
        [SerializeField] private int Test;

        public override void Awake()
        {
            base.Awake();
            
            singleton = this;
        }

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            if (sceneName == RoomScene)
            {
            }
            if (sceneName == GameplayScene)
            {
                
                Debug.Log("Loading GameplayScene");
            }
        }
        
        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName == GameplayScene)
            {
                // DontDestroyOnLoad(s);
                NetworkServer.Spawn(Instantiate(spawnPrefabs[4]));
            }
            base.OnServerSceneChanged(sceneName);
        }


        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            var character = roomPlayer.GetComponent<SRoomPlayerExt>().characterIndex;
            playerPrefab = spawnPrefabs[character];
            return null;
        }

        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
            showStartButton = true;
#endif
        }
        
        

        public override void OnGUI()
        {
            if (!showRoomGUI)
                return;

            if (NetworkServer.active && Utils.IsSceneActive(GameplayScene))
            {
                GUILayout.BeginArea(new Rect(Screen.width - 150f, 10f, 140f, 30f));
                if (GUILayout.Button("Return to Room"))
                    ServerChangeScene(RoomScene);
                GUILayout.EndArea();
            }

            if (Utils.IsSceneActive(RoomScene))
                GUI.Box(new Rect(10f, 180f, 720f, 350f), "PLAYERS");

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }
    }
}
