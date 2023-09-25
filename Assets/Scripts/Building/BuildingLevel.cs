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

	private List<LinkedBlock> _linkedBlockList;
	private bool _isLinking = false;
	private EBuildingType _linkingType;
	private ESourceType _linkingSourceType;
	private List<List<BlockAbility>> _linkedBlockAbilities;
	#endregion

	#region PublicMethod
	public void Init(int level) {
		_gridStart.Get(gameObject).Init(level, Vector2Int.zero, ESourceType.Meat, (_, _) => {});

		_grid = new List<List<Block>>();
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			_grid.Add(new List<Block>());
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				_grid[i].Add(null);
			}
		}

		_level = level;
		_linkedBlockList = new List<LinkedBlock>();

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

	public void InstallBlock(Vector2Int gridPos, ESourceType sourceType) {
		if (_hasInit == false) {
			return;
		}

		Vector2Int listIndex = GridManager.Instance.GetListIndex(gridPos);

		if (_grid[listIndex.x][listIndex.y] == null) {
			GameObject blockObject 
				= Instantiate(Resources.Load<GameObject>(Constants.ResourcesPath.Prefabs.SIDE_BLOCK_PREFAB_PATH), _gridStart.Get().transform.parent);
			Block block = blockObject.GetComponent<Block>();
			block.transform.localPosition = GridManager.Instance.GetLocalPosition(gridPos);
			block.Init(_level, gridPos, sourceType, OnBlockClick);
			_grid[listIndex.x][listIndex.y] = block;
		}
		_installedBlockNum++;
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
	
	public void CancelLinking() {
		_isLinking = false;
		_linkingType = EBuildingType.Factory;
		_linkedBlockAbilities = null;
	}

	public void FinishBlockLinking() {
		if (_linkingType == (EBuildingType)(Enum.GetNames(typeof(EBuildingType)).Length - 1)
			&& _linkedBlockAbilities[(int)_linkingType].Count > 0) {
			if (_linkedBlockList == null) {
				_linkedBlockList = new List<LinkedBlock>();
			}
			_linkedBlockList.Add(new LinkedBlock(_linkingSourceType, _linkedBlockAbilities));
		}
	}
	#endregion
    
	#region PrivateMethod
	private void Update() {
		if (_hasInit == false) {
			return;
		}

		DrawLinkLines();
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

	private void CheckBlockLink(BlockAbility blockAbility) {
		if (_isLinking == false) {
			if (blockAbility.m_BuidlingType != EBuildingType.Factory) {
				return;
			}

			_isLinking = true;
			_linkingType = EBuildingType.Factory;
			_linkingSourceType = blockAbility.m_SourceType;
			_linkedBlockAbilities = new List<List<BlockAbility>>();
			for (int i = 0; i < Enum.GetNames(typeof(EBuildingType)).Length; i++) {
				_linkedBlockAbilities.Add(new List<BlockAbility>());
			}

			_linkedBlockAbilities[(int)_linkingType].Add(blockAbility);
		} else {
			if (blockAbility.m_SourceType != _linkingSourceType) {
				return;
			}

			if (blockAbility.m_BuidlingType != _linkingType) {
				if ((int)_linkingType + 1 >= Enum.GetNames(typeof(EBuildingType)).Length) {
					return;
				}

				if ((int)blockAbility.m_BuidlingType == ((int)_linkingType + 1)) {
					if (_linkedBlockAbilities[(int)_linkingType].Count == 0) {
						return;
					}

					_linkingType = blockAbility.m_BuidlingType;
					_linkedBlockAbilities[(int)_linkingType].Add(blockAbility);
				}
			}
		}
	}

	private void OnBlockClick(int level, Vector2Int gridPos) {
		if (level != _level) {
			return;
		}

		if (GridManager.Instance.State == GridManager.BuildingState.BlockLinking) {
			Vector2Int listIndex = GridManager.Instance.GetListIndex(gridPos);
			BlockAbility blockAbility = _grid[listIndex.x][listIndex.y].GetAbility();
			CheckBlockLink(blockAbility);
		}
	}

	private void DrawLinkLines() {
		if (GridManager.Instance.State != GridManager.BuildingState.BlockLinking) {
			return;
		}

		if (_isLinking == false) {
			return;
		}

		int positionCount = 0;
		foreach (var l in _linkedBlockAbilities) {
			positionCount += l.Count;
		}

		_lineRenderer.Get(gameObject).positionCount = positionCount;
		_lineRenderer.Get(gameObject).startColor = Constants.Colors.BlockLinking.LINE_COLOR;
		_lineRenderer.Get(gameObject).endColor = Constants.Colors.BlockLinking.LINE_COLOR;

		int i = 0;
		foreach (var l in _linkedBlockAbilities) {
			foreach (var b in l) {
				_lineRenderer.Get(gameObject).SetPosition(i, b.transform.position);
				i++;
			}
		}
	}
	#endregion
}

}