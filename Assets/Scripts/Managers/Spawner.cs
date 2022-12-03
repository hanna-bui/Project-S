/*
Spawning the selected character into scene

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private void Start()
    {
        Instantiate(GameManager.instance.currCharacter.prefab, transform.position, Quaternion.identity);
    }
}
