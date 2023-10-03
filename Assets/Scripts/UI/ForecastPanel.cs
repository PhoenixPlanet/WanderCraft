using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static WaterController;

public class ForecastPanel : MonoBehaviour
{
    #region PublicVariables
    public Transform scrollViewContent;
    public GameObject forecastPanelPrefab;
    public WaterController waterController;
    public TextMeshProUGUI _currentTimeUI;

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    public void Update()
    {
        _currentTimeUI.text = waterController.getStatusName(waterController._currentWaveStatus) + "\n" + waterController._timer + "√ ";
    }
//sasd
    public void InstantiateForecastUI(Queue<WaveStatus> _waveStatusForecast)
    {
        if (_waveStatusForecast != null)
        {
            foreach (var waveStatus in _waveStatusForecast)
            {
                GameObject forecastPanel = Instantiate(forecastPanelPrefab, scrollViewContent);
                TextMeshProUGUI textMeshPro = forecastPanel.GetComponentInChildren<TextMeshProUGUI>();
                string target = waterController.getStatusName(waveStatus) +"\n≥Ù¿Ã : " + waterController.getStatusHeight(waveStatus) + "√˛";
                textMeshPro.text = target;
            }
        }
    }

    public void UpdateForecastUI(Queue<WaveStatus> _waveStatusForecast)
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
        InstantiateForecastUI(_waveStatusForecast);
    }
    #endregion

    #region PrivateMethod

    #endregion
}