using UnityEngine;
using UnityEngine.AI;

public class StasisZone : MonoBehaviour
{
    public float radius = 6f;              
    public float freezeDuration = 1f;      
    public LayerMask enemyMask;            
    public float tickInterval = 0.2f;     
    public float zoneLifeTime = 3.3f;      

    float nextTick;

    void OnEnable()
    {
        nextTick = 0f;
        if (zoneLifeTime > 0f) Destroy(gameObject, zoneLifeTime);
    }

    void Update()
    {
        if (Time.time < nextTick) return;
        nextTick = Time.time + tickInterval;

        var cols = Physics.OverlapSphere(transform.position, radius, enemyMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < cols.Length; i++)
        {
            var col = cols[i];

            GameObject freezeTarget = null;

            var enemy = col.GetComponentInParent<Enemy>();
            if (enemy) freezeTarget = enemy.gameObject;

            if (!freezeTarget)
            {
                var agent = col.GetComponentInParent<NavMeshAgent>();
                if (agent) freezeTarget = agent.gameObject;
            }

            if (!freezeTarget && col.attachedRigidbody)
                freezeTarget = col.attachedRigidbody.gameObject;

            if (!freezeTarget)
                freezeTarget = col.gameObject;

            var freezer = freezeTarget.GetComponent<FreezeController>();
            if (!freezer) freezer = freezeTarget.AddComponent<FreezeController>();

            freezer.ApplyFreeze(freezeDuration);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.4f, 0.8f, 1f, 0.25f);
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = new Color(0.2f, 0.6f, 1f, 1f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
