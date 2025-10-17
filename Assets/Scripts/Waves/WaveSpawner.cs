using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveSpawner : MonoBehaviour
{
    [SerializeField] protected TowerWaves TowerWaves;
    [SerializeField, Tooltip("The time in seconds before the next wave spawns if the current one is not complete. Starts after the last unit is spawned")] 
    private float m_maxTimeBetweenWaves = 30.0f;
    
    /// <summary>
    /// Called when a wave has started. Parameter is the wave number that just started.
    /// </summary>
    public event Action<int> OnWaveStartedEvent = delegate { };
    /// <summary>
    /// Called when a wave has ended. Parameter is the wave number that just ended.
    /// </summary>
    public event Action<int> OnWaveEndedEvent = delegate { };
    /// <summary>
    /// Called every frame with a value between 0 and 1 indicating the progress of the current wave. 1 means the wave is complete.
    /// </summary>
    public event Action<float> OnWaveProgressChangedEvent = delegate { };

    private bool m_isSpawningWave = false;
    private float m_currentWaveStartTime = 0.0f;
    private readonly Dictionary<int, IEnumerator> m_waveSpawningCoroutines = new Dictionary<int, IEnumerator>();

    protected abstract IEnumerator SpawnWave(int waveNumber);
    
    public int CurrentWave { get; private set; } = 0;
    
    /// <summary>
    /// A value between 0 and 1 indicating the progress of the current wave. 1 means the wave is complete.
    /// </summary>
    public float CurrentWaveProgress()
    {
        if (m_isSpawningWave) 
            return 0.0f;
        
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
        if (TowerWaves == null)
            throw new Exception($"Wave Manager on {name} is missing reference to a Tower Waves script");

        StartWave();
    }

    private void Update()
    {
        OnWaveProgressChangedEvent.Invoke(CurrentWaveProgress());
    }

    // This must be called by inheriting classes in the SpawnWave method when a wave is finished spawning all its units
    protected void WaveSpawningFinished(int waveNumber)
    {
        OnWaveEndedEvent.Invoke(waveNumber);
        
        m_currentWaveStartTime = Time.time;
        m_isSpawningWave = false;
        m_waveSpawningCoroutines.Remove(waveNumber);
        Invoke(nameof(StartNextWave), m_maxTimeBetweenWaves);
    }
    
    protected void StartNextWave()
    {
        CurrentWave++;
        
        StartWave();
    }

    private void StartWave()
    {
        OnWaveStartedEvent.Invoke(CurrentWave);
        
        m_isSpawningWave = true;
        IEnumerator newWaveCoroutine = SpawnWave(CurrentWave);
        m_waveSpawningCoroutines.Add(CurrentWave, newWaveCoroutine);
        StartCoroutine(newWaveCoroutine);
    }

    public abstract Unit GetCurrentWaveUnit();
    public abstract Unit GetNextWaveUnit();
}
