using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TH.Core {

public class OpenedSideText : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	private ComponentGetter<TextMeshProUGUI> _text =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.This);
	#endregion

	#region PublicMethod
	#endregion
    
	#region PrivateMethod
	private void Update() {
		_text.Get(gameObject).text = "건설 건물: " + (GridManager.Instance.CurrentOpenedBuildingLevel - 1).ToString() + "층";
	}
	#endregion
}

}