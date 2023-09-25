using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LinkedBlock
{
    #region PublicVariables
    public ESourceType sourceType;
    public List<GameObject> objects;
    public List<DiningAbility> diningAbilities;
    public List<FactoryAbility> factoryAbilities;
    public List<KitchenAbility> kitchenAbilities;
    public List<StorageAbility> storageAbilities;
    //GO or individual scripts?
    public LinkedBlock(ESourceType sourceType, List<DiningAbility> diningAbilities, List<FactoryAbility> factoryAbilities, List<KitchenAbility> kitchenAbilities, List<StorageAbility> storageAbilities)
    {
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