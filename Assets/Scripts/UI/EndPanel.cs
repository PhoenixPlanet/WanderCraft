using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class EndPanel : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	#endregion

	#region PublicMethod
	public void ReloadScene() {
		Destroy(GridManager.Instance.gameObject);
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
	#endregion
    
	#region PrivateMethod
	#endregion
}

}