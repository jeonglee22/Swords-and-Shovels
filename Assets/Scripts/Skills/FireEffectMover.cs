using System.Linq;
using UnityEngine;

public class FireEffectMover : MonoBehaviour
{
    public int damage = 30;

    public float castRadius = 0.25f;                        
    public float skinWidth = 0.05f;
    public LayerMask damageMask = ~0;                      
    public QueryTriggerInteraction triggerMode = QueryTriggerInteraction.Collide;

    public bool stopOnNonDamageable = false;

    private Vector3 dir;
    private float speed;
    private float range;
    private float delay;
    private Vector3 startPos;

    public void Init(Vector3 dir, float speed, float range, float delay)
    {
        this.dir = dir.normalized;
        this.speed = Mathf.Max(0.001f, speed);
        this.range = Mathf.Max(0.1f, range);
        this.delay = Mathf.Max(0f, delay);
        this.startPos = transform.position;

        Debug.Log($"[FEM] Init s={this.speed}, r={this.range}, mask={damageMask.value}, trigger={triggerMode}");
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        if (step <= 0f) return;

        Debug.DrawRay(transform.position, dir * (step + skinWidth), Color.cyan, 0.1f);

        Ray ray = new Ray(transform.position, dir);
        RaycastHit[] hits = (castRadius > 0f)
            ? Physics.SphereCastAll(ray, castRadius, step + skinWidth, ~0, triggerMode)
            : Physics.RaycastAll(ray, step + skinWidth, ~0, triggerMode);

        if (hits.Length > 0)
        {
            hits = hits.OrderBy(h => h.distance).ToArray();

            string list = string.Join(", ", hits.Select(h => $"{h.collider.name}[L{h.collider.gameObject.layer}]@{h.distance:F3}"));
            Debug.Log($"[FEM] Hits: {list}");

            foreach (var h in hits)
            {
                var go = h.collider.gameObject;

                bool layerOK = ((damageMask.value & (1 << go.layer)) != 0);

                var dmg = go.GetComponentInParent<IDamagable>() ?? go.GetComponent<IDamagable>();

                if (layerOK && dmg != null)
                {
                    if (dmg is PlayerHealth) { Debug.Log("[FEM] Player는 무시"); continue; }

                    Debug.Log($"[FEM] Damage → {go.name}, -{damage}");
                    dmg.OnDamage(damage);
                    Destroy(gameObject, delay);
                    return;
                }
                else
                {
                    if (stopOnNonDamageable)
                    {
                        Debug.Log($"[FEM] Blocked by {go.name} (non-damageable). Destroy.");
                        Destroy(gameObject, delay);
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            if (stopOnNonDamageable)
            {
                var first = hits[0];
                Debug.Log($"[FEM] No damageable in path; first blocker {first.collider.name}. Destroy.");
                Destroy(gameObject, delay);
                return;
            }
        }

        transform.position += dir * step;

        if ((transform.position - startPos).sqrMagnitude >= range * range)
        {
            Debug.Log("[FEM] Range exceeded → destroy");
            Destroy(gameObject, delay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var go = other.gameObject;
        bool layerOK = ((damageMask.value & (1 << go.layer)) != 0);

        var dmg = go.GetComponentInParent<IDamagable>() ?? go.GetComponent<IDamagable>();
        if (layerOK && dmg != null)
        {
            if (dmg is PlayerHealth) return; 
            Debug.Log($"[FEM] Trigger Damage → {go.name}, -{damage}");
            dmg.OnDamage(damage);
            Destroy(gameObject, delay);
        }
        else if (stopOnNonDamageable)
        {
            Debug.Log($"[FEM] Trigger blocked by {go.name} (non-damageable). Destroy.");
            Destroy(gameObject, delay);
        }
    }
}
