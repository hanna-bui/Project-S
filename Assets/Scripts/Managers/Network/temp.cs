using Managers.Network;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class temp : NetworkManager
{
    // Overrides the base singleton so we don't
    // have to cast to this type everywhere.
    public static new temp singleton { get; private set; }

    public CharacterSelectUI csUI;

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        singleton = this;
    }

    #region Unity Callbacks

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion

    #region Scene Management

    
    /// <summary>
    /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
    /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
    /// </summary>
    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        if (networkSceneName == "SampleScene2.0")
        {
            // NetworkClient.Ready();
            // NetworkClient.AddPlayer();
            var characterMessage = new CreateCharacterMessage
            {
                index = csUI.index
            };
            NetworkClient.Send(characterMessage);
        }
    }

    #endregion

    #region Server System Callbacks

    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
    }

    private void OnCreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(spawnPrefabs[message.index], startPos.position, startPos.rotation)
            : Instantiate(spawnPrefabs[message.index]);
        
        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);
    }
    
    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        
    }
    #endregion
    
}
