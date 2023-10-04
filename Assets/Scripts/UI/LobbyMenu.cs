using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TH.Core {

public class LobbyMenu : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	#endregion

	#region PublicMethod
	public void GoGame(int level) {
		// Load Game Scene
		PlayerPrefs.SetInt("Level", level);
		SceneManager.LoadScene("0");
	}

	public void ExitGame() {
		// Quit Game
		Application.Quit();
	}
	#endregion
    
	#region PrivateMethod
	#endregion
}

}