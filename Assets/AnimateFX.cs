using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFX : MonoBehaviour
{
    public int index;

    private Animator anim;
    [SerializeField] protected AnimationClip[] animations;

    private void Start()
    {
        anim = GetComponent<Animator>();
        var ac = anim.runtimeAnimatorController;
        animations = ac.animationClips;
        Array.Sort(animations, new AnimationCompare());
        Play();
    }

    public void Play()
    {
        anim.Play(animations[index].name);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Helps For Sorting Animation Clips by Name
    /// </summary>
    class AnimationCompare : IComparer<AnimationClip>
    {
        public int Compare(AnimationClip x, AnimationClip y)
        {
            if (x == null || y == null)
            {
                return 0;
            }
          
            // CompareTo() method
            return string.Compare(x.name, y.name, StringComparison.Ordinal);
          
        }
    }
}
