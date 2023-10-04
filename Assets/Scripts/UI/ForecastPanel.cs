using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static WaterController;
using TH.Core;

public class ForecastPanel : MonoBehaviour
{
    #region PublicVariables
    public Transform scrollViewContent;
    public GameObject forecastPanelPrefab;
    public WaterController waterController;
    public TextMeshProUGUI _currentTimeUI;
    public int maxInstantiatablePanel = 7;

    #endregion

    #region PrivateVariables
    private float _timer = 0f;
    #endregion

    #region PublicMethod

    public void Update()
    {   
        _timer += Time.deltaTime;
        _currentTimeUI.text 
            = TH.Core.Constants.GameSetting.WaveInfo.WaveHeightInString(waterController.CurrentWaveData.waveLevel) 
            + "\n" 
            + (int)_timer + "/" + (int)waterController.CurrentWaveData.waveTime + "초";
    }

    public void InstantiateForecastUI(List<WaveData> waveDatas, int currentIdx)
    {
        if (waveDatas != null)
        {
            for (int i = currentIdx + 1; i < currentIdx + 1 + maxInstantiatablePanel; i++) {
                GameObject forecastPanel = Instantiate(forecastPanelPrefab, scrollViewContent);
                TextMeshProUGUI textMeshPro = forecastPanel.GetComponentInChildren<TextMeshProUGUI>();
                float waveLevel = waveDatas[i % waveDatas.Count].waveLevel;
                string target 
                    = TH.Core.Constants.GameSetting.WaveInfo.WaveHeightInString(waveLevel) 
                    + "\n높이 : " + (int)waveLevel + "층";
                textMeshPro.text = target;
            }
        }
        _timer = 0f;
    }

    public void UpdateForecastUI(List<WaveData> waveDatas, int currentIdx)
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
        InstantiateForecastUI(waveDatas, currentIdx);
    }
    #endregion

    #region PrivateMethod

    #endregion
}