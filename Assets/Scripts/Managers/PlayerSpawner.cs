using Managers;
using Movement.Pathfinding;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public void Spawn(GameObject player)
    {
        var grid = GameObject.FindWithTag("Level").GetComponent<NewGrid>();
        var startPos = grid.GetRandomCoords();
        Instantiate(player, startPos, Quaternion.identity);
        if (Camera.main != null) Camera.main.GetComponent<FollowPlayer>().AttachToPlayer(player);
    }
}
