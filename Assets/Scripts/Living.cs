using System;
using UnityEngine;

public class Living : MonoBehaviour, IDamagable
{
    [SerializeField] protected float MaxHealth = 100f;

    public float Health { get; protected set; }
    public bool IsDead { get; protected set; }

    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = MaxHealth;
    }

    public virtual void OnDamage(float damage)
    {
        Health -= damage;

        if(Health <= 0 && !IsDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;
    }
}
