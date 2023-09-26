using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core
{

	public class Block : MonoBehaviour
	{
		#region PublicVariables
		public BlockData Data => _blockData;
		public BlockAbility Ability => _blockAbility.Get(gameObject);
		private ComponentGetter <Rigidbody> _rigidbody = new ComponentGetter<Rigidbody>(TypeOfGetter.This);
			#endregion


		#region PrivateVariables
		private static Material _normal;
		private static Material _transparent;

		[SerializeField] private Vector2Int _gridPos;
		[SerializeField] private int _height;
		private bool isDeleting = false;
		private ComponentGetter<MeshRenderer> _meshRenderer
			= new ComponentGetter<MeshRenderer>(TypeOfGetter.ChildByName, "Renderer");
		private ComponentGetter<Collider> _collider
			= new ComponentGetter<Collider>(TypeOfGetter.This);
		private ComponentGetter<BlockAbility> _blockAbility
			= new ComponentGetter<BlockAbility>(TypeOfGetter.This);

		protected List<BlockMouseSensor> _blockMouseSensors;

		private int _level;
		private BlockData _blockData;
		private Action<int, Vector2Int> _onClick;
        [SerializeField] private float _deletingTime = 1.5f;
        #endregion

        #region PublicMethod
        public void Init(int level, Vector2Int gridPos, BlockData blockData, Action<int, Vector2Int> onClick)
		{
			_rigidbody.Get(gameObject).isKinematic = true;
			if (_normal == null || _transparent == null)
			{
				LoadMaterials();
			}

			_blockMouseSensors = new List<BlockMouseSensor>();

			for (int i = 0; i < 4; i++)
			{
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

			if (_blockData != null)
			{
				_blockAbility.Get(gameObject).Init(_blockData);
			}

			InitPosition();
		}

		public virtual void ActivateBlock()
		{
			_meshRenderer.Get(gameObject).material = _normal;
			_collider.Get(gameObject).enabled = true;
		}

		public virtual void DeactivateBlock()
		{
			_meshRenderer.Get(gameObject).material = _transparent;
			_collider.Get(gameObject).enabled = false;

			for (int i = 0; i < 4; i++)
			{
				_blockMouseSensors[i].gameObject.SetActive(false);
			}
		}

		public virtual void ActivateBlockBuildingUI()
		{
			ActivateBlock();
			_collider.Get(gameObject).enabled = false;

			for (int i = 0; i < 4; i++)
			{
				_blockMouseSensors[i].gameObject.SetActive(true);
			}
		}

		public virtual void DeactivateBlockBuildingUI()
		{
			DeactivateBlock();
		}

		public BlockAbility GetAbility()
		{
			return _blockAbility.Get(gameObject);
		}
		#endregion

		#region PrivateMethod
		private void LoadMaterials()
		{
			_normal = Resources.Load<Material>(Constants.ResourcesPath.Materials.BLOCK_NORMAL_MATERIAL_PATH);
			_transparent = Resources.Load<Material>(Constants.ResourcesPath.Materials.BLOCK_TRANSPARENT_MATERIAL_PATH);
		}

		private void InitPosition()
		{
			transform.localPosition = GridManager.Instance.GetLocalPosition(_gridPos);
		}

		private void SensorMouseOver(Vector2Int direction)
		{
			GridManager.Instance.ShowGridSelector(_level, _gridPos + direction);
		}

		private void SensorMouseExit(Vector2Int direction)
		{
			GridManager.Instance.HideGridSelector();
		}

		private void SensorMouseClick(Vector2Int direction)
		{
			GridManager.Instance.InstallNewBlock(_level, _gridPos + direction);
		}

		private void OnMouseHover()
		{
			_onClick?.Invoke(_level, _gridPos);

            if (GridManager.Instance.State == GridManager.BuildingState.Building)
            {
                if (Input.GetMouseButtonDown(0))
                {
					print(gameObject.name);
                }
                else if (Input.GetMouseButtonDown(1))
                {
					deletingObject();
                }
               
            }


        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
				deletingObject();
            }
        }

		public void deletingObject()
		{
			if (isDeleting == false)
            StartCoroutine(nameof(IE_deleting));
        }


        IEnumerator IE_deleting ()
		{
			isDeleting = true;	
			_rigidbody.Get(gameObject).isKinematic = false;
            yield return new WaitForSeconds(_deletingTime);
			Destroy(gameObject);
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