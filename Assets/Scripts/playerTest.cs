using UnityEngine;

public class playerTest : Living
{
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
