using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockAnimation : MonoBehaviour
{
    private float pulseDuration = 3f;
    private float pulseScaleValue = 0.8f;
    private Vector3 pulseScale;
    protected Sequence sequence;
    private Transform targetTransform;

    #region PublicVariables

    public virtual void turnAnimation(bool value)
    {
        if (value)
        {
            StartPunchAnimation();
        }
        else
        {
            sequence.Kill();
        };
    }

    private void Start()
    {
        Init();
        StartPunchAnimation();
    }

    public virtual void Init()
    {
        pulseScale = Vector3.one * pulseScaleValue;
        targetTransform = transform.Find("Renderer");
    }

    protected void StartPunchAnimation()
    {
        sequence = DOTween.Sequence();
        sequence.Append(targetTransform.DOScale(pulseScale, pulseDuration / 2).SetEase(Ease.InElastic));
        sequence.Append(targetTransform.DOScale(Vector3.one, pulseDuration / 2).SetEase(Ease.InElastic));
        sequence.SetLoops(-1);
    }
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}