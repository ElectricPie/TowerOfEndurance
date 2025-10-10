using TMPro;
using Ui.Hud;
using Ui.WidgetControllers;
using UnityEngine;

namespace Ui.Wave
{
    public class WaveTrackerWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_waveNumberText;

        private void Awake()
        {
            WaveWidgetController widgetController = HudController.Instance.WaveWidgetController;
            widgetController.OnWaveNumberChanged += OnWaveStarted;
        }

        public void OnWaveStarted(int waveNumber)
        {
            m_waveNumberText.text = (waveNumber + 1).ToString("000");
        }
    }
}
