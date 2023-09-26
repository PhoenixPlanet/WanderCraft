using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class AboutText : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	#endregion

	#region PublicMethod
	#endregion
    
	#region PrivateMethod
	private void Start() {
		StartCoroutine(DestroySelf());
	}

	private IEnumerator DestroySelf() {
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}
	#endregion
}

}