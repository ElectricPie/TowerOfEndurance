using System.Collections;
using UnityEngine;

public class SetWaveSpawner : WaveSpawner
{
    [SerializeField] private WaveScriptableObject[] m_waves;

    protected override IEnumerator SpawnWave(int waveNumber)
    {
        WaveScriptableObject wave = m_waves[waveNumber];
        TowerWaves.NewWave(wave.WaveRotationSpeed, wave.UnitCount, waveNumber);
        
        for (int i = 0; i < wave.UnitCount; i++)
        {
            TowerWaves.SpawnUnitToLatestWave(wave.UnitPrefab);
            yield return new WaitForSeconds(wave.TimeSpawnGap);
        }
        
        WaveSpawningFinished(waveNumber);
    }

    public override Unit GetCurrentWaveUnit()
    {
        if (CurrentWave < m_waves.Length - 1)
            return null;
        
        
        return m_waves[CurrentWave].UnitPrefab;
    }

    public override Unit GetNextWaveUnit()
    {
        if (CurrentWave < m_waves.Length - 1)
            return null;
        
        return m_waves[CurrentWave + 1].UnitPrefab;
    }
}
