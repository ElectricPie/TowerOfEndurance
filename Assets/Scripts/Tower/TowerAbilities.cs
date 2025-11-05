using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem.Ability;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using AbilitySystem.Ability.BasicAttack;
using UnityEngine;

public class TowerAbilities : MonoBehaviour
{
    [SerializeField] private TowerWaves m_towerWaves;
    
    [SerializeField] private AbilityScriptableObject m_basicAttackScriptableObject;

    [SerializeField] private Vector3 m_projectileSpawnPointOffset;

    [SerializeField] private AttributeIdScriptableObject m_damageAttributeId;
    [SerializeField] private AttributeIdScriptableObject m_fireRateAttributeId;
    
    private AbilityInstance m_basicAttackInstanceOld;

    private AttributeSet m_attributeSet;
    
    private IEnumerator m_attackCoroutine;
    private readonly HashSet<AbilityInstance> m_onBasicAttackAbilities = new HashSet<AbilityInstance>();
    private readonly HashSet<AbilityInstance> m_onBasicHitAbilities = new HashSet<AbilityInstance>();
    private readonly HashSet<AbilityInstance> m_onAnyDamageAbilities = new HashSet<AbilityInstance>(); // TODO: Get when other abilities deal damage
    private readonly HashSet<TimedAbilityInstance> m_timedAbilities = new HashSet<TimedAbilityInstance>();
    
    private readonly HashSet<AbilityInstance> m_allAbilities = new HashSet<AbilityInstance>();

    private bool m_isActive = true;
    
    public AbilityInstance AddAbility(AbilityScriptableObject newAbility)
    {
        AbilityInitData newInitData = new AbilityInitData(gameObject, newAbility);
        AbilityInstance newAbilityInstance = newAbility.AbilityData.CreateAbilityInstance(newInitData);
        
        switch (newAbility.Trigger)
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
                IEnumerator newTimedAbilityCoroutine = TimedAbilityCoroutine(newAbilityInstance);
                if (m_isActive){
                    StartCoroutine(newTimedAbilityCoroutine);
                }
                m_timedAbilities.Add(new TimedAbilityInstance(newAbilityInstance, newTimedAbilityCoroutine));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        m_allAbilities.Add(newAbilityInstance);
        return newAbilityInstance;
    }

    public bool HasAbilityOfType(AbilityScriptableObject ability)
    {
        return m_allAbilities.Any(abilityInstance => abilityInstance.Ability == ability);
    }

    public void StopAllAbilities()
    {
        m_isActive = false;
        
        if (m_attackCoroutine != null)
        {
            StopCoroutine(m_attackCoroutine);
            m_attackCoroutine = null;
        }
        
        foreach (TimedAbilityInstance timedAbilityInstance in m_timedAbilities)
        {
            StopCoroutine(timedAbilityInstance.Coroutine);
        }
    }
    
    protected void Start()
    {
        m_attributeSet = GetComponent<AttributeSet>();
        
        if (m_basicAttackScriptableObject == null)
            throw new Exception($"{name} is missing Basic Attack ability");

        AbilityInitData initData = new AbilityInitData(gameObject, m_basicAttackScriptableObject);
        m_basicAttackInstanceOld = new BasicAttackAbilityInstance(initData);
        
        BasicAttackAbilityInstance basicAttackAbilityData = (BasicAttackAbilityInstance)m_basicAttackInstanceOld;
        basicAttackAbilityData.OnTargetHit += target =>
        {
            if (!m_isActive)
                return;
            
            if (target == null)
                return;
            
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
        while (m_isActive)
        {
            Unit target = m_towerWaves.GetOldestUnit();
            if (target == null)
            {
                yield return null;
                continue;
            }

            m_basicAttackInstanceOld.TryActivate(target.gameObject);
            foreach (AbilityInstance ability in m_onBasicAttackAbilities)
            {
                ability.TryActivate(target.gameObject);
            }

            float waitTime = m_attributeSet.GetAttributeValue(m_fireRateAttributeId);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator TimedAbilityCoroutine(AbilityInstance abilityInstance)
    {
        yield return new WaitForSeconds(abilityInstance.Ability.GetTriggerTimeAt(abilityInstance.Level));
        while (m_isActive)
        {
            abilityInstance.TryActivate();
            yield return new WaitForSeconds(abilityInstance.Ability.GetTriggerTimeAt(abilityInstance.Level));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPointOffset, 0.5f);
    }
    
    private readonly struct TimedAbilityInstance : IEquatable<TimedAbilityInstance>
    {
        public readonly AbilityInstance Ability;
        public readonly IEnumerator Coroutine;
        
        public TimedAbilityInstance(AbilityInstance ability, IEnumerator coroutine)
        {
            Ability = ability;
            Coroutine = coroutine;
        }

        public bool Equals(TimedAbilityInstance other)
        {
            return Equals(Ability, other.Ability) && Equals(Coroutine, other.Coroutine);
        }

        public override bool Equals(object obj)
        {
            return obj is TimedAbilityInstance other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ability, Coroutine);
        }
    }
}