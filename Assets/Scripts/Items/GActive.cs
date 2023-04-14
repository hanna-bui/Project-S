
using Managers;
using UnityEngine;

namespace Items
{
    // My first attempt - Doesn't do anything
    public class GActive : Items
    {
        private GameManager manager;
        // Start is called before the first frame update
        private void Start()
        {
            manager = GameManager.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("You Won!");
                manager.Win();
            }
        }
    }
}