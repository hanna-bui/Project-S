using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    /* DoorDirection (doorDir) indicates the door requirements for a room spawned at this point
     * 1) Requires BOTTOM door
     * 2) Requires LEFT door
     * 3) Requires TOP door
     * 4) Requires RIGHT door
     */
    public int doorDir;

    private void Update()
    {
        switch (doorDir)
        {
            case 1:
                
                break;
            case 2:
                break;
            case 3: 
                break;
            case 4:
                break;
            default:
                break;
        }
    }
}
