using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class SelectLevels : MonoBehaviour
    {
        public Slider slider;

        public void Select()
        {
            GameManager.instance.lvl = (int)slider.value;
        }
    }
}
