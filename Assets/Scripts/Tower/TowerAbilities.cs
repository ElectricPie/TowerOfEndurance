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
    private readonly HashSet<AbilityInstanceOld> m_onBasicAttackAbilities = new HashSet<AbilityInstanceOld>();
    private readonly HashSet<AbilityInstanceOld> m_onBasicHitAbilities = new HashSet<AbilityInstanceOld>();
    private readonly HashSet<AbilityInstanceOld> m_onAnyDamageAbilities = new HashSet<AbilityInstanceOld>(); // TODO: Get when other abilities deal damage
    private readonly HashSet<TimedAbilityInstance> m_timedAbilities = new HashSet<TimedAbilityInstance>();
    
    private readonly HashSet<AbilityInstanceOld> m_allAbilities = new HashSet<AbilityInstanceOld>();

    private bool m_isActive = true;
    
    public AbilityInstanceOld AddAbility(AbilityScriptableObject newAbility)
    {
        AbilityInitData newInitData = new AbilityInitData(gameObject, newAbility.AbilityData);
        AbilityInstanceOld newAbilityInstanceOld = new AbilityInstanceOld(newAbility, newInitData);
        
        switch (newAbility.Trigger)
        {
            case AbilityTrigger.OnBasicAttackFired:
                m_onBasicAttackAbilities.Add(newAbilityInstanceOld);
                break;
            case AbilityTrigger.OnBasicAttackHit:
                m_onBasicHitAbilities.Add(newAbilityInstanceOld);
                break;
            case AbilityTrigger.OnAnyDamage:
                m_onAnyDamageAbilities.Add(newAbilityInstanceOld);
                break;
            case AbilityTrigger.Timed:
                IEnumerator newTimedAbilityCoroutine = TimedAbilityCoroutine(newAbilityInstanceOld);
                if (m_isActive){
                    StartCoroutine(newTimedAbilityCoroutine);
                }
                m_timedAbilities.Add(new TimedAbilityInstance(newAbilityInstanceOld, newTimedAbilityCoroutine));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        m_allAbilities.Add(newAbilityInstanceOld);
        return newAbilityInstanceOld;
    }

    public bool HasAbilityOfType(AbilityScriptableObject ability)
    {
        return m_allAbilities.Any(abilityInstance => abilityInstance.AbilityScriptableObject == ability);
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

        // BasicAttackInitData initData = new BasicAttackInitData(gameObject, transform, m_projectileSpawnPointOffset);
        AbilityInitData initData = new AbilityInitData(gameObject, m_basicAttackScriptableObject.AbilityData);
        m_basicAttackInstanceOld = new BasicAttackAbilityInstance();
        m_basicAttackInstanceOld.Init(initData);
        
        BasicAttackAbilityInstance basicAttackAbilityData = (BasicAttackAbilityInstance)m_basicAttackInstanceOld;
        basicAttackAbilityData.OnTargetHit += target =>
        {
            if (!m_isActive)
                return;
            
            if (target == null)
                return;
            
            foreach (AbilityInstanceOld ability in m_onBasicHitAbilities)
            {
                ability.TryActivate(target.gameObject);
            }
            
            foreach (AbilityInstanceOld ability in m_onAnyDamageAbilities)
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

            m_basicAttackInstanceOld.TryActivate(target.gameObject, gameObject);
            foreach (AbilityInstanceOld ability in m_onBasicAttackAbilities)
            {
                ability.TryActivate(target.gameObject);
            }

            float waitTime = m_attributeSet.GetAttributeValue(m_fireRateAttributeId);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator TimedAbilityCoroutine(AbilityInstanceOld ability)
    {
        yield return new WaitForSeconds(ability.AbilityScriptableObject.GetTriggerTimeAt(ability.Level));
        while (m_isActive)
        {
            ability.TryActivate();
            yield return new WaitForSeconds(ability.AbilityScriptableObject.GetTriggerTimeAt(ability.Level));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + m_projectileSpawnPointOffset, 0.5f);
    }
    
    private readonly struct TimedAbilityInstance : IEquatable<TimedAbilityInstance>
    {
        public readonly AbilityInstanceOld Ability;
        public readonly IEnumerator Coroutine;
        
        public TimedAbilityInstance(AbilityInstanceOld ability, IEnumerator coroutine)
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