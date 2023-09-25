using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageAbility : BlockAbility
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] 
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    #endregion
    protected override void Start()
    {
        m_BuidlingType = EBuildingType.Storage;
    }
}

