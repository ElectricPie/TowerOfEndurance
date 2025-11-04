using System;
using System.Collections.Generic;
using AbilitySystem.Ability.Attributes;
using AbilitySystem.Ability.AttributeSets;
using AbilitySystem.Effect;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

[Serializable]
public class BasicAttackAbilityData : AbilityData
{
    /* Editor Values */
    [SerializeField] private AttributeIdScriptableObject m_damageLevelAttributeId;
    [SerializeField] private GameEffectScriptableObject m_damageEffect;

    [SerializeField] private TowerProjectile m_projectilePrefab = null;
    [SerializeField] private int m_poolSize = 10;

    public override AbilityData Clone()
    {
        BasicAttackAbilityData clone = (BasicAttackAbilityData)this.MemberwiseClone();
        // clone.m_baseAttackEffect = m_baseAttackEffect;
        clone.m_projectilePrefab = m_projectilePrefab;

        return clone;
    }
    
    
    /* Runtime Values */
    public event Action<Unit> OnTargetHit = delegate { };

    private ObjectPool<TowerProjectile> m_projectilePool;
    private float m_projectileSpeed;
    private TowerWaves m_waveComponent;

    private Unit m_currentTarget;
    private GameObject m_caster;

    private int m_level;
    
    public override void Init(AbilityInitData initData)
    {
        if (initData is not BasicAttackInitData projectileInitData)
            throw new Exception("Tried to initialized projectile ability with non TowerBasicAttackInitData");

        if (m_projectilePrefab == null)
            throw new Exception("Projectile Ability Data is missing projectile prefab");

        m_caster = initData.Caster;

        m_projectileSpeed = m_projectilePrefab.GetComponent<TowerProjectileMovement>().Speed;
        m_waveComponent = initData.Caster.GetComponent<TowerWaves>();

        m_projectilePool = new ObjectPool<TowerProjectile>(
            () =>
            {
                TowerProjectile projectile = Object.Instantiate(m_projectilePrefab);
                return projectile;
            },
            projectile =>
            {
                if (m_currentTarget == null)
                {
                    Debug.LogError("Tried to get projectile with null target");
                    return;
                }

                projectile.gameObject.SetActive(true);

                Vector3 spawnPoint = projectileInitData.SpawnTransform.position + projectileInitData.SpawnOffSet;
                projectile.transform.position = spawnPoint;
                Vector3 predictedPos = GetPredictedLocation(m_currentTarget.transform.position, spawnPoint);
                projectile.SetTarget(m_currentTarget, predictedPos);

                projectile.OnHitEvent += OnProjectileHit;
                projectile.OnTimeoutEvent += OnProjectileHit;
            },
            projectile =>
            {
                projectile.gameObject.SetActive(false);

                projectile.OnHitEvent -= OnProjectileHit;
                projectile.OnTimeoutEvent -= OnProjectileHit;
            },
            projectile => { Object.Destroy(projectile.gameObject); }, false, m_poolSize,
            m_poolSize * 2);
    }

    public override bool TryActivate(GameObject target, GameObject caster, int level = 1)
    {
        if (target == null)
            return false;
        
        m_currentTarget = target.GetComponent<Unit>();
        if (m_currentTarget == null)
            return false;

        m_projectilePool.Get();
        m_level = level;
        
        return true;
    }

    public override Dictionary<string, object> GetTooltipDataMap(int level)
    {
        return new Dictionary<string, object>();
    }

    public float GetDamage(int level)
    {
        return 1.0f;
        // return m_baseAttackEffect.DamageModifierCurve.Evaluate(level);
    }

    private Vector3 GetPredictedLocation(Vector3 targetCurrentPosition, Vector3 projectileSpawn)
    {
        float projectileDistanceToTarget = Vector3.Distance(targetCurrentPosition, projectileSpawn);

        float angularVelocity = m_waveComponent.CurrentWaveRpm * ((2 * Mathf.PI) / 60.0f);
        float timeToTarget = projectileDistanceToTarget / m_projectileSpeed;

        Vector3 centre = m_waveComponent.transform.position;
        // Keep it on one plane so don't need to handle the y-axis
        centre.y = targetCurrentPosition.y;

        // Calculate how much the unit will rotate in a frame
        float startingAngle = Mathf.Atan2(targetCurrentPosition.z - centre.z, targetCurrentPosition.x - centre.x);
        float angleMoved = angularVelocity * timeToTarget;
        float newAngle = startingAngle - angleMoved;

        float targetDistanceFromTower = Vector3.Distance(targetCurrentPosition, centre);
        // Calculate the predicted position
        float x = targetDistanceFromTower * Mathf.Cos(newAngle);
        float z = targetDistanceFromTower * Mathf.Sin(newAngle);

        // Need to account for the towers position
        return new Vector3(x + centre.x, centre.y, z + centre.z);
    }

    private void OnProjectileHit(TowerProjectile projectile)
    {
        if (projectile.Target != null)
        {
            ApplyEffects(projectile.Target);   
            OnTargetHit.Invoke(projectile.Target);
        }
        
        m_projectilePool.Release(projectile);
    }
    
    private void ApplyEffects(Unit target)
    {
        if (target == null)
            return;
        
        // target.EffectsContainer.ApplyEffect(m_caster, m_baseAttackEffect, m_level);
        
        target.EffectsContainer.ApplyEffect(m_caster, m_damageEffect, 1);
    }
}

public class BasicAttackInitData : AbilityInitData
{
    public Transform SpawnTransform { get; } = null;
    public Vector3 SpawnOffSet { get; }
    public AttributeSet AttributeSet { get; }

    public BasicAttackInitData(GameObject caster, Transform spawnTransform, Vector3 spawnOffset) : base(caster)
    {
        SpawnTransform = spawnTransform;
        SpawnOffSet = spawnOffset;
    }
}