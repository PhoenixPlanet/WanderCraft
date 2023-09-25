using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BlockAbility : MonoBehaviour
{
    public EBuildingType m_BuidlingType;
    public ESourceType m_SourceType;

    protected abstract void Start();
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
