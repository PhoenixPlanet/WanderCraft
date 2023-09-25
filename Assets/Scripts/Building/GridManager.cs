using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TH.Core {

public class GridManager : Singleton<GridManager>
{
	public enum BuildingState {
		Normal,
		Building,
		BlockLinking,
	}

    #region PublicVariables
	public Vector2Int GridSize => _gridSize;
	#endregion

	#region PrivateVariables
	private ComponentGetter<GridSelector> _gridSelector
		= new ComponentGetter<GridSelector>(TypeOfGetter.GlobalByName, "GridSelector");

	private ComponentGetter<Building> _building
		= new ComponentGetter<Building>(TypeOfGetter.GlobalByName, "Building");

	[SerializeField] private float _centerBlockHeight;
	[SerializeField] private Vector3 _cellSize;
	[SerializeField] private Vector2Int _gridSize;

	private Transform _centerBlockParent;

	private List<BuildingLevel> _buildingLevels;
	private List<CenterBlock> _centerBlocks;

	private int _currentCenterLevel;
	private int _currentOpenedBuildingLevel;
	[ShowInInspector] private int _selectedBuildingLevel;

	private bool _hasInit = false;
	private BuildingState _buildingState = BuildingState.Normal;
	#endregion

	#region PublicMethod
	public Vector3 GetLocalPosition(Vector2Int gridPos) {
		return new Vector3(gridPos.x * _cellSize.x, 0, gridPos.y * _cellSize.y);
	}

	[Button]
	public void BuildNewCenterBlock() {
		_currentCenterLevel++;

		GameObject blockObject 
			= Instantiate(Resources.Load<GameObject>(Constants.ResourcesPath.Prefabs.CENTER_BLOCK_PREFAB_PATH), _centerBlockParent);
		CenterBlock centerBlock = blockObject.GetComponent<CenterBlock>();
		Vector3 targetPos = GetLocalPosition(new Vector2Int(0, 0));
		targetPos.y = (_currentCenterLevel - 1) * _centerBlockHeight;
		centerBlock.Init(_currentCenterLevel, targetPos);
		_centerBlocks.Add(centerBlock);

		CheckCanOpenLevel();
	}

	public void ShowGridSelector(int level, Vector2Int _gridPos) {
		_gridSelector.Get().gameObject.SetActive(true);
	
		if (IsBlockInstalable(level, _gridPos)) {
			_gridSelector.Get().SetColor(true);
		} else {
			_gridSelector.Get().SetColor(false);
		}

		Vector3 targetPos = GetLocalPosition(_gridPos);
		targetPos.y = LevelYPos(level);
		_gridSelector.Get().transform.localPosition = targetPos;
	}

	public void HideGridSelector() {
		_gridSelector.Get().gameObject.SetActive(false);
	}

	public void InstallNewBlock(int level, Vector2Int gridPos) {
		bool isBlockInstalled = _buildingLevels[level].HasBlockInstalled;
		_buildingLevels[level].InstallBlock(gridPos);
		if (isBlockInstalled == false && _buildingLevels[level].HasBlockInstalled) {
			CheckCanOpenLevel();
		}
	}

	public Vector2Int GetListIndex(Vector2Int gridPos) {
		return new Vector2Int(gridPos.x + _gridSize.x / 2, gridPos.y + _gridSize.y / 2);
	}
	#endregion
    
	#region PrivateMethod
	private void Update() {
		if (_hasInit == false) {
			return;
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			if (_selectedBuildingLevel < _currentOpenedBuildingLevel - 1) {
				_selectedBuildingLevel++;
				SelectLevel();
			}
		} else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			if (_selectedBuildingLevel > 0) {
				_selectedBuildingLevel--;
				SelectLevel();
			}
		}
	}

	protected override void Init() {
		HideGridSelector();

		GameObject centerBlockParentObj = new GameObject(Constants.Helper.Organizer.CENTER_BLOCK_PARENT_NAME);
		_centerBlockParent = centerBlockParentObj.transform;

		_centerBlocks = new List<CenterBlock>();
		_centerBlockHeight = CenterBlock.HEIGHT;
		_currentCenterLevel = 0;
		_currentOpenedBuildingLevel = 0;

		_buildingLevels = new List<BuildingLevel>();
		_selectedBuildingLevel = 0;

		BuildNewCenterBlock();

		_hasInit = true;
	}

	private void CheckCanOpenLevel() {
		if (_currentOpenedBuildingLevel < _currentCenterLevel) {
			if (_currentOpenedBuildingLevel == 0 || _buildingLevels[_currentOpenedBuildingLevel - 1].HasBlockInstalled == true) {
				OpenNewLevel();
			}
		}
	}

	private void OpenNewLevel() {
		BuildingLevel buildingLevel = 
			Instantiate(Resources.Load<GameObject>(Constants.ResourcesPath.Prefabs.BUILDING_LEVEL_PREFAB_PATH), _building.Get().transform).GetComponent<BuildingLevel>();
		buildingLevel.Init(_currentOpenedBuildingLevel);
		buildingLevel.transform.localPosition = new Vector3(0, LevelYPos(_currentOpenedBuildingLevel), 0);
		_buildingLevels.Add(buildingLevel);

		_currentOpenedBuildingLevel++;

		SelectLevel();
	}

	private bool IsBlockInstalable(int level, Vector2Int gridPos) {
		return _buildingLevels[level].IsBlockInstallable(gridPos);
	}

	private float LevelYPos(int level) {
		return _building.Get(gameObject).ZeroLevelY + level * _centerBlockHeight;
	}

	private void SelectLevel() {
		for (int i = 0; i < _buildingLevels.Count; i++) {
			if (i == _selectedBuildingLevel) {
				_buildingLevels[i].ActivateLevel();
			} else {
				_buildingLevels[i].DeactivateLevel();
			}
		}
	}
	#endregion
}

}