using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TH.Core {

public class MainUI : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	private ComponentGetter<Button> _normalButton =
		new ComponentGetter<Button>(TypeOfGetter.ChildByName, "BottomPanel/GameModePanel/NormalButton");
	private ComponentGetter<Button> _buildingButton =
		new ComponentGetter<Button>(TypeOfGetter.ChildByName, "BottomPanel/GameModePanel/BuildingButton");
	private ComponentGetter<Button> _buyButton = 
		new ComponentGetter<Button>(TypeOfGetter.ChildByName, "BottomPanel/BuyButton");
	#endregion

	#region PublicMethod
	#endregion
    
	#region PrivateMethod
	private void Start() {
		_normalButton.Get(gameObject).onClick.AddListener(() => {
			_normalButton.Get(gameObject).interactable = false;
			_buildingButton.Get(gameObject).interactable = true;

			GridManager.Instance.ChangeState(GridManager.BuildingState.Normal);
		});
		_buildingButton.Get(gameObject).onClick.AddListener(() => {
			_normalButton.Get(gameObject).interactable = true;
			_buildingButton.Get(gameObject).interactable = false;

			GridManager.Instance.ChangeState(GridManager.BuildingState.Building);
		});

		_buyButton.Get(gameObject).onClick.AddListener(() => {
			GridManager.Instance.BuyCenterBlock();
		});
	}
	#endregion
}

}