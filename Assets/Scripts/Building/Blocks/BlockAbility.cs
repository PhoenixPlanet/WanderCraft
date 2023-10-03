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

    public bool IsActivated => _isActivated;

    private bool _isActivated = true;
    private bool _lastActivated = true;

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
        _isActivated = true;
    }

    // private void OnTriggerEnter(Collider other) {
    //     Debug.Log("OnTriggerEnter Water");
    //     if (other.gameObject.CompareTag("Water")) {
    //         _isActivated = false;
    //         GridManager.Instance.CalculateAgain();
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     Debug.Log("OnTriggerExit Water");
    //     if (other.gameObject.CompareTag("Water")) {
    //         _isActivated = true;
    //         GridManager.Instance.CalculateAgain();
    //     }
    // }

    private void Update() {
        GameObject water = GameObject.Find("WaterGroup");

        _lastActivated = _isActivated;
        if (water.transform.position.y > transform.position.y) {
            _isActivated = false;
            if (_lastActivated != _isActivated) {
                GridManager.Instance.CalculateAgain();
            }
        } else {
            _isActivated = true;
            if (_lastActivated != _isActivated) {
                GridManager.Instance.CalculateAgain();
            }
        }
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
