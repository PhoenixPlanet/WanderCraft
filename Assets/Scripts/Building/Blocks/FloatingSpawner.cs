using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core
{

    public class FloatingSpawner : MonoBehaviour
    {
        #region PublicVariables
        #endregion

        #region PrivateVariables
        [SerializeField] private List<BlockDataSO> _blockDataList;
        [SerializeField] private float _spawnInterval;

        private Dictionary<ESourceType, List<BlockDataSO>> _blockDataDict;
        private Transform _floatingBlockParentTransform;
        private bool _hasInit = false;

        [SerializeField] Transform _upperTransform;
        [SerializeField] Transform _downTransform;
        [SerializeField] private float minSpawnOffset;
        [SerializeField] private float maxSpawnOffset;
        [SerializeField] float _upperSpawnProbability;

        [SerializeField] float _meatProbability = 0.6f;
        [SerializeField] float _fishProbability = 0.3f;
        [SerializeField] float _veggieProbability = 0.1f;
        #endregion

        #region PublicMethod
        public void Init()
        {
            if (_hasInit == true)
            {
                return;
            }

            _blockDataDict = new Dictionary<ESourceType, List<BlockDataSO>>();
            foreach (BlockDataSO blockData in _blockDataList)
            {
                if (_blockDataDict.ContainsKey(blockData.SourceType) == false)
                {
                    _blockDataDict.Add(blockData.SourceType, new List<BlockDataSO>());
                }
                _blockDataDict[blockData.SourceType].Add(blockData);
            }

            GameObject floatingBlockParentObj = new GameObject(Constants.Helper.Organizer.FLOATING_BLOCK_PARENT_NAME);
            _floatingBlockParentTransform = floatingBlockParentObj.transform;

            StartCoroutine(SpawnFloatingBlock());
            _hasInit = true;
        }
        #endregion

        #region PrivateMethod
        private void Update()
        {
            if (_hasInit == false)
            {
                return;
            }
        }

        private IEnumerator SpawnFloatingBlock()
        {
            int i = 0;
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);
                Vector3 spawnPos;
                Vector3 spawnDir;
                BlockDataSO blockData = _blockDataDict[getSpawnTarget()][i];
                if(Random.Range(0f, 1f) < _upperSpawnProbability)
                {
                    spawnPos = _upperTransform.position;
                    spawnDir = new Vector3(1, 0, 0);
                } else
                {
                    spawnPos = _downTransform.position;
                    spawnDir = new Vector3(0, 0, 1);
                }
                GameObject fb = Instantiate(blockData.FloatingPrefab, spawnPos, Quaternion.identity, _floatingBlockParentTransform);
                fb.GetComponent<FloatingBlock>().Init(blockData, OnFloatingBlockClicked);
                fb.GetComponent<FloatingBlock>().myDirection = spawnDir;
                i++;
                if (i >= _blockDataDict[ESourceType.Red].Count)
                {
                    i = 0;
                }
            }
        }

        ESourceType getSpawnTarget ()
        {
            float RandomValue = Random.Range(0f, 1f);
            if(RandomValue < _veggieProbability)
            {
                return ESourceType.Blue;
            } else if (RandomValue >= _veggieProbability && RandomValue < _fishProbability )  {
                return ESourceType.Green;

            } else if (RandomValue >= _meatProbability)
            {
                return ESourceType.Red;
            } else
            {
                return ESourceType.Red;
            }
        
        }

        private void OnFloatingBlockClicked(FloatingBlock floatingBlock)
        {
            if (GridManager.Instance.State == GridManager.BuildingState.Building)
            {
                GridManager.Instance.SelectFloatingBlock(floatingBlock);
            }
        }
        #endregion
    }

}