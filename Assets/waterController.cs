using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterController : MonoBehaviour
{
    #region PublicVariables
    public GameObject waterObject;
    [SerializeField] private float speed = 0;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Update()
    {
        waterObject.transform.position = waterObject.transform.position + Vector3.up * Time.deltaTime * speed;
    }
    #endregion
}