using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class BlockShopList : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	#endregion

	#region PublicMethod
	public void Init() {
		var blockDataList = GridManager.Instance.BlockDataList;
		var blockSelectButton = Instantiate(Resources.Load<GameObject>(Constants.ResourcesPath.Prefabs.BLOCK_SELECT_BUTTON_PREFAB_PATH));

		foreach (var blockData in blockDataList) {
			var blockSelectButtonInstance = Instantiate(blockSelectButton, transform);
			blockSelectButtonInstance.GetComponent<BlockSelectButton>().Init(blockData, OnHover, OnExit, OnClick);
		}
	}
	#endregion
    
	#region PrivateMethod
	private void OnHover(PropertyData propertyData) {
		
	}

	private void OnExit() {
		
	}

	private void OnClick(BlockDataSO blockData) {
		GridManager.Instance.SelectBlockData(blockData);
	}


	#endregion
}

}