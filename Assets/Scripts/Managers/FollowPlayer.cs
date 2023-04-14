using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public void AttachToPlayer(GameObject player)
    {
        transform.SetParent(player.transform);
        transform.rotation = player.transform.rotation;
        transform.position = player.transform.position + new Vector3(0, 0, -10);
        transform.SetParent(player.transform);
        transform.localScale = new Vector3(0.05f, 0.05f, 1);
        GetComponent<Camera>().nearClipPlane = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
