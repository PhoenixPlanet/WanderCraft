using System;
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

        List<HashSet<BlockCluster>> linkedBlockClusters = new List<HashSet<BlockCluster>>();
        for (int i = 0; i < Enum.GetNames(typeof(EBuildingType)).Length - 1; i++) {
            linkedBlockClusters.Add(new HashSet<BlockCluster>());
        }

        linkedBlockClusters[Enum.GetNames(typeof(EBuildingType)).Length - 1].Add(this);

        for (int i = Enum.GetNames(typeof(EBuildingType)).Length - 1; i >= 1; i--) {
            foreach (var c in linkedBlockClusters[i]) {
                linkedBlockClusters[i - 1].UnionWith(c.GetAllBelowClusters((EBuildingType)(i - 1)));
            }    
        }
    }

    public HashSet<BlockCluster> GetAllBelowClusters(EBuildingType targetBuildingType) {
        HashSet<BlockCluster> belowClusters = new HashSet<BlockCluster>();
        foreach (var blockAbility in blockAbilities) {
            BlockCluster bc = blockAbility.CheckLinkBelow(targetBuildingType);
            if (bc != null) {
                belowClusters.Add(bc);
            }
        }
        return belowClusters;
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