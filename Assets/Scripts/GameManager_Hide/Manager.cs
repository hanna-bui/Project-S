using System.Collections;
using System.Collections.Generic;
using Characters;
using Movement.Pathfinding;
using Characters.Enemy;
using UnityEngine;

namespace GameManager_Hide
{
    public class Manager : MonoBehaviour
    {
        #region Goals
        public Hashtable Players { get; set; }
        #endregion

        private void Start()
        {
            
            #region Goals
            Players = new Hashtable();
            var charactersObject = GameObject.Find("Characters");
            if (charactersObject != null)
                SettingUpPlayers(charactersObject.transform);
            #endregion
        }
        
        private void SettingUpPlayers(Transform p)
        {
            foreach (Transform child in p)
            {
                Players.Add(child.gameObject, child.GetComponent<Character>());
            }
        }
    }
}