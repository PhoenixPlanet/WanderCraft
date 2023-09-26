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
        public BlockCluster Cluster => _cluster;
		#endregion
	#region PrivateVariables
	private Material _normal;
	private static Material _transparent;


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

	private int _level;
	private BlockCluster _cluster;
	private BlockData _blockData;
	private Action<int, Vector2Int> _onClick;
	#endregion

	#region PublicMethod
	public void Init(int level, Vector2Int gridPos, BlockData blockData, Action<int, Vector2Int> onClick) {
		if (_transparent == null) {
			LoadMaterials();
		}

		_normal = Resources.Load<Material>(Constants.ResourcesPath.Materials.BLOCK_NORMAL_MATERIAL_PATH);
		
		_blockMouseSensors = new List<BlockMouseSensor>();

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

	public BlockAbility GetAbility() {
		return _blockAbility.Get(gameObject);
	}

	public void SetNormalMaterial(Material material) {
		_normal = material;
	}

	public void SetCluster(BlockCluster cluster) {
		_cluster = cluster;
	}

	public BlockCluster CheckLinkBelow(EBuildingType targetBuildingType) {
		if (_level == 0) {
			return null;
		}

		Block belowBlock = GridManager.Instance.GetBlock(_level - 1, _gridPos);
		if (belowBlock != null) {
			if (belowBlock.Data.BuildingType == targetBuildingType) {
				return belowBlock.Cluster;
			}
		}

		return null;
	}
	#endregion
    
	#region PrivateMethod
	private void LoadMaterials() {
		_transparent = Resources.Load<Material>(Constants.ResourcesPath.Materials.BLOCK_TRANSPARENT_MATERIAL_PATH);
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