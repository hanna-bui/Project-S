using Mirror;
using Mirror.Examples.NetworkRoom;
using UnityEngine;

namespace Managers.Network
{
    internal class Spawner
    {
        [ServerCallback]
        internal static void InitialSpawn()
        {
            SpawnEnemy();
        }

        [ServerCallback]
        internal static void SpawnEnemy()
        {
            Vector3 spawnPosition = new Vector3(762, 433, 0);
            NetworkServer.Spawn(Object.Instantiate(SRoomManagerExt.singleton.spawnPrefabs[3], spawnPosition,
                Quaternion.identity));
        }
    }
}