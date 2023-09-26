using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace TH.Core {

public class GridManager : Singleton<GridManager>
{
	public enum BuildingState {
		Normal,
		Building,
	}

    #region PublicVariables
	public Vector2Int GridSize => _gridSize;
	public BuildingState State => _buildingState;

		public int CurrentCenterLevel => _currentCenterLevel;

	#endregion

	#region PrivateVariables
	private ComponentGetter<GridSelector> _gridSelector
		= new ComponentGetter<GridSelector>(TypeOfGetter.GlobalByName, "GridSelector");

	private ComponentGetter<Building> _building
		= new ComponentGetter<Building>(TypeOfGetter.GlobalByName, "Building");

	private ComponentGetter<FloatingSpawner> _floatingSpawner
		= new ComponentGetter<FloatingSpawner>(TypeOfGetter.This);

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
	private FloatingBlock _selectedFloatingBlock;
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
		if (_selectedFloatingBlock == null) {
			Debug.LogError("No selected floating block data");
			return;
		} 

		bool isBlockInstalled = _buildingLevels[level].HasBlockInstalled;
		if (_buildingLevels[level].InstallBlock(gridPos, _selectedFloatingBlock.Data) == true) {
			Destroy(_selectedFloatingBlock.gameObject);
			_selectedFloatingBlock = null;
			SelectLevel();
		}

		if (isBlockInstalled == false && _buildingLevels[level].HasBlockInstalled) {
			CheckCanOpenLevel();
		}
	}

	public Vector2Int GetListIndex(Vector2Int gridPos) {
		return new Vector2Int(gridPos.x + _gridSize.x / 2, gridPos.y + _gridSize.y / 2);
	}

	public void ChangeState(BuildingState state) {
		_buildingState = state;
		ApplyState();
	}

	public void SelectFloatingBlock(FloatingBlock block) {
		_selectedFloatingBlock = block;
		SelectLevel();
	}

	public bool IsGridPosInGrid(Vector2Int gridPos) {
		Vector2Int listPos = GetListIndex(gridPos);
		
		return IsListPosInGrid(listPos);
	}

	public bool IsListPosInGrid(Vector2Int listPos) {
		if (listPos.x < 0 || listPos.x >= _gridSize.x) {
			return false;
		}

		if (listPos.y < 0 || listPos.y >= _gridSize.y) {
			return false;
		}

		return true;
	}

	public Block GetBlock(int level, Vector2Int gridPos) {
		if (_hasInit == false) {
			return null;
		}

		Vector2Int listIndex = GetListIndex(gridPos);
		if (IsListPosInGrid(listIndex) == false) {
			return null;
		}

		return _buildingLevels[level].GetBlock(listIndex);
	}
	#endregion
    
	#region PrivateMethod
	private void Update() {
		if (_hasInit == false) {
			return;
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			SelectNextLevel();
		} else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			SelectPrevLevel();
		}

		if (Input.GetMouseButtonDown(2)) {
			CancelFloatingBlockSelect();
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

		_buildingState = BuildingState.Normal;
		ApplyState();

		_floatingSpawner.Get(gameObject).Init();

		_hasInit = true;
	}

	private void ApplyState() {
		switch (_buildingState) {
			case BuildingState.Normal:
				CancelFloatingBlockSelect();
				DeactivateBuildingUI();
				break;
			case BuildingState.Building:
				SelectLevel();
				break;
			default:
				break;
		}
	}

	private void CancelFloatingBlockSelect() {
		if (_selectedFloatingBlock != null) {
			_selectedFloatingBlock = null;
		}

		if (_buildingState == BuildingState.Building) {
			SelectLevel();
		}
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
		if (_buildingState == BuildingState.Normal) {
			return;
		}
		
		for (int i = 0; i < _buildingLevels.Count; i++) {
			_buildingLevels[i].DeactivateBlocks();
			if (i == _selectedBuildingLevel) {
				if (_buildingState == BuildingState.Building) {
					if (_selectedFloatingBlock != null) {
						_buildingLevels[i].ActivateLevelBuildingUI();
					} else {
						_buildingLevels[i].ActivateBlocks();
					}
				}
			} else {
				if (_buildingState == BuildingState.Building) {
					_buildingLevels[i].DeactivateLevelBuildingUI();
				}
			}
		}
	}

	private void SelectNextLevel() {
		if (_buildingState == BuildingState.Normal) {
			return;
		}

		if (_selectedBuildingLevel < _currentOpenedBuildingLevel - 1) {
			_selectedBuildingLevel++;
			SelectLevel();
		}
	}

	private void SelectPrevLevel() {
		if (_buildingState == BuildingState.Normal) {
			return;
		}

		if (_selectedBuildingLevel > 0) {
			_selectedBuildingLevel--;
			SelectLevel();
		}
	}

	private void DeactivateBuildingUI() {
		for (int i = 0; i < _buildingLevels.Count; i++) {
			_buildingLevels[i].DeactivateLevelBuildingUI();
			_buildingLevels[i].ActivateBlocks();
		}
	}
	#endregion
}

}