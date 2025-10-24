using UnityEngine;

public class HealZone : MonoBehaviour
{
    public float radius = 5f;     
    public int healAmount = 50;      
    public LayerMask playerMask;       
    public float zoneLifeTime = 3f; 
    public bool destroyOnHeal = true;  

    void OnEnable()
    {
        if (zoneLifeTime > 0f)
            Destroy(gameObject, zoneLifeTime);
    }

    void Update()
    {
        var cols = Physics.OverlapSphere(transform.position, radius, playerMask, QueryTriggerInteraction.Ignore);
        if (cols.Length == 0) return;

        foreach (var col in cols)
        {
            var living = col.GetComponentInParent<LivingEntity>() ?? col.GetComponent<LivingEntity>();
            if (living == null || living.IsDead) continue;

            float before = living.HP;
            float after = Mathf.Min(living.maxHP, living.HP + healAmount);

            living.HP = after;

            Debug.Log($"[HealZone] {living.name} HP {before:F0} ¡æ {after:F0}");

            if (destroyOnHeal)
            {
                Destroy(this);
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 1f, 0.4f, 0.25f);
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = new Color(0.2f, 1f, 0.4f, 1f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
