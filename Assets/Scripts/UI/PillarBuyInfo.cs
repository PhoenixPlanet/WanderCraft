using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class PillarBuyInfo : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	private ComponentGetter<TextMesh> _red =
		new ComponentGetter<TextMesh>(TypeOfGetter.ChildByName, "Info/Red/text");
	private ComponentGetter<TextMesh> _green =
		new ComponentGetter<TextMesh>(TypeOfGetter.ChildByName, "Info/Green/text");
	private ComponentGetter<TextMesh> _blue =
		new ComponentGetter<TextMesh>(TypeOfGetter.ChildByName, "Info/Blue/text");
	#endregion

	#region PublicMethod
	#endregion
    
	#region PrivateMethod
	private void Update() {
		if (GridManager.Instance.GetCenterBlockPrice().red.ToString().Length > 4) {
			_red.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().red.ToString("G4");
		}
		else {
			_red.Get(gameObject).text = GridManager.Instance.CurrentProperty.red.ToString();
		}

		if (GridManager.Instance.GetCenterBlockPrice().green.ToString().Length > 4) {
			_green.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().green.ToString("G4");
		}
		else {
			_green.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().green.ToString();
		}

		if (GridManager.Instance.GetCenterBlockPrice().blue.ToString().Length > 4) {
			_blue.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().blue.ToString("G4");
		}
		else {
			_blue.Get(gameObject).text = GridManager.Instance.GetCenterBlockPrice().blue.ToString();
		}
	}
	#endregion
}

}