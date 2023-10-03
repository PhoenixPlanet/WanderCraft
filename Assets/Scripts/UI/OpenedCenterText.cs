using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TH.Core {

public class OpenedCenterText : MonoBehaviour
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
		_text.Get(gameObject).text = "중앙 타워: " + GridManager.Instance.CurrentCenterLevel.ToString() + "층";
	}
	#endregion
}

}