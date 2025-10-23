using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    [SerializeField] private float traceDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private int damage = 10;
    private float lastAttackTime;
    [SerializeField] private float attackInterval = 0.5f;

    [SerializeField] private EnemyDatas data;

    private Transform target;

    private NavMeshAgent agent;

    private Animator animator;

    private Collider EnemyCollider;

    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private EnemyType enemyType = EnemyType.Melee;

    [SerializeField] private Bullet bullet;

    private Status currentStatus;
    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;

            switch (currentStatus)
            {
                case Status.Idle:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    agent.isStopped = true;
                    break;
            }
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        EnemyCollider = GetComponent<Collider>();

        Setup(data);
    }

    public void Setup(EnemyDatas data)
    {
        maxHP = data.maxHp;
        damage = data.damage;
        agent.speed = data.speed;
        traceDistance = data.traceDistance;
        attackDistance = data.attackDistance;

        enemyType = data.type;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EnemyCollider.enabled = true;
        CurrentStatus = Status.Idle;
    }

    private void Update()
    {
        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:

                break;
        }

        Debug.Log(currentStatus);
    }

    protected override void Die()
    {
        base.Die();
        currentStatus = Status.Die;
        Destroy(gameObject, 3f);
    }

    private void UpdateIdle()
    {
        if(target != null && Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance);
        animator.SetFloat("Speed", 0f);
    }

    private void UpdateTrace()
    {
        if(target != null && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        if(target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position);

        animator.SetFloat("Speed", Vector3.Magnitude(agent.velocity));
    }

    private void UpdateAttack()
    {
        if(target == null || (target != null && Vector3.Distance(transform.position, target.position) > attackDistance))
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if(lastAttackTime + attackInterval < Time.time)
        {
            lastAttackTime = Time.time;

            var damagable = target.GetComponent<IDamagable>();
            if(damagable != null)
            {
                switch (enemyType)
                {
                    case EnemyType.Melee:
                        damagable.OnDamage(damage);
                        break;
                    case EnemyType.Ranged:
                        break;
                }

                animator.SetTrigger("Attack");
            }
        }
    }

    private void Shoot(Vector3 dir)
    {
        var prefab = Instantiate(bullet);
        prefab.gameObject.SetActive(true);
    }

    protected Transform FindTarget(float radius)
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
        if(colliders.Length == 0)
        {
            return null;
        }

        foreach(var col in colliders)
        {
            if(col.gameObject.tag == "Player")
            {
                return col.gameObject.transform;
            }
        }

        return null;
    }

    //애니메이션 이벤트
    public void Hit()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
