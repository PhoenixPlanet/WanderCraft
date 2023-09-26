using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TH.Core.Constants.Colors;
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
	
	private List<List<BlockWrapperForDFS>> _grid;
	private int _level;

	private bool _hasInit = false;
	private int _installedBlockNum = 0;

	[ShowInInspector] private List<BlockCluster> _blockClusterList;
	#endregion

	#region PublicMethod
	public void Init(int level) {
		_gridStart.Get(gameObject).Init(level, Vector2Int.zero, null, (_, _) => {});

		_grid = new List<List<BlockWrapperForDFS>>();
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			_grid.Add(new List<BlockWrapperForDFS>());
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				_grid[i].Add(new BlockWrapperForDFS(null));
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
		if (_grid[listIndex.x][listIndex.y].Block == null) {
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

		if (_grid[listIndex.x][listIndex.y].Block == null) {
			GameObject blockObject 
				= Instantiate(blockData.RealPrefab, _gridStart.Get().transform.parent);
			Block block = blockObject.GetComponent<Block>();
			block.transform.localPosition = GridManager.Instance.GetLocalPosition(gridPos);
			block.Init(_level, gridPos, blockData, OnBlockClick);
			_grid[listIndex.x][listIndex.y].Block = block;

			_installedBlockNum++;
			ScanLevel();


			return true;
		}

		return false;
	}

	public void DeleteBlock(int level, Vector2Int gridPos) {
		if (_hasInit == false) {
			return;
		}

		Vector2Int listIndex = GridManager.Instance.GetListIndex(gridPos);
		if (GridManager.Instance.IsListPosInGrid(listIndex) == false) {
			return;
		}

		if (_grid[listIndex.x][listIndex.y].Block == null) {
			return;
		}

		Destroy(_grid[listIndex.x][listIndex.y].Block.gameObject);
		_grid[listIndex.x][listIndex.y].Block = null;
		ScanLevel();

		_installedBlockNum--;
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
				if (_grid[i][j].Block != null) {
					_grid[i][j].Block.ActivateBlock();
				}
			}
		}

		_gridStart.Get(gameObject).ActivateBlock();
	}

	public void DeactivateBlocks() {
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				if (_grid[i][j].Block != null) {
					_grid[i][j].Block.DeactivateBlock();
				}
			}
		}

		_gridStart.Get(gameObject).DeactivateBlock();
	}

	public Block GetBlock(Vector2Int gridPos) {
		if (_hasInit == false) {
			return null;
		}

		Vector2Int listIndex = GridManager.Instance.GetListIndex(gridPos);
		if (GridManager.Instance.IsListPosInGrid(listIndex) == false) {
			return null;
		}

		return _grid[listIndex.x][listIndex.y].Block;
	}

	public void ScanLevel() {
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				if (_grid[i][j].Block != null) {
					_grid[i][j].Block.TurnOffPillar();
				}
			}
		}

		_blockClusterList = FindAllClusters();
		if (_blockClusterList == null) {
			return;
		}

		foreach (BlockCluster blockCluster in _blockClusterList) {
			blockCluster.ApplyCluster();
		}
	}

	public List<GridManager.BlockLink> CheckLink() {
		if (_blockClusterList == null) {
			return null;
		}

		List<GridManager.BlockLink> blockLinks = new List<GridManager.BlockLink>();
		foreach (BlockCluster blockCluster in _blockClusterList) {
			List<HashSet<BlockCluster>> t = blockCluster.CheckLink();
			if (t == null) {
				continue;
			}
			GridManager.BlockLink link = new GridManager.BlockLink(t);
			bool isUnique = true;
			foreach (var bl in blockLinks) {
				if (bl.Contains(link)) {
					isUnique = false;
					break;
				}
			}
			if (isUnique) {
				blockLinks.Add(link);
			}
		}

		if (blockLinks.Count > 0) {
			return blockLinks;
		} else {
			return null;
		}
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
				if (_grid[i][j].Block != null) {
					_grid[i][j].Block.ActivateBlockBuildingUI();
				}
			}
		}
	}

	private void DeactivateBlocksBuildingUI() {
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				if (_grid[i][j].Block != null) {
					_grid[i][j].Block.DeactivateBlockBuildingUI();
				}
			}
		}
	}

	private void OnBlockClick(int level, Vector2Int gridPos) {
		if (level != _level) {
			return;
		}
	}

	/*
	DFS algorithm for finding block cluster
	*/
	private void InitDFS() {
		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				_grid[i][j].IsVisited = false;
			}
		}
	}

	public List<BlockCluster> FindClusters(EBuildingType buildingType, ESourceType sourceType) {
		InitDFS();

		_blockClusterList = new List<BlockCluster>();

		for (int i = 0; i < GridManager.Instance.GridSize.x; i++) {
			for (int j = 0; j < GridManager.Instance.GridSize.y; j++) {
				if (_grid[i][j].Block != null 
					&& _grid[i][j].IsVisited == false 
					&& _grid[i][j].Block.Data.BuildingType == buildingType 
					&& _grid[i][j].Block.Data.SourceType == sourceType) {
					BlockCluster blockCluster = new BlockCluster(_level, buildingType, sourceType);
					DFS(new Vector2Int(i, j), blockCluster);
					if (blockCluster.BlockNum() > 0) {
						_blockClusterList.Add(blockCluster);
					}
				}
			}
		}

		return _blockClusterList;
	}

	private void DFS(Vector2Int listPos, BlockCluster blockCluster) {
		if (GridManager.Instance.IsListPosInGrid(listPos) == false) {
			return;
		}

		if (_grid[listPos.x][listPos.y].Block == null) {
			return;
		}

		if (_grid[listPos.x][listPos.y].IsVisited == true) {
			return;
		}

		if (_grid[listPos.x][listPos.y].Block.Data.BuildingType != blockCluster.buildingType 
			|| _grid[listPos.x][listPos.y].Block.Data.SourceType != blockCluster.sourceType) {
			return;
		}

		_grid[listPos.x][listPos.y].IsVisited = true;
		blockCluster.AddBlock(_grid[listPos.x][listPos.y].Block.Ability);

		Vector2Int[] directions = new Vector2Int[] {
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1)
		};

		for (int i = 0; i < directions.Length; i++) {
			Vector2Int nextListPos = listPos + directions[i];
			if (GridManager.Instance.IsListPosInGrid(nextListPos) == true) {
				DFS(nextListPos, blockCluster);
			}
		}
	}

	private List<BlockCluster> FindAllClusters() {
		List<BlockCluster> blockClusterList = new List<BlockCluster>();

		for (int i = 0; i < Enum.GetNames(typeof(EBuildingType)).Length; i++) {
			for (int j = 0; j < Enum.GetNames(typeof(ESourceType)).Length; j++) {
				List<BlockCluster> clusters = FindClusters((EBuildingType)i, (ESourceType)j);
				if (clusters.Count > 0) {
					blockClusterList.AddRange(clusters);
				}
			}
		}

		if (blockClusterList.Count > 0) {
			return blockClusterList;
		} else {
			return null;
		}
	}
	#endregion
}

}