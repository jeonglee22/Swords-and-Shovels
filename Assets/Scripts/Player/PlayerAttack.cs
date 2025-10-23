using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{
    private int attack = 10;

    private Animator animator;
    public MouseAimManager aimManager;
    private NavMeshAgent agent;

    public CancellationTokenSource AttackCts { get; private set; } = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public async UniTaskVoid Attack(LivingEntity obj)
    {
        if (AttackCts != null) return;

        AttackCts = new CancellationTokenSource();

        await MoveToEnemy(obj.transform.position, AttackCts.Token);

        animator.SetTrigger(AnimatorParameter.Attack);

        obj.OnDamage(attack);

        AttackCts.Dispose();
        AttackCts = null;
    }

    private async UniTask MoveToEnemy(Vector3 pos, CancellationToken token)
    {
        agent.SetDestination(pos);

        while (!agent.isStopped)
        {
            await UniTask.Yield(cancellationToken: token);
        }
    }

    public void CancelCts()
    {
        AttackCts.Cancel();
        AttackCts.Dispose();
        AttackCts = null;
    }
}
