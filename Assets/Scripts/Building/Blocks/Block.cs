using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TH.Core {

public class Block : MonoBehaviour
{
    #region PublicVariables
	public BlockDataSO Data => _blockData;
	public BlockAbility Ability => _blockAbility.Get(gameObject);
	public BlockCluster Cluster => _cluster;
	#endregion

	#region PrivateVariables
	private Material _normal;
	private static Material _transparent;

	[SerializeField] private Vector2Int _gridPos;
	[SerializeField] private int _height;

	private ComponentGetter<MeshRenderer> _meshRenderer
		= new ComponentGetter<MeshRenderer>(TypeOfGetter.ChildByName, "Renderer");
	private ComponentGetter<Collider> _collider
		= new ComponentGetter<Collider>(TypeOfGetter.This);
	private ComponentGetter<BlockAbility> _blockAbility
		= new ComponentGetter<BlockAbility>(TypeOfGetter.This);
	private ObjectGetter _pillar
		= new ObjectGetter(TypeOfGetter.ChildByName, "Pillar");
		private ComponentGetter<blockAnimation> _animation
			= new ComponentGetter<blockAnimation>(TypeOfGetter.This);
	private ObjectGetter _iconObj
		= new ObjectGetter(TypeOfGetter.ChildByName, "Icon");

	protected List<BlockMouseSensor> _blockMouseSensors;

	private int _level;
	private BlockCluster _cluster;
	private BlockDataSO _blockData;
	private Action<int, Vector2Int> _onClick;
	#endregion

	#region PublicMethod
	public void Init(int level, Vector2Int gridPos, BlockDataSO blockData, Action<int, Vector2Int> onClick) {
		if (_transparent == null) {
			LoadMaterials();
		}

		_normal = Resources.Load<Material>(Constants.ResourcesPath.Materials.BLOCK_NORMAL_MATERIAL_PATH);
		
		_blockMouseSensors = new List<BlockMouseSensor>();

		for (int i = 0; i < 5; i++) {
			GameObject blockMouseSensorObj = 
				Instantiate(Resources.Load<GameObject>(Constants.ResourcesPath.Prefabs.BLOCK_MOUSE_SENSOR_PREFAB_PATH), transform);
			BlockMouseSensor blockMouseSensor = blockMouseSensorObj.GetComponent<BlockMouseSensor>();
			blockMouseSensor.Init((BlockMouseSensor.Direction)i, SensorMouseOver, SensorMouseExit, SensorMouseClick);

			_blockMouseSensors.Add(blockMouseSensor);
		}

		_level = level;
		_gridPos = gridPos;
		_blockData = blockData;
		_onClick = onClick;
		
		if (_blockData != null) {
			_blockAbility.Get(gameObject).Init(_blockData);
		}

		InitPosition();
	}

	public virtual void ActivateBlock() {
		//_meshRenderer.Get(gameObject).material = _normal;
		MeshRenderer[] meshRenderers = transform.Find("Highlight").GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in meshRenderers) {
			meshRenderer.material = _normal;
			Color color = meshRenderer.material.GetColor("_BaseColor");
			color.a = 0.5f;
			meshRenderer.material.SetColor("_BaseColor", color);
		}

		_collider.Get(gameObject).enabled = true;
	}

	public virtual void DeactivateBlock() {
		//_meshRenderer.Get(gameObject).material = _transparent;
		MeshRenderer[] meshRenderers = transform.Find("Highlight").GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in meshRenderers) {
			meshRenderer.material = _normal;
			Color color = meshRenderer.material.GetColor("_BaseColor");
			color.a = 0.2f;
			meshRenderer.material.SetColor("_BaseColor", color);
		}

		_collider.Get(gameObject).enabled = false;

		for (int i = 0; i < 5; i++) {
			_blockMouseSensors[i].gameObject.SetActive(false);
		}
	}

	public virtual void ActivateBlockBuildingUI() {
		ActivateBlock();
		_collider.Get(gameObject).enabled = false;

		for (int i = 0; i < 5; i++) {
			_blockMouseSensors[i].gameObject.SetActive(true);
		}
	}

	public virtual void DeactivateBlockBuildingUI() {
		DeactivateBlock();
	}

	public BlockAbility GetAbility() {
		return _blockAbility.Get(gameObject);
	}

	public void SetNormalMaterial(Material material) {
		_normal = material;
	}

	public void SetCluster(BlockCluster cluster) {
		_cluster = cluster;
	}

	public BlockCluster CheckLinkBelow(EBuildingType targetBuildingType, ESourceType sourceType) {
		if (_level == 0) {
			return null;
		}

		Block belowBlock = GridManager.Instance.GetBlock(_level - 1, _gridPos);
		if (belowBlock != null) {
			if (belowBlock.Data.BuildingType == targetBuildingType && belowBlock.Data.SourceType == sourceType) {
				return belowBlock.Cluster;
			}
		}

		return null;
	}

	public BlockCluster CheckLinkAbove(EBuildingType targetBuildingType, ESourceType sourceType) {
		if (_level == GridManager.Instance.CurrentOpenedBuildingLevel - 1) {
			return null;
		}

		Block aboveBlock = GridManager.Instance.GetBlock(_level + 1, _gridPos);
		if (aboveBlock != null) {
			if (aboveBlock.Data.BuildingType == targetBuildingType && aboveBlock.Data.SourceType == sourceType) {
				//_pillar.Get(gameObject).SetActive(true);

				return aboveBlock.Cluster;
			}
		}

		return null;
	}

	public void TurnOffPillar() {
			//_pillar.Get(gameObject).SetActive(false);
			if (_animation.Get(gameObject) != null)
			{
				_animation.Get(gameObject).GetComponent<blockAnimation>();
			}
	}
	#endregion
    
	#region PrivateMethod
	private void LoadMaterials() {
		_transparent = Resources.Load<Material>(Constants.ResourcesPath.Materials.BLOCK_TRANSPARENT_MATERIAL_PATH);
	}

	private void InitPosition() {
		transform.localPosition = GridManager.Instance.GetLocalPosition(_gridPos);
	}

	private void SensorMouseOver(Vector2Int direction) {
		GridManager.Instance.ShowGridSelector(direction == Vector2Int.zero ? _level + 1 : _level, _gridPos + direction);
	}

	private void SensorMouseExit(Vector2Int direction) {
		GridManager.Instance.HideGridSelector();	
	}

	private void SensorMouseClick(Vector2Int direction) {
		GridManager.Instance.InstallNewBlock(direction == Vector2Int.zero ? _level + 1 : _level, _gridPos + direction);
	}

	private void OnMouseOver() {
		if (GridManager.Instance.State == GridManager.BuildingState.Building) {
			if (Input.GetMouseButtonDown(0)) {
				_onClick?.Invoke(_level, _gridPos);
			}

			if (Input.GetMouseButtonDown(1)) {
				GridManager.Instance.DeleteBlock(_level, _gridPos);
			}
		}
	}
	#endregion
}

public class BlockWrapperForDFS {
	public Block Block;
	public bool IsVisited;

	public BlockWrapperForDFS(Block block) {
		Block = block;
		IsVisited = false;
	}
}

}