using UnityEngine;

public class FireEffectMover : MonoBehaviour
{
    private Vector3 dir;
    private float speed;
    private float range;
    private float delay;
    private Vector3 startPos;

    public void Init(Vector3 thisDir, float thisSpeed, float thisRange, float thisDelay)
    {
        dir = thisDir;
        speed = Mathf.Max(0.001f, thisSpeed);
        range = Mathf.Max(0.1f, thisRange);
        delay = Mathf.Max(0f, thisDelay);
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        if ((transform.position - startPos).sqrMagnitude >= range * range)
            Destroy(gameObject, delay);
    }
}