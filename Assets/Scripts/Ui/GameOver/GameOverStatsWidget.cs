using TMPro;
using UnityEngine;

namespace Ui.GameOver
{
    public class GameOverStatsWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_wavesText;

        public void UpdateStats()
        {
            WaveSpawner waveSpawner = FindFirstObjectByType<WaveSpawner>();
            m_wavesText.text = waveSpawner.CurrentWave.ToString("000");
        }
    }
}