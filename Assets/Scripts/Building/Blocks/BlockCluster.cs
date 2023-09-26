using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BlockCluster
{
    #region PublicVariables
    public static readonly int[] threashholds = { 2, 4, 8};

    public int level;
    public EBuildingType buildingType;
    public ESourceType sourceType;
    public List<BlockAbility> blockAbilities;

    //GO or individual scripts?
    public BlockCluster(int level, EBuildingType buildingType, ESourceType sourceType)
    {
        this.level = level;
        this.buildingType = buildingType;
        this.sourceType = sourceType;

        blockAbilities = new List<BlockAbility>();
    }
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    public void AddBlock(BlockAbility blockAbility)
    {
        blockAbilities.Add(blockAbility);
    }

    public int BlockNum() {
        return blockAbilities.Count;
    }

    public void ApplyCluster() {
        if (blockAbilities.Count == 0) {
            return;
        }

        if (blockAbilities.Count > 0 && blockAbilities.Count < threashholds[0]) {
            SetBlocksSynergyColor(0);
        }
        else if (blockAbilities.Count >= threashholds[0] && blockAbilities.Count < threashholds[1]) {
            SetBlocksSynergyColor(1);
        }
        else if (blockAbilities.Count >= threashholds[1] && blockAbilities.Count < threashholds[2]) {
            SetBlocksSynergyColor(2);
        }
        else if (blockAbilities.Count >= threashholds[2]) {
            SetBlocksSynergyColor(3);
        }

        for (int i = 0; i < blockAbilities.Count; i++) {
            blockAbilities[i].SetCluster(this);
        }
    }

    public void CheckLink() {
        if (buildingType != EBuildingType.Dining) {
            return;
        }

        HashSet<BlockCluster> belowClusters = new HashSet<BlockCluster>();
        foreach (var blockAbility in blockAbilities) {
            BlockCluster bc = blockAbility.CheckLinkBelow((EBuildingType)((int)EBuildingType.Dining - 1));
            if (bc != null) {
                belowClusters.Add(bc);
            }
        }
    }

    public void CheckLinkBelow(EBuildingType targetBuildingType) {
        if (buildingType == EBuildingType.Dining) {
            return;
        }

        foreach (var blockAbility in blockAbilities) {
            //blockAbility.CheckLinkBelow();
        }
    }
    #endregion

    #region PrivateMethod
    private void SetBlocksSynergyColor(int step) {
        foreach (var blockAbility in blockAbilities) {
            blockAbility.SetClusterNumberSynergy(step);
        }
    }
    #endregion
}