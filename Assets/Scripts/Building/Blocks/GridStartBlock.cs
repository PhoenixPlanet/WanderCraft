using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class GridStartBlock : Block
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	#endregion

	#region PublicMethod
	public override void ActivateBlock()
	{
		for (int i = 0; i < 4; i++) {
			_blockMouseSensors[i].gameObject.SetActive(true);
		}
	}

	public override void DeactivateBlock()
	{
		for (int i = 0; i < 4; i++) {
			_blockMouseSensors[i].gameObject.SetActive(false);
		}
	}
	#endregion

	#region PrivateMethod
	#endregion
}

}