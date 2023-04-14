
using Managers;
using UnityEngine;

namespace Items
{
    public class GInactive : Items
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
                manager.Next();
            }
        }
    }
}