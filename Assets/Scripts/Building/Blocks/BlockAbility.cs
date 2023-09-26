using System.Collections;
using System.Collections.Generic;
using TH.Core;
using Unity.VisualScripting;
using UnityEngine;

public class BlockAbility : MonoBehaviour
{
    public EBuildingType m_BuidlingType;
    public ESourceType m_SourceType;
    public Block block => _block.Get(gameObject);

    private ComponentGetter<Block> _block =
        new ComponentGetter<Block>(TypeOfGetter.This);

    public void Init(BlockData blockData) {
        m_BuidlingType = blockData.BuildingType;
        m_SourceType = blockData.SourceType;
    }

    public void SetClusterNumberSynergy(int step) {
        //Debug.Log($"SetClusterNumberSynergy {step}, {m_SourceType}");
        _block.Get(gameObject).SetNormalMaterial(
            Resources.Load<Material>(TH.Core.Constants.ResourcesPath.Materials.BlockColor.INTENSITY[m_SourceType][step])
        );
    }

    public void SetCluster(BlockCluster blockCluster) {
        _block.Get(gameObject).SetCluster(blockCluster);
    }

    public BlockCluster CheckLinkBelow(EBuildingType buildingType, ESourceType sourceType) {
        return _block.Get(gameObject).CheckLinkBelow(buildingType, sourceType);
    }
    public BlockCluster CheckLinkAbove(EBuildingType buildingType, ESourceType sourceType) {
        return _block.Get(gameObject).CheckLinkAbove(buildingType, sourceType);
    }

    protected void Start() {

    }
}    

public enum EBuildingType
{
    Factory,
    Kitchen,
    Dining
}

public enum ESourceType
{
    Fish,
    Meat,
    Plant
}
