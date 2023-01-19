using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    [Tooltip("DoorDirection (doorDir) indicates the door requirements for a room spawned at this point\n" +
             "1) Requires BOTTOM door\n" +
             "2) Requires LEFT door\n" +
             "3) Requires TOP door\n" +
             "4) Requires RIGHT door")]
    public int doorDir;

    private int random;
    private bool spawned = false;

    // Load in the Room Templates (stored in the RoomTemplates script attached to RoomTemplates)
    private RoomTemplates templates;
    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.5f);
    }

    void Spawn()
    {
        if (spawned == false)
        {
            GameObject room;
            Vector3 point = transform.position;
            switch (doorDir)
            {
                case 1:
                    random = Random.Range(0, templates.BRooms.Length);
                    room = templates.BRooms[random];
                    point.x += room.transform.position.x - 7.5f;
                    point.y += room.transform.position.y - 7.5f;
                    Instantiate(room, point, room.transform.rotation);
                    templates.rooms.Add(gameObject);
                    break;
                case 2:
                    random = Random.Range(0, templates.LRooms.Length);
                    room = templates.LRooms[random];
                    point.x += room.transform.position.x - 7.5f;
                    point.y += room.transform.position.y - 7.5f;
                    Instantiate(room, point, room.transform.rotation);
                    templates.rooms.Add(gameObject);
                    break;
                case 3:
                    random = Random.Range(0, templates.TRooms.Length);
                    room = templates.TRooms[random];
                    point.x += room.transform.position.x - 7.5f;
                    point.y += room.transform.position.y - 7.5f;
                    Instantiate(room, point, room.transform.rotation);
                    templates.rooms.Add(gameObject);
                    break;
                case 4:
                    random = Random.Range(0, templates.RRooms.Length);
                    room = templates.RRooms[random];
                    point.x += room.transform.position.x - 7.5f;
                    point.y += room.transform.position.y - 7.5f;
                    Instantiate(room, point, room.transform.rotation);
                    templates.rooms.Add(gameObject);
                    break;
                default:
                    Debug.Log("Error: RoomSpawner received an illegal DoorDir!");
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("SpawnPt"))
        {
            int dooorDir2 = col.GetComponent<RoomSpawner>().doorDir;
            if (dooorDir2 == 0)
            {
                Destroy(gameObject);
            }
            // if two rooms try to spawn a room in the same place
            else if (!(col.GetComponent<RoomSpawner>().spawned) && !(spawned))
            {
                GameObject corner = GetCorner(dooorDir2);
                Vector3 point = transform.position;
                point.x += corner.transform.position.x - 7.5f;
                point.y += corner.transform.position.y - 7.5f;
                Instantiate(corner, point, Quaternion.identity);
                Destroy(gameObject);
            }
            // if collides with a spawnpoint, set spawned to true because either this or col will spawn a room
            spawned = true;
        }
    }

    // NOTE: This method relies on the rooms being stored in a specific order in RoomTemplates!!!
    private GameObject GetCorner(int doorDir2)
    {
        // One of the rooms needs a bottom door
        if (doorDir == 1 || doorDir2 == 1)
        { // The other room needs a left door
            if (doorDir == 2 || doorDir2 == 2)
            {
                return templates.BRooms[2];
            } // The other room needs a right door
            else
            {
                return templates.BRooms[3];
            }
        }
        else
        {   // Same but for one of the rooms needing a bottom door.
            if (doorDir == 2 || doorDir2 == 2)
            {
                return templates.TRooms[2];
            } 
            else
            {
                return templates.TRooms[3];
            }
        }
    }
}
