using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerUI : MonoBehaviour
{
    public Character player;

    [FormerlySerializedAs("HP")] public GameObject hpUI;

    // ReSharper disable once InconsistentNaming
    public GameObject Heart;

    private readonly List<GameObject> playerHearts = new();

    private int hp;
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void InitalHearts(int hpValue)
    {
        hp = hpValue;
        
        var posX = 0;
        
        for (; hpValue > 0; hpValue -= 4)
        {
            var partial = Math.Max(0, hpValue-4) > 0 ? 4 : Math.Max(0, hpValue);
            
            var heart = Instantiate(Heart, hpUI.transform, true);
            heart.transform.localScale = Vector3.one;
            var t = heart.GetComponent<RectTransform>();
            t.localPosition = new Vector3(posX, 0, 0);
            playerHearts.Add(heart);
            
            heart.GetComponent<SpriteChanger>().ChangeHeart(partial);
            
            posX += 70;
        }
    }

    public void UpdateHeart(int chp)
    {
        foreach (var t in playerHearts)
        {
            var partial = Math.Max(0, chp-4) > 0 ? 4 : Math.Max(0, chp);
            chp -= 4;
            
            t.GetComponent<SpriteChanger>().ChangeHeart(partial);
        }
    }
}
