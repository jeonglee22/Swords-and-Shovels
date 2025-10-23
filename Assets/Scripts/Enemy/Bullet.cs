using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float moveSpeed = 10f;
    public int Damage { get; set; }
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 newpos = rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newpos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var target = other.gameObject.GetComponent<IDamagable>();
            if (target == null)
            {
                return;
            }

            target.OnDamage(Damage);

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
