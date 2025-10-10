using Ui.Hud;
using Ui.WidgetControllers;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Wave
{
    public class WaveTimeWidget : MonoBehaviour
    {
        [SerializeField] private Slider m_waveTimeSlider;
        
        private void Awake()
        {
            WaveWidgetController widgetController = HudController.Instance.WaveWidgetController;
            widgetController.OnWaveProgressChanged += OnTimerChanged;
        }

        private void OnTimerChanged(float newValue)
        {
            m_waveTimeSlider.value = newValue;
        }
    }
}