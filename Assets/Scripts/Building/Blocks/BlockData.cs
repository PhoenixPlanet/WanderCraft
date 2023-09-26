using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SO/BlockData", order = 1)]
public class BlockData : ScriptableObject
{
    #region PublicVariables
    public EBuildingType BuildingType;
    public ESourceType SourceType;
    public GameObject FloatingPrefab;
    public GameObject RealPrefab;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}