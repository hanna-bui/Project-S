using UnityEngine;

namespace Characters.Colliders
{
    public class RangeCollider : MonoBehaviour
    {
        protected Agent agent;
        
        private void Start()
        {
            agent = transform.parent.gameObject.GetComponent<Agent>();
        }
        
        public CircleCollider2D SetupCollider(float rad)
        {
            transform.localScale = new Vector3(0.07f,0.07f,1);
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;

            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = rad*5;
            circle.isTrigger = true;

            return circle;
        }
    }
}