using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WaveSpawner : MonoBehaviour
{
    [SerializeField] protected TowerWaves m_towerWaves;
    [Tooltip("The time in seconds before the next wave spawns if the current one is not complete. Starts after the last unit is spawned")] 
    [SerializeField] private float m_maxTimeBetweenWaves = 30.0f;

    // Parameter is the new wave number
    public UnityEvent<int> OnWaveStartedEvent;

    public bool IsSpawningWave { get; private set; } = false;
    
    private int m_currentWave = 0;
    private float m_currentWaveStartTime = 0.0f;
    
    private readonly Dictionary<int, IEnumerator> m_waveSpawningCoroutines = new Dictionary<int, IEnumerator>();

    protected abstract IEnumerator SpawnWave(int waveNumber);
    
    /// <summary>
    ///  A value between 0 and 1 indicating the progress of the current wave. 1 means the wave is complete.
    /// </summary>
    public float CurrentWaveProgress()
    {
        float currentWaveTime = Time.time - m_currentWaveStartTime;
        return Mathf.Clamp01(currentWaveTime / m_maxTimeBetweenWaves);
    }
    
    protected void WaveFinished(Wave completedWave)
    {
        // Stop any waiting time
        CancelInvoke(nameof(StartNextWave));
        StartNextWave();
        m_waveSpawningCoroutines.Remove(completedWave.WaveNumber);
    }

    private void Start()
    {
        if (m_towerWaves == null)
            throw new Exception($"Wave Manager on {name} is missing reference to a Tower Waves script");

        StartWave();
    }

    // This must be called by inheriting classes in the SpawnWave method when a wave is finished spawning all its units
    protected void WaveSpawningFinished(int waveNumber)
    {
        m_currentWaveStartTime = Time.time;
        IsSpawningWave = false;
        m_waveSpawningCoroutines.Remove(waveNumber);
        Invoke(nameof(StartNextWave), m_maxTimeBetweenWaves);
    }
    
    protected void StartNextWave()
    {
        m_currentWave++;
        
        StartWave();
    }

    private void StartWave()
    {
        IsSpawningWave = true;
        IEnumerator newWaveCoroutine = SpawnWave(m_currentWave);
        m_waveSpawningCoroutines.Add(m_currentWave, newWaveCoroutine);
        StartCoroutine(newWaveCoroutine);
        
        OnWaveStartedEvent.Invoke(m_currentWave);
    }
}
