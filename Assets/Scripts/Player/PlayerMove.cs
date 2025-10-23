using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public MouseAimManager aimManager;
    private PlayerAttack playerAttack;

    public NavMeshLink jumpLink;
    public float jumpDistance = 2f;

    private Vector3 jumpPos1;
    private Vector3 jumpPos2;
    private Vector3 startPos;
    private Vector3 endPos;

    public Transform[] doorMoveEndPos;
    private Vector3 agentFinishPoint;

    private bool IsCanJump = true;
    private bool IsJump;
    private bool isGround;
    private int jumpStep = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        playerAttack = GetComponent<PlayerAttack>();

        jumpPos1 = jumpLink.startPoint;
        jumpPos2 = jumpLink.endPoint;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, jumpPos1) > jumpDistance &&
            Vector3.Distance(transform.position, jumpPos2) > jumpDistance && isGround)
            IsCanJump = true;

        if (!IsCanJump) return;

        animator.SetFloat(AnimatorParameter.Speed, Vector3.Magnitude(agent.velocity));

        if (Vector3.Distance(transform.position, jumpPos1) <= jumpDistance ||
            Vector3.Distance(transform.position, jumpPos2) <= jumpDistance)
        {
            Debug.Log(1);
            agentFinishPoint = aimManager.AimPosition.position;
            if(Vector3.Distance(transform.position, jumpPos1) <= jumpDistance)
            {
                startPos = jumpPos1;
                endPos = jumpPos2;
            }
            else
            {
                startPos = jumpPos2;
                endPos = jumpPos1;
            }
            IsCanJump = false;
            IsJump = true;
            isGround = false;
            agent.ResetPath();
        }
    }

    private void FixedUpdate()
    {
        if (!IsJump) return;

        if(jumpStep == 100)
        {
            jumpStep = 0;
            IsJump = false;
            isGround = true;
            agent.SetDestination(agentFinishPoint);
            return;
        }

        var pos = GetBetweenJumpPoints(startPos, endPos, jumpStep++);
        transform.position = pos;
        Debug.Log(pos);
    }

    public void MoveToAnoterDoor()
    {
        var door1Distance = Vector3.Distance(doorMoveEndPos[0].transform.position, transform.position);
        var door2Distance = Vector3.Distance(doorMoveEndPos[1].transform.position, transform.position);

        if(door1Distance < door2Distance)
        {
            agent.SetDestination(doorMoveEndPos[1].transform.position);
        }
        else
        {
            agent.SetDestination(doorMoveEndPos[0].transform.position);
        }
    }

    public Vector3 GetClickedGroud(Vector3 mousePos)
    {
        var ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out var hitInfo, float.MaxValue, LayerMask.GetMask(LayerName.Floor)))
        {
            var pos = hitInfo.point;
            return pos;
        }

        return transform.position;
    }

    public Vector3 GetBetweenJumpPoints(Vector3 start, Vector3 end, int step)
    {
        var centerPoint = (start + end) * 0.5f;

        Vector2 start2 = new Vector2(start.x, start.z);
        Vector2 end2 = new Vector2(end.x, end.z);

        var QuadraticFunc = new Func<Vector2, float>(x => { return Vector2.Distance(x, start2) * Vector2.Distance(x, end2); });
 
        float t = (float)step / 100;

        var xPos = Mathf.Lerp(start.x, end.x, t);
        var zPos = Mathf.Lerp(start.z, end.z, t);

        Vector3 point = new Vector3(xPos, QuadraticFunc(new Vector2(xPos, zPos)), zPos);
        return point;
    }
}
