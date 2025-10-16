using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerLivesManager : MonoBehaviour
{
    public event Action<int> OnCurrentLivesChangedEvent = delegate { };
    public event Action<int> OnMaxLivesChangedEvent = delegate { };
    
    [SerializeField, RequiredIn(PrefabKind.InstanceInScene)] private TowerWaves m_towerWaves;
    
    [SerializeField] private int m_initialLives = 40;
    
    public int MaxLives { get; private set; }
    public int CurrentLives { get; private set; }

    private void Awake()
    {
        MaxLives = m_initialLives;
        OnMaxLivesChangedEvent.Invoke(MaxLives);
        CurrentLives = m_initialLives;
        OnCurrentLivesChangedEvent.Invoke(CurrentLives);

        m_towerWaves.OnUnitSpawnedEvent += OnUnitsSpawned;
    }

    private void OnUnitsSpawned(Unit spawnedUnit, int waveNumber)
    {
        spawnedUnit.HealthComponent.OnKilledEvent += OnUnitKilled;

        UnitLiveCost cost = spawnedUnit.LivesCostComponent;
        CurrentLives -= cost.LiveCost;

        OnCurrentLivesChangedEvent.Invoke(CurrentLives);
    }

    private void OnUnitKilled(GameObject killedUnitGameObject, GameObject killerGameObject)
    {
        UnitLiveCost cost = killedUnitGameObject.GetComponent<UnitLiveCost>();
        CurrentLives += cost.LiveCost;

        OnCurrentLivesChangedEvent.Invoke(CurrentLives);
    }
}