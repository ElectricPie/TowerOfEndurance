using System;
using UnityEngine;

[RequireComponent(typeof(TowerProjectileMovement))]
public class TowerProjectile : MonoBehaviour
{
    public event Action<TowerProjectile> OnHitEvent = delegate { };
    public event Action<TowerProjectile> OnTimeoutEvent = delegate { };
    public event Action<TowerProjectile> OnTargetKilledEvent = delegate { };

    public Unit Target { get; private set; } = null;

    [SerializeField, Min(0), Tooltip("Time after creation before projectile the projectile triggers its on hit event")] 
    private float m_timeoutTime = 2.0f;

    private TowerProjectileMovement m_movementComponent = null;

    private void Awake()
    {
        m_movementComponent = GetComponent<TowerProjectileMovement>();
    }

    private void Update()
    {
        if (Target == null)
        {
            OnTimeoutEvent.Invoke(this);
            return;
        }
    
        float distanceToTarget = Vector3.Distance(transform.position, m_movementComponent.TargetPos);
        if (distanceToTarget < 0.1f)
        {
            CancelInvoke(nameof(Timeout));
            OnHitEvent?.Invoke(this);
        }
    }

    public void SetTarget(Unit target, Vector3 targetPos)
    {
        if (target == null)
        {
            OnTimeoutEvent.Invoke(this);
            return;
        }

        target.HealthComponent.OnKilledEvent += OnTargetKilled;
        
        m_movementComponent.TargetPos = targetPos;
        Target = target;
        StartTimeout();
    }

    private void HitTarget()
    {
        CancelInvoke(nameof(Timeout));
        OnHitEvent?.Invoke(this);
    }

    private void StartTimeout()
    {
        CancelInvoke(nameof(Timeout));
        Invoke(nameof(Timeout), m_timeoutTime);
    }

    private void Timeout()
    {
        CancelInvoke(nameof(Timeout));
        
        // Hit the target if it's still valid as this means we missed
        if (Target != null)
        {
            HitTarget();
            return;
        }
        
        Target = null;
        OnTimeoutEvent.Invoke(this);
    }

    private void OnTargetKilled(GameObject target, GameObject killer)
    {
        CancelInvoke(nameof(Timeout));
        OnTargetKilledEvent.Invoke(this);
        
        Target = null;
    }
}