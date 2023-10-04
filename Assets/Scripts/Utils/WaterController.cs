using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading;
using UnityEditor;
using UnityEngine.UI;
using TH.Core;

public class WaterController : MonoBehaviour
{
    #region PublicVariables
    public float _Speed = 0.1f;
    public ForecastPanel forecastPanel;
    public WaveDataSO waveDataSO;
    public WaveDataSO zeroWaveDataSO;
    public List<Image> indicators;

    public WaveData CurrentWaveData
    {
        get
        {
            return waveDataSO._waveList[cycleIdx];
        }
    }

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    private GameObject _waterGroup;
    public int cycleIdx = 0;
    private bool _isExecutingCycle = false;
    #endregion

    #region PrivateMethod

    private void Start()
    {
        if (PlayerPrefs.GetInt("Level") == 0)
        {
            waveDataSO = zeroWaveDataSO;
        }
        
        _waterGroup = GameObject.FindGameObjectWithTag("Water");
        forecastPanel.InstantiateForecastUI(waveDataSO._waveList, cycleIdx);
        StartCoroutine(IE_totalCycle());
    }

    IEnumerator IE_totalCycle()
    {
        while (true)
        {
            if (_isExecutingCycle == false)
            {
                nextState();

            }
            yield return null;
        }
    }

    private IEnumerator WaveCoroutine(float height, float time) {
        moveSeaLevel(height);
        yield return new WaitForSeconds(5);
        GridManager.Instance.CalculateAgain();
        yield return new WaitForSeconds(time - 5);
        _isExecutingCycle = false;
    }

    void moveSeaLevel(float level)
    {
        _waterGroup.transform.DOMoveY(level, 5).SetEase(Ease.OutCubic);
    }

    private void nextState()
    {
        _isExecutingCycle = true;

        cycleIdx++;
        if (cycleIdx >= waveDataSO._waveList.Count)
        {
            cycleIdx = 0;
        }

        forecastPanel.UpdateForecastUI(waveDataSO._waveList, cycleIdx);
        moveIndicator();
        StartCoroutine(WaveCoroutine(CurrentWaveData.waveLevel, CurrentWaveData.waveTime));
    }

    private void moveIndicator()
    {
        foreach(var i in indicators)
        {
            i.color = Color.white;
        }
        indicators[(int)waveDataSO._waveList[(cycleIdx + 1) % waveDataSO._waveList.Count].waveLevel].color = Color.red;
        
    }

    #endregion
}

