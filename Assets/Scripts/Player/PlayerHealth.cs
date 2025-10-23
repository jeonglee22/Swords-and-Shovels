using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private GameObject DamageEffect;

    private void Awake()
    {
        
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);

    }

    public override void Die()
    {
        base.Die();
    }
}
