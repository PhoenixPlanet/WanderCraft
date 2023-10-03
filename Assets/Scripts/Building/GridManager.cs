using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace TH.Core {

public class GridManager : Singleton<GridManager>
{
	[Serializable]
	public class BlockLink {
		[Serializable]
		public class ScoreData {
			public ESourceType sourceType;
			public PropertyData[] levelScores;
			public int thread;
			public PropertyData totalScore;

			public int[] GetLevelScores() {
				int[] scores = new int[levelScores.Length];
				for (int i = 0; i < levelScores.Length; i++) {
					scores[i] = levelScores[i].GetPropertyValue(sourceType);
				}

				return scores;
			}
		}

		public List<HashSet<BlockCluster>> link;
		public ScoreData scoreData = new ScoreData();

		public BlockLink(List<HashSet<BlockCluster>> link) {
			this.link = link;
		}

		public bool Contains(BlockAbility blockAbility) {
			List<BlockCluster> diningClusterList = link[link.Count - 1].ToList(); 
			foreach (var bc in diningClusterList) {
				if (bc.Contains(blockAbility)) {
					return true;
				}
			}

			return false;
		}

		public bool Contains(BlockLink blockLink) {
			List<HashSet<BlockCluster>> targetLink = blockLink.link;
			BlockAbility dining = targetLink[targetLink.Count - 1].ToList()[0].blockAbilities[0];

			return Contains(dining);
		}

		public ScoreData CalculateScore() {
			if (link == null || link.Count == 0) {
				scoreData = null;
				return null;
			}

			ScoreData sd = new ScoreData
			{
				sourceType = link[link.Count - 1].ToList()[0].sourceType,
				levelScores = new PropertyData[link.Count],
				thread = 0,
				totalScore = PropertyData.Zero,
			};

			int[] threadNum = new int[link.Count];

			bool isSink = false;
			for (int i = 0; i < link.Count; i++) {
				PropertyData s = PropertyData.Zero;
				int t = 0;
				foreach (var bc in link[i]) {
					PropertyData ps = bc.GetProductionScore();
					
					if (ps == PropertyData.Zero) {
						isSink = true;
					}

					s += ps;
					t++;
				}
				sd.levelScores[i] = s / link[i].Count;
				threadNum[i] = t;
			}

			sd.thread = threadNum.Min();

			sd.totalScore = PropertyData.Zero;
			if (isSink == false) {
				for (int i = 0; i < link.Count; i++) {
					sd.totalScore += sd.levelScores[i] * sd.thread;
				}
			}

			scoreData = sd;
			return sd;
		}
	}

	public enum BuildingState {
		Normal,
		Building,
	}

    #region PublicVariables
	public List<BlockDataSO> BlockDataList => _blockDataList;

	public Vector2Int GridSize => _gridSize;
	public BuildingState State => _buildingState;
	public PropertyData CurrentProperty => _currentProperty;

	public int SelectedBuildingLevel => _selectedBuildingLevel;
	public int CurrentCenterLevel => _currentCenterLevel;
	public int CurrentOpenedBuildingLevel => _currentOpenedBuildingLevel;

	public PriceDataSO PriceData => _priceDataSO;

	#endregion

	#region PrivateVariables
	private ComponentGetter<GridSelector> _gridSelector
		= new ComponentGetter<GridSelector>(TypeOfGetter.GlobalByName, "GridSelector");

	private ComponentGetter<Building> _building
		= new ComponentGetter<Building>(TypeOfGetter.GlobalByName, "Building");

	private ObjectGetter _waterObj
		= new ObjectGetter(TypeOfGetter.GlobalByName, "WaterGroup");

	private ObjectGetter _endGameUI
		= new ObjectGetter(TypeOfGetter.GlobalByName, "Canvas/MainUI/EndPanel");

	private ComponentGetter<FloatingSpawner> _floatingSpawner
		= new ComponentGetter<FloatingSpawner>(TypeOfGetter.This);

	private ComponentGetter<InfoPanel> _infoPanel
		= new ComponentGetter<InfoPanel>(TypeOfGetter.Global);

	private ComponentGetter<BlockShopList> _blockShopList
		= new ComponentGetter<BlockShopList>(TypeOfGetter.Global);

	[SerializeField] private List<BlockDataSO> _blockDataList;

	[SerializeField] private float _centerBlockHeight;
	[SerializeField] private Vector3 _cellSize;
	[SerializeField] private Vector2Int _gridSize;

	[SerializeField] private PriceDataSO _priceDataSO;

	private Transform _centerBlockParent;

	private List<BuildingLevel> _buildingLevels;
	private List<CenterBlock> _centerBlocks;

	private int _currentCenterLevel;
	private int _currentOpenedBuildingLevel;
	[ShowInInspector] private int _selectedBuildingLevel;

	private bool _hasInit = false;
	private BuildingState _buildingState = BuildingState.Normal;
	private BlockDataSO _selectedBlockData;
	private PropertyData _selectedBlockPrice;
	private Action _onBlockInstalled;

	[ShowInInspector] private List<BlockLink> _blockLinks;
	private PropertyData _currentProperty;
	private float _resourceIncreaseInterval = 1f;
	private float _resourceIncreaseTimer = 0f;
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

		//CheckCanOpenLevel();
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

	public bool InstallNewBlock(int level, Vector2Int gridPos) {
		if (_selectedBlockData == null) {
			Debug.LogError("No selected floating block data");
			return false;
		}

		if (gridPos == Vector2Int.zero) {
			return false;
		}

		if (_currentProperty < _selectedBlockPrice) {
			return false;
		}

		if (level >= _currentOpenedBuildingLevel) {
			OpenNewLevel();
		}

		bool isBlockInstalled = _buildingLevels[level].HasBlockInstalled;
		bool installSuccess = false;
		if (_buildingLevels[level].InstallBlock(gridPos, _selectedBlockData) == true) {
			CheckLink();

			_onBlockInstalled?.Invoke();
			_selectedBlockData = null;

			SelectLevel();

			installSuccess = true;

			_currentProperty -= _selectedBlockPrice;
		}

		HideGridSelector();

		return installSuccess;
	}

	public void DeleteBlock(int level, Vector2Int gridPos) {
		_buildingLevels[level].DeleteBlock(level, gridPos);
		CheckLink();
		SelectLevel();
	}

	public Vector2Int GetListIndex(Vector2Int gridPos) {
		return new Vector2Int(gridPos.x + _gridSize.x / 2, gridPos.y + _gridSize.y / 2);
	}

	public void ChangeState(BuildingState state) {
		_buildingState = state;
		ApplyState();
	}

	public void SelectFloatingBlock(FloatingBlock block) {
		_selectedBlockData = block.Data;
		_selectedBlockPrice = PropertyData.Zero;
		_onBlockInstalled = () => {
			Destroy(block.gameObject);
		};
		SelectLevel();
	}

	public void SelectBlockData(BlockDataSO blockData) {
		_selectedBlockData = blockData;
		_selectedBlockPrice = blockData.blockPrice;
		_onBlockInstalled = () => {
			_selectedBlockData = null;
		};
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

		return _buildingLevels[level].GetBlock(gridPos);
	}

	public void BuyCenterBlock() {
		if ((_currentProperty < _priceDataSO._priceList[_currentCenterLevel]) == true) {
			return;
		}

		_currentProperty -= _priceDataSO._priceList[_currentCenterLevel];
		BuildNewCenterBlock();
	}

	public void ActivateLinkBlocks(List<HashSet<BlockCluster>> blockClusters) {
		DeactivateAllBlocks();

		foreach (var bc in blockClusters) {
			foreach (var blockCluster in bc) {
				foreach (var blockAbility in blockCluster.blockAbilities) {
					blockAbility.block.ActivateBlock();
				}
			}
		}
	}

	public void CalculateAgain() {
		foreach (var bl in _blockLinks) {
			bl.CalculateScore();
		}
		
		_infoPanel.Get().SetInfoPanel(_blockLinks);
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

		_resourceIncreaseTimer += Time.deltaTime;
		if (_resourceIncreaseTimer >= _resourceIncreaseInterval) {
			_resourceIncreaseTimer = 0f;
			_currentProperty += CalculateProduction();
		}

		if ((_currentOpenedBuildingLevel-1) * 0.6 < _waterObj.Get().transform.position.y) {
			_endGameUI.Get().SetActive(true);
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

		_currentProperty = PropertyData.Zero;

		for (int i = 0; i < 3; i++) {
			BuildNewCenterBlock();
		}

		_blockShopList.Get().Init();

		_selectedBlockData = _blockDataList[0];
		_selectedBlockPrice = PropertyData.Zero;
		InstallNewBlock(0, new Vector2Int(1, 0));
		SelectLevel();

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

	private void CheckLink() {
		_blockLinks = new List<BlockLink>();

		for (int i = _currentOpenedBuildingLevel - 1; i >= 0; i--) {
			List<BlockLink> links = _buildingLevels[i].CheckLink();
			if (links != null) {
				foreach (var bl in links) {
					bool isUnique = true;
					foreach(var worldBl in _blockLinks) {
						if (worldBl.Contains(bl)) {
							isUnique = false;
							break;
						}
					}
					if (isUnique) {
						_blockLinks.Add(bl);
					}
				}
			}
		}

		if (_blockLinks == null) {
			return;
		}

		foreach (var bl in _blockLinks) {
			bl.CalculateScore();
		}
		
		_infoPanel.Get().SetInfoPanel(_blockLinks);
	}

	private PropertyData CalculateProduction() {
		PropertyData production = PropertyData.Zero;

		if (_blockLinks == null) {
			return production;
		}

		foreach (var bl in _blockLinks) {
			BlockLink.ScoreData sd = bl.CalculateScore();
			production += sd == null ? PropertyData.Zero : sd.totalScore;
		}

		//Debug.Log("Production: " + production);
		return production;
	}

	private void CancelFloatingBlockSelect() {
		if (_selectedBlockData != null) {
			_selectedBlockData = null;
			_onBlockInstalled = null;
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

		//SelectLevel();
	}

	private bool IsBlockInstalable(int level, Vector2Int gridPos) {
		if (level >= _currentOpenedBuildingLevel) {
			if (level > _currentCenterLevel) {
				return false;
			}

			if (gridPos.magnitude == 1) {
				return true;
			}
			return false;
		}
		return _buildingLevels[level].IsBlockInstallable(gridPos);
	}

	private float LevelYPos(int level) {
		return _building.Get(gameObject).ZeroLevelY + level * _centerBlockHeight;
	}

	private void SelectLevel() {
		if (_buildingState == BuildingState.Normal) {
			return;
		}
		
		// for (int i = 0; i < _buildingLevels.Count; i++) {
		// 	_buildingLevels[i].DeactivateBlocks();
		// 	if (i == _selectedBuildingLevel) {
		// 		if (_buildingState == BuildingState.Building) {
		// 			if (_selectedBlockData != null) {
		// 				_buildingLevels[i].ActivateLevelBuildingUI();
		// 			} else {
		// 				_buildingLevels[i].ActivateBlocks();
		// 			}
		// 		}
		// 	} else {
		// 		if (_buildingState == BuildingState.Building) {
		// 			_buildingLevels[i].DeactivateLevelBuildingUI();
		// 		}
		// 	}
		// }
		for (int i = 0; i < _buildingLevels.Count; i++) {
			if (_buildingState == BuildingState.Building) {
				_buildingLevels[i].DeactivateBlocks();
				if (_selectedBlockData != null) {
					_buildingLevels[i].ActivateLevelBuildingUI();
					Debug.Log("ActivateLevelBuildingUI");
				} else {
					_buildingLevels[i].DeactivateLevelBuildingUI();
					_buildingLevels[i].ActivateBlocks();
					Debug.Log("DeactivateLevelBuildingUI");
				}
			} else {
				_buildingLevels[i].DeactivateBlocks();
				Debug.Log("DeactivateBlocks");
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

	private void DeactivateAllBlocks() {
		for (int i = 0; i < _buildingLevels.Count; i++) {
			_buildingLevels[i].DeactivateBlocks();
		}
	}
	#endregion
}

}