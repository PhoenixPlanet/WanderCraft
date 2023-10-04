using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading;
using UnityEditor;

public class WaterController : MonoBehaviour
{
    #region PublicVariables


    public enum WaveStatus
    {
        Idle,
        Low,
        Middle,
        High
    }
    WaveStatus _waveStatus;
    public float _Speed = 0.1f;
    public Queue<WaveStatus> _waveStatusForecast;
    public WaveStatus _currentWaveStatus;
    public ForecastPanel forecastPanel;

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    private GameObject _waterGroup;
    [Header("IdleState")]
    public float _IdleWaveHeight;
    public float _IdleWaveTime;
    [Header("LowState")]
    public float _LowWaveHeight;
    public float _LowWaveTime;
    [Header("MiddleState")]
    public float _MiddleWaveHeight;
    public float _MiddleWaveTime;
    [Header("HighState")]
    public float _HighWaveHeight;
    public float _HighWaveTime;
    private int CycleCount = 0;
    private bool _isExecutingCycle = false;

    public float _timer = 0;
    #endregion

    #region PrivateMethod

    private void Start()
    {
        _waterGroup = GameObject.FindGameObjectWithTag("Water");
        _waveStatusForecast = new Queue<WaveStatus>();
        for (int i = 0; i < 7; i++)
        {
            _waveStatusForecast.Enqueue(WaveStatus.Idle);
        }
        setRandomStatusOrder();
        forecastPanel.InstantiateForecastUI(_waveStatusForecast);
        StartCoroutine(IE_totalCycle());

    }

    public string getStatusName(WaveStatus waveStatus)
    {
        switch (waveStatus)
        {
            case WaveStatus.Idle: return "기본 파도";
            case WaveStatus.Low: return "낮은 파도";
            case WaveStatus.Middle: return "중간 파도";
            case WaveStatus.High: return "높은 파도";
            default: return "";
        }
    }

    public int getStatusHeight(WaveStatus waveStatus)
    {
        switch (waveStatus)
        {
            case WaveStatus.Idle: return (int)_IdleWaveHeight;
            case WaveStatus.Low: return (int)_LowWaveHeight;
            case WaveStatus.Middle: return (int)_MiddleWaveHeight;
            case WaveStatus.High: return (int)_HighWaveHeight;
            default: return 0;
        }
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

    private IEnumerator IdleWaveCoroutine()
    {

        yield return new WaitForSeconds(_IdleWaveTime);
        _isExecutingCycle = false;
    }

    private IEnumerator LowWaveCoroutine()
    {
        riseSeaLevel(_LowWaveHeight + CycleCount / 3 * 4);
        yield return new WaitForSeconds(_LowWaveTime);
        lowerSeaLevel(_IdleWaveHeight);
        yield return null;
        _isExecutingCycle = false;
    }

    private IEnumerator MiddleWaveCoroutine()
    {
        riseSeaLevel(_MiddleWaveHeight + CycleCount / 3 * 4);
        yield return new WaitForSeconds(_MiddleWaveTime);
        lowerSeaLevel(_MiddleWaveHeight);
        yield return null;
        _isExecutingCycle = false;
    }

    private IEnumerator HighWaveCoroutine()
    {
        riseSeaLevel(_HighWaveHeight + CycleCount / 3 * 4);
        yield return new WaitForSeconds(_HighWaveTime);
        lowerSeaLevel(_HighWaveHeight);
        yield return null;
        _isExecutingCycle = false;
    }


    void riseSeaLevel(float level)
    {
        _waterGroup.transform.DOMoveY(level, 5).SetEase(Ease.OutCubic);
    }

    void lowerSeaLevel(float level)
    {
        _waterGroup.transform.DOMoveY(level, 5).SetEase(Ease.OutCubic);
    }

    private void setRandomStatusOrder()
    {
        _waveStatusForecast.Enqueue(WaveStatus.Idle);
        _waveStatusForecast.Enqueue(WaveStatus.Idle);
        _waveStatusForecast.Enqueue(WaveStatus.Middle);
        _waveStatusForecast.Enqueue(WaveStatus.Low);
        for (int i = 0; i < 3; i++)
        {
            WaveStatus waveStatus;
            int numValues = System.Enum.GetValues(typeof(WaveStatus)).Length;
            int randomIndex = UnityEngine.Random.Range(1, numValues);
            waveStatus = (WaveStatus)randomIndex;
            _waveStatusForecast.Enqueue(waveStatus);
        }
    }

    private void nextState()
    {
        _isExecutingCycle = true;
        if (_waveStatusForecast.Count < 7)
        {
            CycleCount++;
            setRandomStatusOrder();
            forecastPanel.UpdateForecastUI(_waveStatusForecast);
        }
        WaveStatus nextWaveStatus = _waveStatusForecast.Dequeue();
        forecastPanel.UpdateForecastUI(_waveStatusForecast);
        _currentWaveStatus = nextWaveStatus;
        print(_currentWaveStatus);

        switch (nextWaveStatus)
        {
            case WaveStatus.Idle:
                _timer = 0f;
                StartCoroutine(IdleWaveCoroutine());
                break;
            case WaveStatus.Low:
                _timer = 0f;
                StartCoroutine(LowWaveCoroutine());
                break;
            case WaveStatus.Middle:
                _timer = 0f;
                StartCoroutine(MiddleWaveCoroutine());

                break;
            case WaveStatus.High:
                _timer = 0f;
                StartCoroutine(HighWaveCoroutine());

                break;
        }
    }


    #endregion
}

