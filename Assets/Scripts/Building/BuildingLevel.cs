using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class BuildingLevel : MonoBehaviour
{
    #region PublicVariables
	public bool HasBlockInstalled => _installedBlockNum > 0;
	#endregion

	#region PrivateVariables
	private ComponentGetter<Block> _gridStart
		= new ComponentGetter<Block>(TypeOfGetter.ChildByName, "GridStart");
	private ComponentGetter<LineRenderer> _lineRenderer
		= new ComponentGetter<LineRenderer>(TypeOfGetter.This);
	
	private List<List<Block>> _grid;
	private int _level;

	private bool _hasInit = false;
	private int _installedBlockNum = 0;

	private List<BlockCluster> _BlockClusterList;
	#endregion

	#region PublicMethod
	public void Init(int level) {
		_gridStart.Get(gameObject).Init(level, Vector2Int.zero, null, (_, _) => {});

		_grid = new List<List<Block>>();
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			_grid.Add(new List<Block>());
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				_grid[i].Add(null);
			}
		}

		_level = level;

		_hasInit = true;
	}

	public bool IsBlockInstallable(Vector2Int gridPos) {
		if (_hasInit == false) {
			return false;
		}

		Vector2Int listIndex = GridManager.Instance.GetListIndex(gridPos);
		if (_grid[listIndex.x][listIndex.y] == null) {
			return true;
		} else {
			return false;
		}
	}

	public bool InstallBlock(Vector2Int gridPos, BlockData blockData) {
		if (_hasInit == false) {
			return false;
		}

		Vector2Int listIndex = GridManager.Instance.GetListIndex(gridPos);

		if (_grid[listIndex.x][listIndex.y] == null) {
			GameObject blockObject 
				= Instantiate(blockData.RealPrefab, _gridStart.Get().transform.parent);
			Block block = blockObject.GetComponent<Block>();
			block.transform.localPosition = GridManager.Instance.GetLocalPosition(gridPos);
			block.Init(_level, gridPos, blockData, OnBlockClick);
			_grid[listIndex.x][listIndex.y] = block;

			_installedBlockNum++;

			return true;
		}

		return false;
	}

	public void ActivateLevelBuildingUI() {
		ActivateBlocksBuildingUI();

		_gridStart.Get(gameObject).ActivateBlockBuildingUI();
	}

	public void DeactivateLevelBuildingUI() {
		DeactivateBlocksBuildingUI();

		_gridStart.Get(gameObject).DeactivateBlockBuildingUI();
	}

	public void ActivateBlocks() {
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				if (_grid[i][j] != null) {
					_grid[i][j].ActivateBlock();
				}
			}
		}

		_gridStart.Get(gameObject).ActivateBlock();
	}

	public void DeactivateBlocks() {
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				if (_grid[i][j] != null) {
					_grid[i][j].DeactivateBlock();
				}
			}
		}

		_gridStart.Get(gameObject).DeactivateBlock();
	}
	#endregion
    
	#region PrivateMethod
	private void Update() {
		if (_hasInit == false) {
			return;
		}
	}

	private void ActivateBlocksBuildingUI() {
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				if (_grid[i][j] != null) {
					_grid[i][j].ActivateBlockBuildingUI();
				}
			}
		}
	}

	private void DeactivateBlocksBuildingUI() {
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				if (_grid[i][j] != null) {
					_grid[i][j].DeactivateBlockBuildingUI();
				}
			}
		}
	}

	private void OnBlockClick(int level, Vector2Int gridPos) {
		if (level != _level) {
			return;
		}
	}
	#endregion
}

}