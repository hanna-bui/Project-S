using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Character[] characters;

    public Character currCharacter;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(currCharacter == null)
        {
            currCharacter = characters[0];
        }
    }

    public void SetCharacter(Character character)
    {
        currCharacter = character;
    }
}
