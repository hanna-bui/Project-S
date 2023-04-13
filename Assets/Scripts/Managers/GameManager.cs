using System.Collections;
using Characters;
using Characters.Enemy;
using Mirror;
using Movement.Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager instance;
        public Player[] characters;

        public Player currCharacter;
        

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
            if (currCharacter == null)
            {
                var temp = GameObject.Find("Ninja");
                
                if (temp == null) temp = GameObject.Find("Samurai");
                else currCharacter = temp.GetComponent<Ninja>();
                
                if (temp == null) temp = GameObject.Find("Monk");
                else currCharacter = temp.GetComponent<Samurai>();
            }
        }

        public void SetCharacter(Player character)
        {
            currCharacter = character;
        }
    }
}