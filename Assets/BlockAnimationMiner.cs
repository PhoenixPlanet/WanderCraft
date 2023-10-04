using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockAnimationMiner : blockAnimation
{
    #region PublicVariables
    public Animator animator;
    public override void turnAnimation(bool value)
    {
        if (value)
        {
            StartPunchAnimation();
            animator.SetBool("isOn", true);
        }
        else {

            animator.SetBool("isOn", false);
            sequence.Kill();
        }
        return;
    }

    public override void Init()
    {
        base.Init();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("isOn", true);

    }



    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}