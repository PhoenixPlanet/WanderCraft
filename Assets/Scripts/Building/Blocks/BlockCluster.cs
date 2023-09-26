using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockCluster
{
    #region PublicVariables
    public ESourceType sourceType;
    public Dictionary<EBuildingType, List<BlockAbility>> blockAbilities;

    //GO or individual scripts?
    public BlockCluster(ESourceType sourceType, List<List<BlockAbility>> blockAbilities)
    {
        this.sourceType = sourceType;
        this.blockAbilities = new Dictionary<EBuildingType, List<BlockAbility>>();
        for (int i = 0; i < blockAbilities.Count; i++)
        {
            this.blockAbilities.Add((EBuildingType)i, blockAbilities[i]);
        }
    }
    #endregion
    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Update()
    {

    }
    #endregion
}