using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private Vector3 startPos;
    private void Awake()
    {
        Instantiate(GameManager.instance.currCharacter.prefab, transform.position, Quaternion.identity);
        startPos = GameObject.Find("Walkable").transform.position;
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = startPos;
    }
}
