using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class FloatingSpawner : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField] private List<BlockData> _blockDataList;
	[SerializeField] private float _spawnInterval;

	private Dictionary<ESourceType, List<BlockData>> _blockDataDict;
	private Transform _floatingBlockParentTransform;
	private bool _hasInit = false;
	#endregion

	#region PublicMethod
	public void Init() {
		if (_hasInit == true) {
			return;
		}

		_blockDataDict = new Dictionary<ESourceType, List<BlockData>>();
		foreach (BlockData blockData in _blockDataList) {
			if (_blockDataDict.ContainsKey(blockData.SourceType) == false) {
				_blockDataDict.Add(blockData.SourceType, new List<BlockData>());
			}
			_blockDataDict[blockData.SourceType].Add(blockData);
		}

		GameObject floatingBlockParentObj = new GameObject(Constants.Helper.Organizer.FLOATING_BLOCK_PARENT_NAME);
		_floatingBlockParentTransform = floatingBlockParentObj.transform;

		StartCoroutine(SpawnFloatingBlock());
		_hasInit = true;
	}
	#endregion
    
	#region PrivateMethod
	private void Update() {
		if (_hasInit == false) {
			return;
		}
	}

	private IEnumerator SpawnFloatingBlock() {
		int i = 0;
		while (true) {
			yield return new WaitForSeconds(_spawnInterval);

			BlockData blockData = _blockDataDict[ESourceType.Meat][i];
			GameObject fb = Instantiate(blockData.FloatingPrefab, Random.insideUnitSphere * 5, Quaternion.identity, _floatingBlockParentTransform);
			fb.GetComponent<FloatingBlock>().Init(blockData, OnFloatingBlockClicked);
			i++;
			if (i >= _blockDataDict[ESourceType.Meat].Count) {
				i = 0;
			}
		}
	}

	private void OnFloatingBlockClicked(FloatingBlock floatingBlock) {
		if (GridManager.Instance.State == GridManager.BuildingState.Building) {
			GridManager.Instance.SelectFloatingBlock(floatingBlock);
		}
	}
	#endregion
}

}