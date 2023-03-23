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
            if (currCharacter == null)
            {
                var temp = GameObject.Find("Ninja");
                
                if (temp == null) temp = GameObject.Find("Samurai");
                else currCharacter = temp.GetComponent<Ninja>();
                
                if (temp == null) temp = GameObject.Find("Monk");
                else currCharacter = temp.GetComponent<Samurai>();
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

        private void Update()
        {
            if (grid == null)
            {
                
            }
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