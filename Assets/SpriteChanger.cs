using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    public Image image;
    public Sprite[] heartSprits;

    public void ChangeHeart(int r)
    {
        image.sprite = heartSprits[r];
    }
}
