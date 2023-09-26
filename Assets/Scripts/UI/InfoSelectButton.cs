using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TH.Core {

public class InfoSelectButton : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	private ComponentGetter<Button> _infoSelectButton =
		new ComponentGetter<Button>(TypeOfGetter.This);
	private ComponentGetter<TextMeshProUGUI> _titleText =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Title");
	private ComponentGetter<TextMeshProUGUI> _threadText =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Description/ThreadNum");
	private ComponentGetter<TextMeshProUGUI> _averageText =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Description/Average");
	private ComponentGetter<TextMeshProUGUI> _TotalText =
		new ComponentGetter<TextMeshProUGUI>(TypeOfGetter.ChildByName, "Description/Total");

	private GridManager.BlockLink _blockLink;
	#endregion

	#region PublicMethod
	public void Init(string title, int threadNum, float average, float total, GridManager.BlockLink blockLink) {
		_titleText.Get(gameObject).text = title;
		_threadText.Get(gameObject).text = "활성 쓰레드: " + threadNum.ToString();
		_averageText.Get(gameObject).text = "평균 생산량: " + average.ToString();
		_TotalText.Get(gameObject).text = "전체 생산량: " + total.ToString() + (total == 0 ? " (잠김)" : "");

		_blockLink = blockLink;

		_infoSelectButton.Get(gameObject).onClick.AddListener(() => {
			GridManager.Instance.ActivateLinkBlocks(_blockLink.link);
		});
	}
	#endregion
    
	#region PrivateMethod
	#endregion
}

}