using System.Collections;
using Characters;
using Characters.Enemy;
using Movement.Pathfinding;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public Character[] characters;

        public Character currCharacter;
    
        #region Goals
        private GameObject gridObject;
        public NewGrid grid;

        public Hashtable Players { get; set; }
        public Hashtable Enemies { get; set; }
        #endregion

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
        
            #region Goals
            Players = new Hashtable();
            var charactersObject = GameObject.Find("Characters");
            if (charactersObject != null)
                SettingUpPlayers(charactersObject.transform);
            Enemies = new Hashtable();
            var enemyObject = GameObject.Find("Enemies");
            if (enemyObject != null)
                SettingUpEnemies(enemyObject.transform);
            #endregion
        }

        public void SetCharacter(Character character)
        {
            currCharacter = character;
        }
        
        private void SettingUpPlayers(Transform p)
        {
            foreach (Transform child in p)
            {
                Players.Add(child.gameObject, child.GetComponent<Character>());
            }
        }
        private void SettingUpEnemies(Transform e)
        {
            foreach (Transform child in e)
            {
                Enemies.Add(child.gameObject, child.GetComponent<Enemy>());
            }
        }
    }
}