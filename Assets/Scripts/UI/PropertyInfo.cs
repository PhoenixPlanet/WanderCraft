using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TH.Core {

public class PropertyInfo : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	private ComponentGetter<TextMeshProUGUI> _red =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Info/Red/text");

	private ComponentGetter<TextMeshProUGUI> _green =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Info/Green/text");

	private ComponentGetter<TextMeshProUGUI> _blue =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Info/Blue/text");
	#endregion

	#region PublicMethod
	#endregion
    
	#region PrivateMethod
	private void Update() {
		_red.Get(gameObject).text = $"{GridManager.Instance.CurrentProperty.red}";
		_green.Get(gameObject).text = $"{GridManager.Instance.CurrentProperty.green}";
		_blue.Get(gameObject).text = $"{GridManager.Instance.CurrentProperty.blue}";
	}
	#endregion
}

}