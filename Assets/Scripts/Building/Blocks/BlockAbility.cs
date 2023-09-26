using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockAbility : MonoBehaviour
{
    public EBuildingType m_BuidlingType;
    public ESourceType m_SourceType;

    public void Init(BlockData blockData) {
        m_BuidlingType = blockData.BuildingType;
        m_SourceType = blockData.SourceType;
    }

    protected void Start() {

    }
}    

public enum EBuildingType
{
    Factory,
    Storage,
    Kitchen,
    Dining
}

public enum ESourceType
{
    Fish,
    Meat,
    Plant
}
