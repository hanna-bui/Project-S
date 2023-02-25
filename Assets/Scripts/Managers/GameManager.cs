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
        public readonly int click = Animator.StringToHash("Click");
        
        private GameObject gridObject;
        public NewGrid grid;

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
            Enemies = new Hashtable();
            var enemyObject = GameObject.Find("Enemies");
            if (enemyObject == null) return;
            
            SettingUpEnemies(enemyObject.transform);
            #endregion
        }

        public void SetCharacter(Character character)
        {
            currCharacter = character;
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