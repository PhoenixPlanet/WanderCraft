using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TH.Core {

public class SceneLoader : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	#endregion

	#region PublicMethod
	public void DestroyEverythingAndGoLobby() {
		// Destroy all objects
		foreach (var obj in FindObjectsOfType<GameObject>()) {
			if (obj.name == "CamRig" || obj.name == "Ending" || obj == gameObject) {
				continue;
			}
			Destroy(obj);
		}
		// Load Lobby Scene
		SceneManager.LoadScene("Lobby");
	}
	#endregion
    
	#region PrivateMethod
	#endregion
}

}