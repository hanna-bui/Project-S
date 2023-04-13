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
        [SerializeField] public GameObject Test; 

        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            
            singleton = this;
        }

        /// <summary>Called from ServerChangeScene immediately before SceneManager.LoadSceneAsync is executed</summary>
        public override void OnServerChangeScene(string newSceneName)
        {
            // if (newSceneName == GameplayScene)
            // {
            //     var s = Instantiate(Test);
            //     DontDestroyOnLoad(s);
            //     NetworkServer.Spawn(s);
            // }
        }

        /// <summary>
        /// This is called on the server when a networked scene finishes loading.
        /// </summary>
        /// <param name="sceneName">Name of the new scene.</param>
        public override void OnRoomServerSceneChanged(string sceneName)
        {
            // // spawn the initial batch of Rewards
            if (sceneName == RoomScene)
            {
            }
            if (sceneName == GameplayScene)
            {
                // NetworkServer.Spawn(Object.Instantiate(Test));
                Debug.Log("Loading GameplayScene");
                // Spawner.InitialSpawn();
            }
        }
        
        /// <summary>
        /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
        /// </summary>
        /// <param name="sceneName">The name of the new scene.</param>
        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName == GameplayScene)
            {
                // var s = Instantiate(Test);
                // // DontDestroyOnLoad(s);
                // NetworkServer.Spawn(s);
            }
            base.OnRoomServerSceneChanged(sceneName);
        }


        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            var character = roomPlayer.GetComponent<SRoomPlayerExt>().characterIndex;
            playerPrefab = spawnPrefabs[character];
            return null;
        }

        /*
            This code below is to demonstrate how to do a Start button that only appears for the Host player
            showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
            all players are ready, but if a player cancels their ready state there's no callback to set it back to false
            Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
            Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
            is set as DontDestroyOnLoad = true.
        */

        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
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
