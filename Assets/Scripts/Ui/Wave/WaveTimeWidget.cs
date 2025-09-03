using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Wave
{
    public class WaveTimeWidget : MonoBehaviour
    {
        [SerializeField] private Slider m_waveTimeSlider;

        [SerializeField, RequiredIn(PrefabKind.InstanceInScene)]
        private WaveSpawner m_waveSpawner;

        public void Update()
        {
            m_waveTimeSlider.value = m_waveSpawner.IsSpawningWave ? 0 : m_waveSpawner.CurrentWaveProgress();
        }
    }
}