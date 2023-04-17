using System.Collections.Generic;
using Characters;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public Character player;

    public GameObject HP;

    public GameObject Heart;

    private readonly List<GameObject> playerHearts = new();

    private int hp;

    // ReSharper disable once ParameterHidesMember
    // ReSharper disable once InconsistentNaming
    public void AddHeart(int HPValue)
    {
        hp += (HPValue - hp);
        var i = hp / 4;
        var r = hp % 4;

        var posx = 0;

        for (var j = playerHearts.Count; j < i; ++j)
        {
            var heart = Instantiate(Heart);
            heart.transform.SetParent(HP.transform);
            heart.transform.localScale = Vector3.one;
            var t = heart.GetComponent<RectTransform>();
            t.localPosition = new Vector3(posx, 0, 0);
            playerHearts.Add(heart);
            posx += 70;
        }

        if (r > 0)
        {
            var heart = Instantiate(Heart);
            heart.transform.SetParent(HP.transform);
            heart.transform.localScale = Vector3.one;
            var t = heart.GetComponent<RectTransform>();
            t.localPosition = new Vector3(posx, 0, 0);
            heart.GetComponent<SpriteChanger>().ChangeHeart(r);
            playerHearts.Add(heart);
        }
    }

    public void UpdateHeart(int chp)
    {
        var r = chp % 4;

        for(var i = 0; i < playerHearts.Count; i++)
        {
            if (i < chp / 4)
            {
                playerHearts[i].GetComponent<SpriteChanger>().ChangeHeart(4);
            }
            else if (i==chp/4)
            {
                playerHearts[i].GetComponent<SpriteChanger>().ChangeHeart(r);
                r = 0;
            }
            else
            {
                playerHearts[i].GetComponent<SpriteChanger>().ChangeHeart(r);
            }
        }
    }
}
