using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockAnimationMiner : blockAnimation
{
    #region PublicVariables
    public GameObject spike;
    private float yOffset = 0.2f;

    protected override void StartPunchAnimation()
    {
        sequence = DOTween.Sequence();
        float originalY = spike.transform.localPosition.y;
        sequence.Append(spike.transform.DOLocalMoveY(originalY + yOffset, pulseDuration / 2).SetEase(Ease.InElastic));
        sequence.Append(spike.transform.DOLocalMoveY(originalY, pulseDuration / 2).SetEase(Ease.InElastic));

        sequence.SetLoops(-1);
    }

}



    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
