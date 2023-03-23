using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Managers;

public class MyNetworkManagement : NetworkManager
{
    void CreateCharacter(NetworkConnectionToClient conn)
    {
        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject gameobject = Instantiate(GameManager.instance.currCharacter.prefab);

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }
}
