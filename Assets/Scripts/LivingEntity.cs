using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float HP { get; set; }
    public float maxHP { get; set; } = 100;

    public bool IsDead { get; protected set; }
    public event Action OnDeath;

    private void OnEnable()
    {
        IsDead = false;
        HP = maxHP;
    }

    public virtual void OnDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0 && !IsDead)
        {
            HP = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;
    }
}
