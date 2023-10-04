using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TH.Core {

public class PriceShowPanel : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private ComponentGetter<TextMeshProUGUI> _redPrice 
		= new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Red/text");
	private ComponentGetter<TextMeshProUGUI> _greenPrice
		= new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Green/text");
	private ComponentGetter<TextMeshProUGUI> _bluePrice
		= new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Blue/text");
	#endregion

	#region PublicMethod
	public void Show(PropertyData price) {
		_redPrice.Get(gameObject).text = price.GetPropertyValue(ESourceType.Red).ToString();
		_greenPrice.Get(gameObject).text = price.GetPropertyValue(ESourceType.Green).ToString();
		_bluePrice.Get(gameObject).text = price.GetPropertyValue(ESourceType.Blue).ToString();

		if (price.GetPropertyValue(ESourceType.Red) > 
			GridManager.Instance.CurrentProperty.red) {
			_redPrice.Get(gameObject).color = Color.red;
		} else {
			_redPrice.Get(gameObject).color = Color.black;
		}

		if (price.GetPropertyValue(ESourceType.Green) > 
			GridManager.Instance.CurrentProperty.green) {
			_greenPrice.Get(gameObject).color = Color.red;
		} else {
			_greenPrice.Get(gameObject).color = Color.black;
		}

		if (price.GetPropertyValue(ESourceType.Blue) > 
			GridManager.Instance.CurrentProperty.blue) {
			_bluePrice.Get(gameObject).color = Color.red;
		} else {
			_bluePrice.Get(gameObject).color = Color.black;
		}
	}
	#endregion
    
	#region PrivateMethod
	#endregion
}

}
