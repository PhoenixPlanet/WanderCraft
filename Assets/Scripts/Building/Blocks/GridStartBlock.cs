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
	public override void ActivateBlockBuildingUI()
	{
		for (int i = 0; i < 4; i++) {
			_blockMouseSensors[i].gameObject.SetActive(true);
		}
	}

	public override void DeactivateBlockBuildingUI()
	{
		for (int i = 0; i < 4; i++) {
			_blockMouseSensors[i].gameObject.SetActive(false);
		}
	}

	public override void ActivateBlock()
	{
		
	}

	public override void DeactivateBlock()
	{
		DeactivateBlockBuildingUI();
	}
	#endregion

	#region PrivateMethod
	#endregion
}

}