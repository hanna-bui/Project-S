using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private void spawn()
    {
        GameObject player = Instantiate(GameManager.instance.currCharacter.prefab, transform.position, transform.rotation);
        player.transform.localScale = new Vector3(30, 30, 0);
    }
}
