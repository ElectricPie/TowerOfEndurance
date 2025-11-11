using System;
using AbilitySystem.Effect;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;


namespace AbilitySystem.Ability.BasicAttack
{
    [Serializable]
    public class BasicAttackAbilityData : AbilityData
    {
        [SerializeField] private GameEffectScriptableObject m_damageEffect;

        [SerializeField] private TowerProjectile m_projectilePrefab = null;
        [SerializeField] private int m_poolSize = 10;

        public GameEffectScriptableObject DamageEffect => m_damageEffect;
        public TowerProjectile ProjectilePrefab => m_projectilePrefab;
        public int PoolSize => m_poolSize;
        
        public override AbilityInstance CreateAbilityInstance(AbilityInitData initData)
        {
            return new BasicAttackAbilityInstance(initData);
        }
    }

    public class BasicAttackAbilityInstance : AbilityInstance
    {
        public event Action<Unit> OnTargetHit = delegate { };

        private readonly ObjectPool<TowerProjectile> m_projectilePool;
        private readonly float m_projectileSpeed;
        private readonly TowerWaves m_waveComponent;

        private Unit m_currentTarget;
        private readonly GameObject m_source;
        private readonly BasicAttackAbilityData m_abilityData;

        public BasicAttackAbilityInstance(AbilityInitData initData) : base(initData)
        {
            if (initData.AbilityData is not BasicAttackAbilityData basicAttackAbilityData)
                throw new Exception("Tried to initialize Basic Attack ability with non BasicAttackAbilityData");
            m_abilityData = basicAttackAbilityData;
            
            if (m_abilityData.ProjectilePrefab == null)
                throw new Exception("Projectile Ability Data is missing projectile prefab");

            m_source = initData.Source;

            m_projectileSpeed = m_abilityData.ProjectilePrefab.GetComponent<TowerProjectileMovement>().Speed;
            m_waveComponent = initData.Source.GetComponent<TowerWaves>();

            m_projectilePool = new ObjectPool<TowerProjectile>(
                () =>
                {
                    TowerProjectile projectile = Object.Instantiate(m_abilityData.ProjectilePrefab);
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

                    Vector3 spawnPoint = m_source.transform.position +
                                         m_source.GetComponent<BasicAttackData>().ProjectileSpawnOffset;
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
                projectile => { Object.Destroy(projectile.gameObject); }, false, m_abilityData.PoolSize,
                m_abilityData.PoolSize * 2);
        }

        public override void TryActivate(GameObject target = null)
        {
            if (target == null)
                return;

            m_currentTarget = target.GetComponent<Unit>();
            if (m_currentTarget == null)
                return;

            m_projectilePool.Get();
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

            target.EffectsContainer.ApplyEffect(m_source, m_abilityData.DamageEffect, Level);
        }
    }
}