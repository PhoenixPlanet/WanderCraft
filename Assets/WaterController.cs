using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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
    public Stack<WaveStatus> _waveStatusForecast;
    public WaveStatus _currentWaveStatus;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    private GameObject _waterGroup;
    [Header("IdleState")]
    [SerializeField] private float _IdleWaveHeight;
    [SerializeField] private float _IdleWaveTime;
    [Header("LowState")]
    [SerializeField] float _LowWaveHeight;
    [SerializeField] private float _LowWaveTime;
    [Header("MiddleState")]
    [SerializeField] private float _MiddleWaveHeight;
    [SerializeField] private float _MiddleWaveTime;
    [Header("HighState")]
    [SerializeField] private float _HighWaveHeight;
    [SerializeField] private float _HighWaveTime;
    private int CycleCount = 0;
    private bool _isExecutingCycle = false;
    #endregion

    #region PrivateMethod

    private void Start()
    {
        _waterGroup = GameObject.FindGameObjectWithTag("Water");
        _waveStatusForecast = new Stack <WaveStatus>();
        setRandomStatusOrder();
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
        _waveStatusForecast.Push(WaveStatus.Idle);
        for(int i = 0; i<10; i++)
        {
            WaveStatus waveStatus;
            int numValues = System.Enum.GetValues(typeof(WaveStatus)).Length;
            int randomIndex = Random.Range(1, numValues);
            waveStatus = (WaveStatus)randomIndex;
            _waveStatusForecast.Push(waveStatus);
        }
    }

    private void nextState()
    {
        _isExecutingCycle = true;
        if (_waveStatusForecast.Count == 0)
        {
            CycleCount++;
            setRandomStatusOrder();
        }
        WaveStatus nextWaveStatus = _waveStatusForecast.Pop();
        _currentWaveStatus = nextWaveStatus;
        print(_currentWaveStatus);

        switch (nextWaveStatus)
        {
            case WaveStatus.Idle:
                StartCoroutine(IdleWaveCoroutine());
                break;
            case WaveStatus.Low:
                StartCoroutine(LowWaveCoroutine());
                break;
            case WaveStatus.Middle:
                StartCoroutine(MiddleWaveCoroutine());

                break;
            case WaveStatus.High:
                StartCoroutine(HighWaveCoroutine());

                break;
        }
    }


    #endregion
}

