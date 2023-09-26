using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TH.Core {

public class InfoPanel : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField] private GameObject _infoSelectButtonPrefab;
	[SerializeField] private RectTransform _contentRectTransform;

	private List<InfoSelectButton> _infoSelectButtons = new List<InfoSelectButton>();
	#endregion

	#region PublicMethod
	public void SetInfoPanel(List<GridManager.BlockLink> blockLinks) {
		foreach (var i in _infoSelectButtons) {
			Destroy(i.gameObject);
		}
		_infoSelectButtons.Clear();

		foreach (var i in blockLinks) {
			var infoSelectButton = Instantiate(_infoSelectButtonPrefab, _contentRectTransform).GetComponent<InfoSelectButton>();
			infoSelectButton.Init(
				Constants.GameSetting.SourceName.name[i.scoreData.sourceType], 
				i.scoreData.thread, 
				(int) i.scoreData.levelScores.Average(), 
				i.scoreData.totalScore,
				i);
			_infoSelectButtons.Add(infoSelectButton);
		}
	}
	#endregion
    
	#region PrivateMethod
	#endregion
}

}