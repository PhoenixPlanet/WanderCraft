using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class BlockShopList : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private ObjectGetter _red = new ObjectGetter(TypeOfGetter.ChildByName, "Red/Blocks");
    private ObjectGetter _green = new ObjectGetter(TypeOfGetter.ChildByName, "Green/Blocks");
    private ObjectGetter _blue = new ObjectGetter(TypeOfGetter.ChildByName, "Blue/Blocks");

	[SerializeField] private PriceShowPanel _priceShowPanel;
	#endregion

	#region PublicMethod
	public void Init() {
		var blockDataList = GridManager.Instance.BlockDataList;
		var blockSelectButton = Instantiate(Resources.Load<GameObject>(Constants.ResourcesPath.Prefabs.BLOCK_SELECT_BUTTON_PREFAB_PATH));

		foreach (var blockData in blockDataList) {
			var blockSelectButtonInstance = Instantiate(blockSelectButton, GetRectTransformByType(blockData.SourceType));
			blockSelectButtonInstance.GetComponent<BlockSelectButton>().Init(blockData, OnHover, OnExit, OnClick);
		}

		_priceShowPanel.gameObject.SetActive(false);
	}
	#endregion
    
	#region PrivateMethod
	private RectTransform GetRectTransformByType(ESourceType sourceType) {
		switch (sourceType) {
			case ESourceType.Red:
				return _red.Get(gameObject).GetComponent<RectTransform>();
			case ESourceType.Green:
				return _green.Get(gameObject).GetComponent<RectTransform>();
			case ESourceType.Blue:
				return _blue.Get(gameObject).GetComponent<RectTransform>();
			default:
				return null;
		}
	}

	private RectTransform GetParentRectTransformByType(ESourceType sourceType) {
		switch (sourceType) {
			case ESourceType.Red:
				return _red.Get(gameObject).transform.parent.GetComponent<RectTransform>();
			case ESourceType.Green:
				return _green.Get(gameObject).transform.parent.GetComponent<RectTransform>();
			case ESourceType.Blue:
				return _blue.Get(gameObject).transform.parent.GetComponent<RectTransform>();
			default:
				return null;
		}
	}

	private void OnHover(PropertyData propertyData, ESourceType targetSourceType) {
		_priceShowPanel.gameObject.SetActive(true);
		_priceShowPanel.Show(propertyData);

		float targetX = 
			// GetComponent<RectTransform>().rect.width
			// / (Enum.GetNames(typeof(ESourceType)).Length + 1) 
			// * (int)targetSourceType;
			GetParentRectTransformByType(targetSourceType).anchoredPosition.x;
		RectTransform rectTransform = _priceShowPanel.GetComponent<RectTransform>();

		Vector2 targetPosition = new Vector2(
			targetX + rectTransform.rect.x / 2, 
			GetComponent<RectTransform>().rect.height
		);
		rectTransform.anchoredPosition = targetPosition;
	}

	private void OnExit() {
		_priceShowPanel.gameObject.SetActive(false);
	}

	private void OnClick(BlockDataSO blockData) {
		//GridManager.Instance.SelectBlockData(blockData);
	}

	#endregion
}

}