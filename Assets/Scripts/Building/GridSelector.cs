using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class GridSelector : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField] private Material _green;
	[SerializeField] private Material _red;

	private ComponentGetter<MeshRenderer> _meshRenderer
		= new ComponentGetter<MeshRenderer>(TypeOfGetter.ChildByName, "Cube");
	#endregion

	#region PublicMethod
	public void SetColor(bool isAvailable) {
		_meshRenderer.Get(gameObject).material = isAvailable ? _green : _red;
	}
	#endregion
    
	#region PrivateMethod
	#endregion
}

}