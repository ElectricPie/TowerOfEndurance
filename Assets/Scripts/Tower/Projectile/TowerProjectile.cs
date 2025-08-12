using System;
using UnityEngine;

[RequireComponent(typeof(TowerProjectileMovement))]
public class TowerProjectile : MonoBehaviour
{
    public event Action<TowerProjectile> OnHitEvent = delegate { };
    public event Action<TowerProjectile> OnTimeoutEvent = delegate { };
    public event Action<TowerProjectile> OnTargetKilledEvent = delegate { };

    public GameObject Target { get; private set; } = null;

    [SerializeField, Min(0), Tooltip("Time after creation before projectile the projectile triggers its on hit event")] 
    private float m_timeoutTime = 2.0f;

    private TowerProjectileMovement m_movementComponent = null;

    private void Awake()
    {
        m_movementComponent = GetComponent<TowerProjectileMovement>();
    }

    public void SetTarget(Unit target, Vector3 targetPos)
    {
        if (target == null)
        {
            OnTimeoutEvent?.Invoke(this);
            return;
        }

        target.HealthComponent.OnKilledEvent += OnTargetKilled;
        
        m_movementComponent.TargetPos = targetPos;
        Target = target.gameObject;
        StartTimeout();
    }

    private void StartTimeout()
    {
        CancelInvoke(nameof(Timeout));
        Invoke(nameof(Timeout), m_timeoutTime);
    }

    private void Timeout()
    {
        Target = null;
        OnTimeoutEvent?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null)
            return;
        
        if (collision.gameObject != Target.gameObject) 
            return;
        
        CancelInvoke(nameof(Timeout));
        OnHitEvent?.Invoke(this);
    }

    private void OnTargetKilled(GameObject target, GameObject killer)
    {
        CancelInvoke(nameof(Timeout));
        OnTargetKilledEvent?.Invoke(this);
        
        Target = null;
    }
}