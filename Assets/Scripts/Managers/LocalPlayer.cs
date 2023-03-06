using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class LocalPlayer : NetworkBehaviour
{
   
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

    }
}
