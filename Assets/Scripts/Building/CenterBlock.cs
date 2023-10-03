using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TH.Core {

public class CenterBlock : MonoBehaviour
{
    #region PublicVariables
	public const float HEIGHT = 1f;
	#endregion

	#region PrivateVariables
	[ShowInInspector] private int _level;
	#endregion

	#region PublicMethod
	public void Init(int level, Vector3 position) {
		_level = level;
		transform.localPosition = position;
	}
	#endregion
    
	#region PrivateMethod
	#endregion
}

}