using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static WaterController;

public class ForecastPanel : MonoBehaviour
{
    #region PublicVariables
    public Transform scrollViewContent;
    public GameObject forecastPanelPrefab;
    public WaterController waterController;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod



    public void InstantiateForecastUI(Queue<WaveStatus> _waveStatusForecast)
    {
        if (_waveStatusForecast != null)
        {
            foreach (var waveStatus in _waveStatusForecast)
            {
                GameObject forecastPanel = Instantiate(forecastPanelPrefab, scrollViewContent);
                TextMeshProUGUI textMeshPro = forecastPanel.GetComponentInChildren<TextMeshProUGUI>();
                string target = "";
                switch (waveStatus)
                {
                    case WaveStatus.Idle: target = "파도 없음\n" + (int)waterController._IdleWaveTime + "초\n" + "높이 : " + (int)waterController._IdleWaveHeight + "층"; break;
                    case WaveStatus.Low: target = "잔잔한 파도\n" + (int)waterController._LowWaveTime + "초" + "높이 : " + (int)waterController._LowWaveHeight + "층"; break;
                    case WaveStatus.Middle: target = "강한 파도\n" + (int)waterController._MiddleWaveTime + "초" + "높이 : " + (int)waterController._MiddleWaveHeight + "층"; break;
                    case WaveStatus.High: target = "쓰나미\n" + waterController._HighWaveTime + "초" + "높이 : " + (int)waterController._HighWaveHeight + "층"; break;
                }

                textMeshPro.text = target;
            }
        }
    }

    public void UpdateForecastUI(Queue<WaveStatus> _waveStatusForecast)
    {
        // Remove the old UI elements.
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new UI elements.
        InstantiateForecastUI(_waveStatusForecast);
    }
    #endregion

    #region PrivateMethod

    #endregion
}