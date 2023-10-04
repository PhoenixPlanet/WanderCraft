using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[ManageableData]
[CreateAssetMenu(fileName = "SpawnDataSO", menuName = "SO/SpawnDataSO", order = 4)]
public class SpawnDataSO : ScriptableObject
{
    [SerializeField]
    public List<SpawnData> spawnList = new List<SpawnData>();
}

[System.Serializable]
public class SpawnData
{
    [HorizontalGroup("Split")]
    [VerticalGroup("Split/Left")]
    public BlockDataSO block;

    [VerticalGroup("Split/Right")]
    public float spawnRate;
    [VerticalGroup("Split/Right")]
    public float spawnProbability;
}