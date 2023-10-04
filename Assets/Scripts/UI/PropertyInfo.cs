using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

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

		private ComponentGetter<TextMeshProUGUI> _bigBG =
			new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.GlobalByName, "CamRig/MainCamera/Title/Canvas/text");
    #endregion

        #region PublicMethod
        #endregion

        #region PrivateMethod
    private void Update() {
		if (GridManager.Instance.CurrentProperty.red.ToString().Length > 5) {
			_red.Get(gameObject).text = GridManager.Instance.CurrentProperty.red.ToString("G4", CultureInfo.InvariantCulture);
		}
		else {
			_red.Get(gameObject).text = GridManager.Instance.CurrentProperty.red.ToString();
		}

		if (GridManager.Instance.CurrentProperty.green.ToString().Length > 5) {
			_green.Get(gameObject).text = GridManager.Instance.CurrentProperty.green.ToString("G4", CultureInfo.InvariantCulture);
		}
		else {
			_green.Get(gameObject).text = GridManager.Instance.CurrentProperty.green.ToString();
		}

		if (GridManager.Instance.CurrentProperty.blue.ToString().Length > 5) {
			_blue.Get(gameObject).text = GridManager.Instance.CurrentProperty.blue.ToString("G4", CultureInfo.InvariantCulture);
		}
		else {
			_blue.Get(gameObject).text = GridManager.Instance.CurrentProperty.blue.ToString();
		}

		//==
        if (GridManager.Instance.CurrentProperty.blue.ToString().Length > 4 && GridManager.Instance.CurrentProperty.green.ToString().Length > 4 && GridManager.Instance.CurrentProperty.red.ToString().Length > 4)
        {
            _bigBG.Get(gameObject).text =
                     GridManager.Instance.CurrentProperty.red.ToString("G4", CultureInfo.InvariantCulture) +"\n" +
					 GridManager.Instance.CurrentProperty.green.ToString("G4", CultureInfo.InvariantCulture) + "\n" +
					 GridManager.Instance.CurrentProperty.blue.ToString("G4", CultureInfo.InvariantCulture);
        }
        else
            {
                _bigBG.Get(gameObject).text =
                GridManager.Instance.CurrentProperty.red.ToString() + "\n" +
                GridManager.Instance.CurrentProperty.green.ToString() + "\n" +
                GridManager.Instance.CurrentProperty.blue.ToString();
            }
		//==
        }
	#endregion
}

}