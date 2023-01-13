using UnityEngine;

namespace Characters
{
    public class Player : MonoBehaviour
    {
        private Character self;

        // Start is called before the first frame update
        void Start()
        {
            self = new Ninja();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}