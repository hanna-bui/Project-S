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
            switch (doorDir)
            {
                case 1:
                    random = Random.Range(0, templates.BRooms.Length);
                    //Transform t = transform;
                    //t.position.x -= ;
                    //Instantiate(templates.BRooms[random], transform.position, Quaternion.identity);
                    //Instantiate(templates.BRooms[random], transform.position, templates.BRooms[random].transform.rotation);
                    Instantiate(templates.BRooms[random], transform.position, templates.BRooms[random].transform.rotation);
                    break;
                case 2:
                    random = Random.Range(0, templates.LRooms.Length);
                    //Instantiate(templates.LRooms[random], transform.position, Quaternion.identity);
                    //Instantiate(templates.LRooms[random], transform.position, templates.LRooms[random].transform.rotation);
                    break;
                case 3:
                    random = Random.Range(0, templates.TRooms.Length);
                    //Instantiate(templates.TRooms[random], transform.position, Quaternion.identity);
                    //Instantiate(templates.TRooms[random], transform.position, templates.TRooms[random].transform.rotation);
                    break;
                case 4:
                    random = Random.Range(0, templates.RRooms.Length);
                    //Instantiate(templates.RRooms[random], transform.position, Quaternion.identity);
                    //Instantiate(templates.RRooms[random], transform.position, templates.RRooms[random].transform.rotation);
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
