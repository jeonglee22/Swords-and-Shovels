using UnityEngine;

public class DamageTextMove : MonoBehaviour
{
    private float velocity = 5f;

    private float deactiveInterval = 1f;
    private float deactiveTime = 0;

    private void OnEnable()
    {
        deactiveTime = 0;
    }

    private void FixedUpdate()
    {
        transform.position += velocity * Vector3.up * Time.deltaTime;
        deactiveTime += Time.deltaTime;
        if(deactiveTime > deactiveInterval)
            gameObject.SetActive(false);
    }
}
