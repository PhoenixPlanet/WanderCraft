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

    public TextMeshProUGUI _currentStatusUI;

    
    public enum WaveStatus
    {
       Idle,
       Low,
       Middle,
       High
    }
    WaveStatus _waveStatus;
    public float _Speed = 0.1f;
    public Stack<WaveStatus> _waveStatusForecast;
    public WaveStatus _currentWaveStatus;
    public ForecastPanel forecastPanel;

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    private GameObject _waterGroup;
    [Header("IdleState")]
    [SerializeField] private float _IdleWaveHeight;
    public float _IdleWaveTime;
    [Header("LowState")]
    [SerializeField] float _LowWaveHeight;
    public float _LowWaveTime;
    [Header("MiddleState")]
    [SerializeField] private float _MiddleWaveHeight;
    public float _MiddleWaveTime;
    [Header("HighState")]
    [SerializeField] private float _HighWaveHeight;
    public float _HighWaveTime;
    private int CycleCount = 0;
    private bool _isExecutingCycle = false;

    private float _timer = 0;
    #endregion

    #region PrivateMethod

    private void Start()
    {
        _waterGroup = GameObject.FindGameObjectWithTag("Water");
        _waveStatusForecast = new Stack <WaveStatus>();
        setRandomStatusOrder();
        forecastPanel.InstantiateForecastUI(_waveStatusForecast);
        StartCoroutine(IE_totalCycle());
        
    }

    IEnumerator IE_totalCycle()
    {
        while(true)
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


    void riseSeaLevel (float level)
    {
        _waterGroup.transform.DOMoveY(level, 5).SetEase(Ease.OutCubic);
    }

    void lowerSeaLevel(float level)
    {
        _waterGroup.transform.DOMoveY(level, 5).SetEase(Ease.OutCubic);
    }

    private void setRandomStatusOrder()
    {
        for(int i = 0; i<3; i++)
        {
            WaveStatus waveStatus;
            int numValues = System.Enum.GetValues(typeof(WaveStatus)).Length;
            int randomIndex = Random.Range(1, numValues);
            waveStatus = (WaveStatus)randomIndex;
            _waveStatusForecast.Push(waveStatus);
        }
        _waveStatusForecast.Push(WaveStatus.Idle);
        for(int i = 0; i<3; i++)
        {
            WaveStatus waveStatus;
            int numValues = System.Enum.GetValues(typeof(WaveStatus)).Length;
            int randomIndex = Random.Range(1, numValues);
            waveStatus = (WaveStatus)randomIndex;
            _waveStatusForecast.Push(waveStatus);
        }

        _waveStatusForecast.Push(WaveStatus.Middle);
        _waveStatusForecast.Push(WaveStatus.Low);
        _waveStatusForecast.Push(WaveStatus.Idle);
        _waveStatusForecast.Push(WaveStatus.Idle);

    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        switch (_currentWaveStatus)
        {
            case WaveStatus.Idle:
                _currentStatusUI.text = "파도 없음\n" +  ((int)_timer).ToString() + "/" + _IdleWaveTime +"초";
                break;
            case WaveStatus.Low:
                _currentStatusUI.text = "잔잔한 파도\n" +  ((int)_timer).ToString() + "/" + _LowWaveTime + "초";
                break;
            case WaveStatus.Middle:
                _currentStatusUI.text = "높은 파도\n" + ((int)_timer).ToString() + "/" +  _MiddleWaveTime + "초";
                break;
            case WaveStatus.High:
                _currentStatusUI.text = "쓰나미\n" + ((int)_timer).ToString() + "/" +  _HighWaveTime + "초";
                break;
        }
        
    }

    private void nextState()
    {
        _isExecutingCycle = true;
        if (_waveStatusForecast.Count == 0)
        {
            CycleCount++;
            setRandomStatusOrder();
            forecastPanel.UpdateForecastUI(_waveStatusForecast);
        }
        WaveStatus nextWaveStatus = _waveStatusForecast.Pop();
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

