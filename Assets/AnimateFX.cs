using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFX : MonoBehaviour
{
    public int index;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Play()
    {
        anim.Play("DualWieldFx");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
