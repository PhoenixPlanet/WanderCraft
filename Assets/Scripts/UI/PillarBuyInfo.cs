using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TH.Core {

public class PillarBuyInfo : MonoBehaviour
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
		if (GridManager.Instance.GetCenterBlockPrice().red.ToString().Length > 5) {
			_red.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().red.ToString("G4");
		}
		else {
			_red.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().red.ToString();
		}

		if (GridManager.Instance.GetCenterBlockPrice().green.ToString().Length > 5) {
			_green.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().green.ToString("G4");
		}
		else {
			_green.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().green.ToString();
		}

		if (GridManager.Instance.GetCenterBlockPrice().blue.ToString().Length > 5) {
			_blue.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().blue.ToString("G4");
		}
		else {
			_blue.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().blue.ToString();
		}

		if (GridManager.Instance.GetCenterBlockPrice().red > 
			GridManager.Instance.CurrentProperty.red) {
			_red.Get(gameObject).color = Color.red;
		} else {
			_red.Get(gameObject).color = Color.black;
		}

		if (GridManager.Instance.GetCenterBlockPrice().green > 
			GridManager.Instance.CurrentProperty.green) {
			_green.Get(gameObject).color = Color.red;
		} else {
			_green.Get(gameObject).color = Color.black;
		}

		if (GridManager.Instance.GetCenterBlockPrice().blue > 
			GridManager.Instance.CurrentProperty.blue) {
			_blue.Get(gameObject).color = Color.red;
		} else {
			_blue.Get(gameObject).color = Color.black;
		}
	}
	#endregion
}

}