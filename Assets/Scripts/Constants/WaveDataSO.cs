using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TH.Core;

[ManageableData]
[CreateAssetMenu(fileName = "WaveDataSO", menuName = "SO/WaveDataSO", order = 3)]
public class WaveDataSO : ScriptableObject
{
    [SerializeField]
    public List<WaveData> _waveList = new List<WaveData>();
}

[System.Serializable]
public class WaveData
{
    [HorizontalGroup("Split")]
    [VerticalGroup("Split/Left")]
    public float waveLevel;
    [VerticalGroup("Split/Right")]
    public float waveTime;

    public WaveData(float level, float time)
    {
        this.waveLevel = level;
        this.waveTime = time;
    }
}