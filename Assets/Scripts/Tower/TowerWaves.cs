using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerWaves : MonoBehaviour
{
    [SerializeField, Tooltip("The angle from the tower the spawn point will be in degrees")] 
    private float m_spawnPointAngle = 0.0f;
    [SerializeField] private float m_spawnPointDistance = 4.5f;
    [SerializeField] private Vector3 m_spawnPointOffset = new Vector3(0.0f, -0.8f, 0.0f);
    [SerializeField, Tooltip("If set to a value will move a new spawned unit left/right the amount of the value")]
    private float m_unitSpawnPointVariation = 2.0f;

    public event Action<Unit> OnUnitSpawnedEvent = delegate { };

    public float CurrentWaveRpm => m_waves[0].RotationsPerMinute;

    private List<Wave> m_waves;

    public Wave NewWave(float waveRotationSpeed, int waveUnitCount, int waveNumber)
    {
        // Create the game object to rotate the units
        GameObject waveGameObject = new GameObject("Wave")
        {
            transform =
            {
                parent = transform,
                localPosition = Vector3.zero,
                localScale = Vector3.one
            }
        };

        Wave newWave = new Wave(waveGameObject.transform, waveRotationSpeed, waveUnitCount, waveNumber);
        newWave.OnAllUnitsKilled += wave =>
        {
            m_waves.Remove(wave);
            Destroy(wave.WaveTransform.gameObject);
        };
        m_waves.Add(newWave);

        return newWave;
    }

    public void SpawnUnitToLatestWave(Unit unitPrefab, bool modifyUnit = false, float unitHealth = 1.0f, float moneyWorth = 1.0f)
    {
        if (unitPrefab is null)
            throw new Exception($"{name} is missing unitPrefab");
        
        Wave latestWave = m_waves[m_waves.Count - 1];
        
        Vector3 insideOfSpawn = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance - m_unitSpawnPointVariation);
        insideOfSpawn += m_spawnPointOffset;
        Vector3 outsideOfSpawn = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance + m_unitSpawnPointVariation);
        outsideOfSpawn += m_spawnPointOffset;
        float randomLerpValue = Random.Range(0.0f, 1.0f);
        Vector3 spawnPosition = Vector3.Lerp(insideOfSpawn, outsideOfSpawn, randomLerpValue);
        
        Unit newUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, latestWave.WaveTransform.transform).GetComponent<Unit>();
        if (modifyUnit)
        {
            newUnit.HealthComponent.UpdateMaxHealth(unitHealth, false);
            newUnit.MoneyComponent.MoneyWorth = moneyWorth;
        }
        
        latestWave.AddUnit(newUnit);

        OnUnitSpawnedEvent?.Invoke(newUnit);
    }
    
    /// <summary>
    /// Gets the earliest spawned <c>Unit</c> in the earliest wave
    /// </summary>
    /// <returns>The oldest spawned <c>Unit</c> if there are waves otherwise null</returns>
    public Unit GetOldestUnit()
    {
        if (m_waves == null)
            return null;

        if (m_waves.Count == 0)
            return null;

        return m_waves[0].GetFirstUnit();
    }

    public Unit GetRandomUnit()
    {
        if (m_waves == null)
            return null;

        if (m_waves.Count == 0)
            return null;

        int randomWaveIndex = Random.Range(0, m_waves.Count - 1);
        Wave randomWave = m_waves[randomWaveIndex];
        return randomWave.GetRandomUnit();
    }

    // Start is called before the first frame update
    protected void Awake()
    {
        m_waves = new List<Wave>();
    }

    // Update is called once per frame
    protected void Update()
    {
        RotateWaves();
    }

    private void RotateWaves()
    {
        // Rotate each wave based on each waves' RotationsPerMinute
        foreach (Wave wave in m_waves)
        {
            // Convert RPM to angle by / 60 to get seconds and * by 360 to convert to angle
            float rotationAngle = wave.RotationsPerMinute / 60 * 360 * Time.deltaTime;
            wave.WaveTransform.Rotate(Vector3.up, rotationAngle);
        }
    }

    private Vector3 CalculatePositionAroundTower(float angleInDegrees, float distanceFromTower)
    {
        float angleInRadians = angleInDegrees * (Mathf.PI / 180);
        
        float x = distanceFromTower * Mathf.Cos(angleInRadians);
        float z = distanceFromTower * Mathf.Sin(angleInRadians);

        return transform.position + new Vector3(x, 0, z);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        // Draw spawn point
        Vector3 spawnPoint = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance) + m_spawnPointOffset;
        Gizmos.DrawSphere(spawnPoint, 0.5f);
        
        // Draw spawn variation
        Vector3 insideOfSpawn = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance - m_unitSpawnPointVariation);
        insideOfSpawn += m_spawnPointOffset;
        Vector3 outsideOfSpawn = CalculatePositionAroundTower(m_spawnPointAngle, m_spawnPointDistance + m_unitSpawnPointVariation);
        outsideOfSpawn += m_spawnPointOffset;
        Gizmos.DrawLine(insideOfSpawn, outsideOfSpawn);
    }
}

public class Wave
{
    public readonly Transform WaveTransform;
    public readonly float RotationsPerMinute;
    public int RemainingUnits { get; private set; }
    public int WaveNumber { get; private set; }

    private readonly List<Unit> m_units;

    public event Action<Wave> OnAllUnitsKilled = delegate { };

    public Wave(Transform waveTransform, float rotationsPerMinute, int waveUnitCount, int waveNumber)
    {
        WaveTransform = waveTransform;
        RotationsPerMinute = rotationsPerMinute;
        m_units = new List<Unit>(waveUnitCount);
        RemainingUnits = waveUnitCount;
        WaveNumber = waveNumber;
    }

    public void AddUnit(Unit newUnit)
    {
        m_units.Add(newUnit);
        newUnit.HealthComponent.OnKilledEvent += OnUnitKilled;
    }

    public Unit GetFirstUnit()
    {
        return m_units.Count == 0 ? null : m_units[0];
    }

    public Unit GetRandomUnit()
    {
        if (m_units.Count == 0)
            return null;

        int randomUnitIndex = Random.Range(0, m_units.Count - 1);
        return m_units[randomUnitIndex];
    }

    private void OnUnitKilled(GameObject killedUnitGameObject, GameObject killer)
    {
        Unit killedUnit = killedUnitGameObject.GetComponent<Unit>();
        m_units.Remove(killedUnit);

        RemainingUnits--;
        if (RemainingUnits <= 0)
        {
            OnAllUnitsKilled?.Invoke(this);
        }
    }
}
