using System.Collections;
using System.Collections.Generic;
using Movement.Pathfinding;
using Characters.Enemy;
using UnityEngine;

namespace GameManager_Hide
{
    public class Manager : MonoBehaviour
    {
        public readonly int click = Animator.StringToHash("Click");
        
        private GameObject gridObject;
        public NewGrid grid;

        public Hashtable Enemies { get; set; }

        private void Start()
        {
            Enemies = new Hashtable();
            var enemyObject = GameObject.Find("Enemies");
            if (enemyObject != null)
            {
                SettingUpEnemies(enemyObject.transform);
                foreach (DictionaryEntry de in Enemies)
                    Debug.Log(de.Key, (Object)de.Value);
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