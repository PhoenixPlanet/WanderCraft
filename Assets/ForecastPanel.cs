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

    

    public void InstantiateForecastUI(Stack<WaveStatus> _waveStatusForecast)
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
                    case WaveStatus.Idle: target = "�ĵ� ����\n" + (int)waterController._IdleWaveTime +"��"; break;
                    case WaveStatus.Low: target = "������ �ĵ�\n" + (int)waterController._LowWaveTime + "��"; break;
                    case WaveStatus.Middle: target = "���� �ĵ�\n" + (int)waterController._MiddleWaveTime + "��"; break;
                    case WaveStatus.High: target = "������\n" + waterController._HighWaveTime + "��"; break;
                }

                textMeshPro.text = target;
            }
        }
    }

    public void UpdateForecastUI(Stack<WaveStatus> _waveStatusForecast)
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