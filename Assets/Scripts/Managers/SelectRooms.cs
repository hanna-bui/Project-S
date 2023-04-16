using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class SelectRooms : MonoBehaviour
    {
        public Slider slider;
        
        public void Select()
        {
            GameManager.instance.scale = (int)slider.value;
        }
    }
}