using System.Collections;
using UnityEngine;

namespace Waves
{
    public class GeneratedWaveSpawner : WaveSpawner
    {
        [SerializeField] private WaveSettingsScriptableObject m_waveSettings;
        [SerializeField] private float m_maxRandomDelayBetweenSpawns = 0.5f;

        protected override IEnumerator SpawnWave(int waveNumber)
        {
            Wave newWave = m_towerWaves.NewWave(m_waveSettings.WaveRotationalSpeed, m_waveSettings.UnitsPerWave, waveNumber);
            newWave.OnAllUnitsKilled += WaveFinished;

            float unitHealth = m_waveSettings.UnitHealthAtWave(waveNumber);
            float unitWorth = m_waveSettings.UnitMoneyWorthAtWave(waveNumber);

            for (int i = 0; i < m_waveSettings.UnitsPerWave; i++)
            {
                m_towerWaves.SpawnUnitToLatestWave(m_waveSettings.UnitBase, true, unitHealth, unitWorth);
                // Adding a small random delay to make the spawning less organised
                yield return new WaitForSeconds(m_waveSettings.TimeBetweenUnits + Random.Range(0, m_maxRandomDelayBetweenSpawns));
            }

            WaveSpawningFinished(waveNumber);
        }
    }
}