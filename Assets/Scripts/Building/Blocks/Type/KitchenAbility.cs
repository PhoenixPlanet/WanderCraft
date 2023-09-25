using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenAbility : BlockAbility
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
        m_BuidlingType = EBuildingType.Kitchen;
    }

}