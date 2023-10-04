using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    IEnumerator IEdeleter()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    #endregion

    #region PrivateMethod

    private void Start()
    {
        StartCoroutine(nameof(IEdeleter));
    }

    #endregion
}