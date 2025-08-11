using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class BasicAttackAbilityData : AbilityData, ISharedEffects
{
    /* Editor Values */
    [SerializeReference, BoxGroup("Base Attack")]
    private DamageEffect m_baseAttackEffect;

    [SerializeField] private TowerProjectile m_projectilePrefab = null;
    [SerializeField] private int m_poolSize = 10;

    public override AbilityData Clone()
    {
        BasicAttackAbilityData clone = (BasicAttackAbilityData)this.MemberwiseClone();
        clone.m_baseAttackEffect = m_baseAttackEffect;
        clone.m_projectilePrefab = m_projectilePrefab;

        return clone;
    }
    
    
    /* Runtime Values */
    public event Action<GameObject> OnTargetHit = delegate { };

    private ObjectPool<TowerProjectile> m_projectilePool;
    private float m_projectileSpeed;
    private TowerWaves m_waveComponent;
    private readonly List<GameEffect> m_effects = new List<GameEffect>();

    private Unit m_currentTarget;
    
    public override void Init(AbilityInitData initData)
    {
        if (initData is not BasicAttackInitData projectileInitData)
            throw new Exception("Tried to initialized projectile ability with non TowerBasicAttackInitData");

        if (m_projectilePrefab == null)
            throw new Exception("Projectile Ability Data is missing projectile prefab");

        m_projectileSpeed = m_projectilePrefab.GetComponent<TowerProjectileMovement>().Speed;
        m_waveComponent = initData.Caster.GetComponent<TowerWaves>();

        m_effects.Add(m_baseAttackEffect);

        m_projectilePool = new ObjectPool<TowerProjectile>(
            () =>
            {
                TowerProjectile projectile = Object.Instantiate(m_projectilePrefab);
                projectile.Owner = projectileInitData.Caster;
                projectile.Effects = this;
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
                projectile.OnTargetKilledEvent += OnProjectileHit;
            },
            projectile =>
            {
                projectile.gameObject.SetActive(false);

                projectile.OnHitEvent -= OnProjectileHit;
                projectile.OnTimeoutEvent -= OnProjectileHit;
                projectile.OnTargetKilledEvent -= OnProjectileHit;
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

        TowerProjectile projectile = m_projectilePool.Get();
        projectile.Level = level;
        
        return true;
    }

    /* ISharedEffects Interface begin */
    public List<GameEffect> GetEffects()
    {
        return m_effects;
    }
    /* ISharedEffects Interface end */

    public float GetDamage(int level)
    {
        return m_baseAttackEffect.DamageCurve.Evaluate(level);
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
        OnTargetHit?.Invoke(projectile.Target);
        m_projectilePool.Release(projectile);
    }
}

public class BasicAttackInitData : AbilityInitData
{
    public Transform SpawnTransform { get; } = null;
    public Vector3 SpawnOffSet { get; }= Vector3.zero;

    public BasicAttackInitData(GameObject caster, Transform spawnTransform, Vector3 spawnOffset) : base(caster)
    {
        SpawnTransform = spawnTransform;
        SpawnOffSet = spawnOffset;
    }
}