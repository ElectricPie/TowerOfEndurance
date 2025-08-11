using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerAbilities : MonoBehaviour
{
    [SerializeField] private GameObject m_owningPlayer;
    [SerializeField] private TowerWaves m_towerWaves;
    
    [SerializeField] private TowerBasicAttackAbilityData m_basicAttack;

    [SerializeField] private Vector3 m_projectileSpawnPointOffset;

    public TowerBasicAttackAbilityInstance BasicAttackInstance { get; private set; }

    private readonly HashSet<AbilityInstance> m_onBasicAttackAbilities = new HashSet<AbilityInstance>();
    private readonly HashSet<AbilityInstance> m_onBasicHitAbilities = new HashSet<AbilityInstance>();
    private readonly HashSet<AbilityInstance> m_onAnyDamageAbilities = new HashSet<AbilityInstance>(); // TODO: Get when other abilities deal damage
    private readonly HashSet<AbilityInstance> m_timedAbilities = new HashSet<AbilityInstance>();

    public void AddAbility(AbilityData newAbility)
    {
        AbilityInitData newAbilityData = new AbilityInitData(m_owningPlayer);
        
        switch (newAbility.Trigger)
        {
            case AbilityTrigger.OnBasicAttackFired:
                m_onBasicAttackAbilities.Add(newAbility.CreateAbilityInstance(newAbilityData));
                break;
            case AbilityTrigger.OnBasicAttackHit:
                m_onBasicHitAbilities.Add(newAbility.CreateAbilityInstance(newAbilityData));
                break;
            case AbilityTrigger.OnAnyDamage:
                m_onAnyDamageAbilities.Add(newAbility.CreateAbilityInstance(newAbilityData));
                break;
            case AbilityTrigger.Timed:
                // TODO: Start coroutine for timed abilities
                m_timedAbilities.Add(newAbility.CreateAbilityInstance(newAbilityData));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void Awake()
    {
        if (m_basicAttack == null)
            throw new Exception($"{name} is missing Basic Attack ability");
        
        TowerBasicAttackInitData initData = new TowerBasicAttackInitData(m_owningPlayer)
        {
            SpawnTransform = transform,
            SpawnOffSet = m_projectileSpawnPointOffset,
            TowerWaveComponent = m_towerWaves
        };
        BasicAttackInstance = (TowerBasicAttackAbilityInstance)m_basicAttack.CreateAbilityInstance(initData);

        BasicAttackInstance.OnFire += () =>
        {
            foreach (AbilityInstance ability in m_onBasicAttackAbilities)
            {
                ability.TryActivate();
            }
        };
        BasicAttackInstance.OnTargetHit += target =>
        { 
            foreach (AbilityInstance ability in m_onBasicHitAbilities)
            {
                ability.TryActivate(target);
            }
            
            foreach (AbilityInstance ability in m_onAnyDamageAbilities)
            {
                ability.TryActivate(target);
            }
        };
    }

    private void Update()
    {
        BasicAttackInstance.TryActivate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPointOffset, 0.5f);
    }
}