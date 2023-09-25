using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class Block : MonoBehaviour
{
    #region PublicVariables
	#endregion

	#region PrivateVariables
	private static Material _normal;
	private static Material _transparent;

	[SerializeField] private Vector2Int _gridPos;
	[SerializeField] private int _height;

	private ComponentGetter<MeshRenderer> _meshRenderer
		= new ComponentGetter<MeshRenderer>(TypeOfGetter.ChildByName, "Renderer");
	private ComponentGetter<Collider> _collider
		= new ComponentGetter<Collider>(TypeOfGetter.This);
	private ComponentGetter<BlockAbility> _blockAbility
		= new ComponentGetter<BlockAbility>(TypeOfGetter.This);

	protected List<BlockMouseSensor> _blockMouseSensors;

	private int _level;
	private ESourceType _sourceType;
	private Action<int, Vector2Int> _onClick;
	#endregion

	#region PublicMethod
	public void Init(int level, Vector2Int gridPos, ESourceType sourceType, Action<int, Vector2Int> onClick) {
		if (_normal == null || _transparent == null) {
			LoadMaterials();
		}
		
		_blockMouseSensors = new List<BlockMouseSensor>();

		for (int i = 0; i < 4; i++) {
			GameObject blockMouseSensorObj = 
				Instantiate(Resources.Load<GameObject>(Constants.ResourcesPath.Prefabs.BLOCK_MOUSE_SENSOR_PREFAB_PATH), transform);
			BlockMouseSensor blockMouseSensor = blockMouseSensorObj.GetComponent<BlockMouseSensor>();
			blockMouseSensor.Init((BlockMouseSensor.Direction)i, SensorMouseOver, SensorMouseExit, SensorMouseClick);

			_blockMouseSensors.Add(blockMouseSensor);
		}

		_level = level;
		_gridPos = gridPos;
		_sourceType = sourceType;
		_onClick = onClick;
		InitPosition();
	}

	public virtual void ActivateBlock() {
		_meshRenderer.Get(gameObject).material = _normal;
		_collider.Get(gameObject).enabled = true;
	}

	public virtual void DeactivateBlock() {
		_meshRenderer.Get(gameObject).material = _transparent;
		_collider.Get(gameObject).enabled = false;

		for (int i = 0; i < 4; i++) {
			_blockMouseSensors[i].gameObject.SetActive(false);
		}
	}

	public virtual void ActivateBlockBuildingUI() {
		ActivateBlock();
		_collider.Get(gameObject).enabled = false;

		for (int i = 0; i < 4; i++) {
			_blockMouseSensors[i].gameObject.SetActive(true);
		}
	}

	public virtual void DeactivateBlockBuildingUI() {
		DeactivateBlock();
	}

	public BlockAbility GetAbility() {
		return _blockAbility.Get(gameObject);
	}
	#endregion
    
	#region PrivateMethod
	private void LoadMaterials() {
		_normal = Resources.Load<Material>(Constants.ResourcesPath.Materials.BLOCK_NORMAL_MATERIAL_PATH);
		_transparent = Resources.Load<Material>(Constants.ResourcesPath.Materials.BLOCK_TRANSPARENT_MATERIAL_PATH);
	}

	private void InitPosition() {
		transform.localPosition = GridManager.Instance.GetLocalPosition(_gridPos);
	}

	private void SensorMouseOver(Vector2Int direction) {
		GridManager.Instance.ShowGridSelector(_level, _gridPos + direction);
	}

	private void SensorMouseExit(Vector2Int direction) {
		GridManager.Instance.HideGridSelector();	
	}

	private void SensorMouseClick(Vector2Int direction) {
		GridManager.Instance.InstallNewBlock(_level, _gridPos + direction);
	}

	private void OnMouseDown() {
		_onClick?.Invoke(_level, _gridPos);
	}
	#endregion
}

}