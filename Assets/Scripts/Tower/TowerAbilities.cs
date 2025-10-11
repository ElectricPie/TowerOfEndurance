using System;
using System.Collections;
using System.Collections.Generic;
using AbilitySystem.Ability;
using AbilitySystem.Ability.Attributes;
using UnityEngine;

[RequireComponent(typeof(TowerAttributeSet))]
public class TowerAbilities : MonoBehaviour
{
    [SerializeField] private TowerWaves m_towerWaves;
    
    [SerializeField] private AbilityScriptableObject m_basicAttackScriptableObject;

    [SerializeField] private Vector3 m_projectileSpawnPointOffset;

    private AbilityInstance m_basicAttackInstance;
    private TowerAttributeSet m_attributeSet;
    
    private IEnumerator m_attackCoroutine;
    private readonly HashSet<AbilityInstance> m_onBasicAttackAbilities = new HashSet<AbilityInstance>();
    private readonly HashSet<AbilityInstance> m_onBasicHitAbilities = new HashSet<AbilityInstance>();
    private readonly HashSet<AbilityInstance> m_onAnyDamageAbilities = new HashSet<AbilityInstance>(); // TODO: Get when other abilities deal damage
    private readonly HashSet<AbilityInstance> m_timedAbilities = new HashSet<AbilityInstance>();
    
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
                StartCoroutine(TimedAbilityCoroutine(newAbilityInstance));
                m_timedAbilities.Add(newAbilityInstance);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    protected void Awake()
    {
        m_attributeSet = GetComponent<TowerAttributeSet>();
        
        if (m_basicAttackScriptableObject == null)
            throw new Exception($"{name} is missing Basic Attack ability");

        BasicAttackInitData initData = new BasicAttackInitData(gameObject, transform, m_projectileSpawnPointOffset);
        m_basicAttackInstance = new AbilityInstance(m_basicAttackScriptableObject.AbilityData, initData);
        
        BasicAttackAbilityData basicAttackAbilityData = (BasicAttackAbilityData)m_basicAttackInstance.AbilityData;
        basicAttackAbilityData.OnTargetHit += target =>
        { 
            foreach (AbilityInstance ability in m_onBasicHitAbilities)
            {
                ability.TryActivate(target.gameObject);
            }
            
            foreach (AbilityInstance ability in m_onAnyDamageAbilities)
            {
                ability.TryActivate(target.gameObject);
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

            m_basicAttackInstance.TryActivate(target.gameObject);
            foreach (AbilityInstance ability in m_onBasicAttackAbilities)
            {
                ability.TryActivate(target.gameObject);
            }

            yield return new WaitForSeconds(m_attributeSet.FireRate);
        }
    }

    private IEnumerator TimedAbilityCoroutine(AbilityInstance ability)
    {
        yield return new WaitForSeconds(ability.AbilityData.TriggerTime(ability.Level));
        while (true)
        {
            ability.TryActivate();
            yield return new WaitForSeconds(ability.AbilityData.TriggerTime(ability.Level));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPointOffset, 0.5f);
    }
}