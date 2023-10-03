using System.Collections;
using System.Collections.Generic;
using TH.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SO/BlockData", order = 1)]
public class BlockData : ScriptableObject
{
    #region PublicVariables
    public EBuildingType BuildingType;
    public ESourceType SourceType;
    public GameObject FloatingPrefab;
    public GameObject RealPrefab;

    public PropertyData blockPrice;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}