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
    [SerializeField] private int doorDir;

    private int random;
    private bool spawned = false;

    // Load in the Room Templates (stored in the RoomTemplates script attached to RoomTemplates)
    private RoomTemplates templates;
    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 1);
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
                    break;
                case 2:
                    random = Random.Range(0, templates.LRooms.Length);
                    room = templates.LRooms[random];
                    point.x += room.transform.position.x - 7.5f;
                    point.y += room.transform.position.y - 7.5f;
                    Instantiate(room, point, room.transform.rotation);
                    break;
                case 3:
                    random = Random.Range(0, templates.TRooms.Length);
                    room = templates.TRooms[random];
                    point.x += room.transform.position.x - 7.5f;
                    point.y += room.transform.position.y - 7.5f;
                    Instantiate(room, point, room.transform.rotation);
                    break;
                case 4:
                    random = Random.Range(0, templates.RRooms.Length);
                    room = templates.RRooms[random];
                    point.x += room.transform.position.x - 7.5f;
                    point.y += room.transform.position.y - 7.5f;
                    Instantiate(room, point, room.transform.rotation);
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
        if (col.CompareTag("SpawnPt") && col.GetComponent<RoomSpawner>().spawned)
        {
            Destroy(gameObject);
        }
    }
}
