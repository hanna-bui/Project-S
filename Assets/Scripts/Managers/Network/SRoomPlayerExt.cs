using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers.Network
{
    public class SRoomPlayerExt : NetworkRoomPlayer
    {
        public int characterIndex = 0;
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
        }

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public override void OnGUI()
        {
            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room)
            {
                if (!room.showRoomGUI)
                    return;

                if (!Utils.IsSceneActive(room.RoomScene))
                    return;
                
                if (GUI.Button(new Rect(120, 275, 80, 20), "Ninja"))
                {
                    characterIndex = 0;
                }
                if (GUI.Button(new Rect(20f, 275, 80, 20), "Samurai"))
                {
                    characterIndex = 1;
                }
                if (GUI.Button(new Rect(220, 275, 80, 20), "Monk"))
                {
                    characterIndex = 2;
                }
            }
            
            base.OnGUI();
        }
    }
}