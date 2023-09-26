using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TH.Core {

public class ScoreText : MonoBehaviour
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
		_text.Get(gameObject).text = GridManager.Instance.Money.ToString();
	}
	#endregion
}

}