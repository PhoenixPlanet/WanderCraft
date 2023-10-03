using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TH.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SO/BlockDataSO", order = 1)]
public class BlockDataSO : ScriptableObject
{
    #region PublicVariables

    [Title("Strings")]
    public string blockName;
    [Multiline(4)]
    public string description;
    
    [Title("Enums")]
    [EnumToggleButtons]
    public EBuildingType BuildingType;
    [EnumToggleButtons]
    public ESourceType SourceType;
    
    [TitleGroup("Prefabs")]
    [HorizontalGroup("Prefabs/Split"), VerticalGroup("Prefabs/Split/Left"), Required, PreviewField(100)]
    public GameObject FloatingPrefab;
    [HorizontalGroup("Prefabs/Split"), VerticalGroup("Prefabs/Split/Right"), Required, PreviewField(100)]
    public GameObject RealPrefab;
    
    [Title("Prices")]
    public PropertyData blockPrice;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}