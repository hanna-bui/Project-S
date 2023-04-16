using UnityEngine;

namespace Managers
{
    public class SelectCharacter : MonoBehaviour
    {
        public GameObject character;

        public void Press()
        {
            GameManager.instance.SetCharacter(character);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime*10);
        }
    }
}

