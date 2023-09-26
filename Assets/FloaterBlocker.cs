using System.Collections;
using System.Collections.Generic;
using TH.Core;
using UnityEngine;

public class FloaterBlocker : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private float _sphereRadius;
    private SphereCollider _sphereCollider;
    
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Start()
    {
        _sphereRadius = GridManager.Instance.GridSize.x;
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _sphereRadius;
    }




    #endregion
}