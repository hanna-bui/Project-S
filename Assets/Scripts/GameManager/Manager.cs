using Movement.Pathfinding;
using UnityEngine;

namespace GameManager
{

    public class Manager : MonoBehaviour
    {
        public readonly int click = Animator.StringToHash("Click");
        
        private GameObject gridObject;
        public NewGrid grid;
    }
}