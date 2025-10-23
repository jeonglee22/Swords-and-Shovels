using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestFallAndDie : MonoBehaviour
{
    private Vector3 target;
    private float speed;
    private float delay;
    private bool arrived;

    public void Init(Vector3 thisTarget, float thisSpeed, float thisDelay)
    {
        target = thisTarget;
        speed = Mathf.Max(0.1f, thisSpeed);
        delay = Mathf.Max(0f, thisDelay);
    }

    void Update()
    {
        if (arrived) return;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if ((transform.position - target).sqrMagnitude <= 0.0004f) // ¾à 2cm
        {
            arrived = true;
            Destroy(gameObject, delay);
        }
    }

}
