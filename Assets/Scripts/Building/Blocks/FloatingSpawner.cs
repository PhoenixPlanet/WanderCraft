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
        [SerializeField] private float _spawnInterval;

        private Dictionary<ESourceType, List<BlockDataSO>> _blockDataDict;
        private Transform _floatingBlockParentTransform;
        private bool _hasInit = false;

        //[SerializeField] Transform _upperTransform;
        //[SerializeField] Transform _downTransform;
        private int minSpawnOffset = 0;
        //[SerializeField] private int maxSpawnOffset;
        //[SerializeField] float _upperSpawnProbability;

        //[SerializeField] float _RProbability = 0.6f;
        //[SerializeField] float _GProbability = 0.3f;
        //[SerializeField] float _BProbability = 0.1f;
        [SerializeField] private int _xOffset;
        [SerializeField] private int _zOffset;
        [SerializeField] private float _spawnYpos;
        #endregion

        #region PublicMethod

        public void Init()
        {
            
            if (_hasInit == true)
            {
               return;
            }

            _blockDataDict = new Dictionary<ESourceType, List<BlockDataSO>>();
            foreach (BlockDataSO blockData in GridManager.Instance.BlockDataList)
            {
                if (_blockDataDict.ContainsKey(blockData.SourceType) == false)
                {
                    _blockDataDict.Add(blockData.SourceType, new List<BlockDataSO>());
                }
                _blockDataDict[blockData.SourceType].Add(blockData);
            }

            GameObject floatingBlockParentObj = new GameObject(Constants.Helper.Organizer.FLOATING_BLOCK_PARENT_NAME);
            _floatingBlockParentTransform = floatingBlockParentObj.transform;

            _hasInit = true;
        }


        public void spawnBlocks(float interval, int minNum, int maxNum, float Rprob, float Gprob, float Bprob)
        {
            StartCoroutine(IEFeverSpawnFloatingBlock(interval, minNum, maxNum, Rprob, Gprob, Bprob));
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


        private IEnumerator IEFeverSpawnFloatingBlock(float _Interval, int minNum, int maxNum, float Rprob, float Gprob, float Bprob)
        {
            int n = UnityEngine.Random.Range(minNum, maxNum);
            for (int j = 0; j < n; j++)
            {
                yield return new WaitForSeconds(_Interval);
                Vector3 spawnPos;
                Vector3 spawnDir;

                for (int i = 0; i < 3; i++)
                {
                    BlockDataSO blockData = _blockDataDict[getSpawnTarget(Rprob, Gprob, Bprob)][i];
                    float randomX = Random.Range(-_xOffset, _xOffset);
                    float randomZ = Random.Range(-_zOffset, _zOffset);
                    spawnPos = new Vector3(randomX, _spawnYpos, randomZ);
                    spawnDir = new Vector3(1, 0, 0);
                    GameObject fb = Instantiate(blockData.FloatingPrefab, spawnPos, Quaternion.identity,
                        _floatingBlockParentTransform);
                    //fb.GetComponent<FloatingBlock>().Init(blockData);
                    fb.GetComponent<FloatingBlock>().myDirection = spawnDir;
                }
            
            }
        }

        ESourceType getSpawnTarget (float RProb, float GProb, float BProb)
        {
            float RandomValue = Random.Range(0f, 1f);
            if(RandomValue < BProb)
            {
                return ESourceType.Blue;
            } else if (RandomValue >= GProb && RandomValue < BProb )  {
                return ESourceType.Green;

            } else if (RandomValue >= RProb)
            {
                return ESourceType.Red;
            } else
            {
                return ESourceType.Red;
            }
        
        }
        #endregion
    }

}