using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GameObject player = Instantiate(GameManager.instance.currCharacter.prefab, transform.position, transform.rotation);
        player.transform.localScale = new Vector3(20, 20, 0);
    }
}
