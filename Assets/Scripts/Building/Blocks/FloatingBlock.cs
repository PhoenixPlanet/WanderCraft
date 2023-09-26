using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TH.Core {

public class FloatingBlock : MonoBehaviour
{
    #region PublicVariables
	public BlockData Data => _blockData;
	#endregion

	#region PrivateVariables
	[SerializeField] private BlockData _blockData;

	private Action<FloatingBlock> _onFloatingBlockClicked;
	private bool _hasInit = false;
	#endregion

	#region PublicMethod
	public void Init(BlockData blockData, Action<FloatingBlock> onFloatingBlockClicked) {
		_blockData = blockData;
		_onFloatingBlockClicked = onFloatingBlockClicked;

		_hasInit = true;
	}
	#endregion
    
	#region PrivateMethod
	private void OnMouseDown() {
		if (_hasInit == false) {
			return;
		}

		_onFloatingBlockClicked?.Invoke(this);
	}
	#endregion
}

}