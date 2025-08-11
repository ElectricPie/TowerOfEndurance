using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class TowerAbilities : MonoBehaviour
{
    [SerializeField] private PlayerManager m_owningPlayer;
    [SerializeField] private TowerWaves m_towerWaves;
    
    [SerializeField] private AbilityScriptableObject m_basicAttackScriptableObject;

    [SerializeField] private Vector3 m_projectileSpawnPointOffset;

    [SerializeField] private AnimationCurve m_fireRateCurve;

    public AbilityInstance BasicAttackInstance { get; private set; }

    public float CurrentFireRate => 1 / m_fireRateCurve.Evaluate(FireRateLevel);
    public int FireRateLevel { get; private set; } = 1;

    private IEnumerator m_attackCoroutine;

    private readonly HashSet<AbilityInstance> m_onBasicAttackAbilities = new HashSet<AbilityInstance>();
    private readonly HashSet<AbilityInstance> m_onBasicHitAbilities = new HashSet<AbilityInstance>();
    private readonly HashSet<AbilityInstance> m_onAnyDamageAbilities = new HashSet<AbilityInstance>(); // TODO: Get when other abilities deal damage
    private readonly HashSet<AbilityInstance> m_timedAbilities = new HashSet<AbilityInstance>();
    
    public PlayerManager GetOwner()
    {
        return m_owningPlayer;
    }
    
    public void AddAbility(AbilityScriptableObject newAbility)
    {
        AbilityInitData newInitData = new AbilityInitData(gameObject);
        AbilityInstance newAbilityInstance = new AbilityInstance(newAbility.AbilityData, newInitData);
        
        switch (newAbility.AbilityData.Trigger)
        {
            case AbilityTrigger.OnBasicAttackFired:
                m_onBasicAttackAbilities.Add(newAbilityInstance);
                break;
            case AbilityTrigger.OnBasicAttackHit:
                m_onBasicHitAbilities.Add(newAbilityInstance);
                break;
            case AbilityTrigger.OnAnyDamage:
                m_onAnyDamageAbilities.Add(newAbilityInstance);
                break;
            case AbilityTrigger.Timed:
                // TODO: Start coroutine for timed abilities
                m_timedAbilities.Add(newAbilityInstance);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public float GetFireRateAt(int level)
    {
        return m_fireRateCurve.Evaluate(level);
    }

    public void IncreaseFireRateLevel()
    {
        FireRateLevel++;
    }
    
    protected void Awake()
    {
        if (m_basicAttackScriptableObject == null)
            throw new Exception($"{name} is missing Basic Attack ability");

        BasicAttackInitData initData = new BasicAttackInitData(gameObject, transform, m_projectileSpawnPointOffset);
        BasicAttackInstance = new AbilityInstance(m_basicAttackScriptableObject.AbilityData, initData);
        
        BasicAttackAbilityData basicAttackAbilityData = (BasicAttackAbilityData)BasicAttackInstance.AbilityData;
        basicAttackAbilityData.OnTargetHit += target =>
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

        m_attackCoroutine = Fire();
        StartCoroutine(m_attackCoroutine);
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            Unit target = m_towerWaves.GetOldestUnit();
            if (target == null)
            {
                yield return null;
                continue;
            }

            BasicAttackInstance.TryActivate(target.gameObject);
            foreach (AbilityInstance ability in m_onBasicAttackAbilities)
            {
                ability.TryActivate(target.gameObject);
            }

            yield return new WaitForSeconds(CurrentFireRate);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPointOffset, 0.5f);
    }
}