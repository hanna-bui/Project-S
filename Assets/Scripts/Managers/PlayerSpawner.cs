using Characters;
using Managers;
using Movement.Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // ReSharper disable Unity.PerformanceAnalysis
    public GameObject Spawn(GameObject player)
    { 
        var spawnpt = GameObject.Find("SpawnPoint").transform;

        var parent = GameObject.Find("Characters").transform;
        if (parent!=null) player.transform.SetParent(parent);
        if (spawnpt != null) player.transform.position = spawnpt.position;

        var cam = Instantiate(Camera.main);
        var s = cam.AddComponent<FollowPlayer>();
        s.AttachToPlayer(player);
        if (Camera.main != null) Camera.main.gameObject.SetActive(false);
        return player;
    }
}
