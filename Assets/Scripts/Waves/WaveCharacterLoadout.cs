using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Waves 
{
    [RequireComponent(typeof(TowerWaves))]
    public class WaveCharacterLoadout : MonoBehaviour
    {
        private TowerWaves m_towerWaves;
        private WaveSpawner m_waveSpawner;
        
        private readonly Dictionary<int, CharacterPackLoadout> m_waveCharacterLoadouts = new Dictionary<int, CharacterPackLoadout>();
        
        private void Awake()
        {
            m_towerWaves = GetComponent<TowerWaves>();
            m_towerWaves.OnUnitSpawnedEvent += OnUnitSpawned;

            m_waveSpawner = FindFirstObjectByType<WaveSpawner>();
            m_waveSpawner.OnWaveStartedEvent += OnWaveSpawningStarted;
            m_waveSpawner.OnWaveEndedEvent += OnWaveEnded;
        }

        private void OnUnitSpawned(Unit newUnit, int waveNumber)
        {
            if (m_waveCharacterLoadouts.TryGetValue(waveNumber, out CharacterPackLoadout loadout))
            {
                newUnit.GetComponent<CharacterPackAppearance>().SetLoadout(loadout);
            }
        }
        
        private void OnWaveSpawningStarted(int newWaveNumber)
        {
            Unit prefabUnit = m_waveSpawner.GetCurrentWaveUnit();
            CharacterPackAppearance characterPackAppearance = prefabUnit.GetComponent<CharacterPackAppearance>();

            CharacterPackLoadout characterPackLoadout = characterPackAppearance.GetRandomPackLoadout();
            m_waveCharacterLoadouts.TryAdd(newWaveNumber, characterPackLoadout);
        }

        private void OnWaveEnded(int waveNumber)
        {
            m_waveCharacterLoadouts.Remove(waveNumber);
        }
    }
}