using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractiveObject : MonoBehaviour
{
    public float moveDistance = 10f; // 내려갈 거리
    public float moveSpeed = 2f; // 이동 속도

    private bool isMoving = false;
    private Vector3 targetPosition;

    public void Interaction()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveDown());
        }
    }

    private IEnumerator MoveDown()
    {
        isMoving = true;

        targetPosition = transform.position + Vector3.down * moveDistance;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var agent = collision.gameObject.GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        agent.ResetPath();
    }
}
