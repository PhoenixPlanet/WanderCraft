using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TH.Core {

public class BlockSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	private ComponentGetter<Button> _button =
		new ComponentGetter<Button>(TypeOfGetter.This);
	private ComponentGetter<TextMeshProUGUI> _text =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Title");
	private ComponentGetter<Image> _image =
		new ComponentGetter<Image>(TypeOfGetter.ChildByName, "Image");

	private PropertyData _blockPrice;
	private BlockDataSO _blockData;

	private Action<PropertyData, ESourceType> _onHover;
	private Action _onExit;
	private Action<BlockDataSO> _onClick;
	#endregion

	#region PublicMethod
	public void Init(BlockDataSO blockData, Action<PropertyData, ESourceType> onHover, Action onExit, Action<BlockDataSO> onClick) {
		_button.Get(gameObject).onClick.AddListener(() => {
			GridManager.Instance.ChangeState(GridManager.BuildingState.Building);
			GridManager.Instance.SelectBlockData(blockData);
		});

		_text.Get(gameObject).text = blockData.blockName;
		_image.Get(gameObject).sprite = blockData.uiSprite;

		_blockPrice = blockData.blockPrice;
		_blockData = blockData;

		_onHover = onHover;
		_onExit = onExit;
		_onClick = onClick;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_onHover?.Invoke(_blockPrice, _blockData.SourceType);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_onExit?.Invoke();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_onClick?.Invoke(_blockData);
	}
	#endregion

	#region PrivateMethod
	#endregion
}

}