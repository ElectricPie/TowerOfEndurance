using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public event Action OnGameOverEvent = delegate { };

    private PlayerManager m_player;
    private WaveSpawner m_waveSpawner = null;
    private TowerAbilities m_towerAbilities = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Multiple GameManager instances found in the scene. Destroying {gameObject.name}.", this);
            Destroy(this);
            return;
        }
        
        Instance = this;
        
        // Get components
        m_waveSpawner = GetComponent<WaveSpawner>();
        m_towerAbilities = FindFirstObjectByType<TowerAbilities>();
        
        // Get the player
        m_player = FindFirstObjectByType<PlayerManager>();

        m_player.GetComponent<PlayerLivesManager>().OnZeroLivesLeftEvent += GameOver;
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Player has no lives left.");
        
        m_waveSpawner.StopSpawning();
        m_towerAbilities.StopAllAbilities();
        
        OnGameOverEvent.Invoke();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}