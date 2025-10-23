using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{
    private int attack = 10;

    private Animator animator;
    public MouseAimManager aimManager;
    private NavMeshAgent agent;
    public bool IsAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void Attack(LivingEntity obj)
    {
        var lookPos = obj.transform.position;
        lookPos.y = transform.position.y;

        transform.LookAt(lookPos);

        animator.SetTrigger(AnimatorParameter.Attack);
    }

    public void Hit()
    {
        aimManager.AimTarget.GetComponent<LivingEntity>().OnDamage(attack);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag(TagString.Enemy) && IsAttack)
        {
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            Attack(aimManager.AimTarget.GetComponent<LivingEntity>());
            IsAttack = false;
        }
    }
}
