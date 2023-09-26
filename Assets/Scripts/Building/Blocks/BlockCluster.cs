using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockCluster
{
    #region PublicVariables
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
        
    }
    #endregion

    #region PrivateMethod
    #endregion
}