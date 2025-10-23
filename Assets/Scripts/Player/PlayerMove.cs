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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpPos1 = jumpLink.startPoint + jumpLink.transform.position;
        jumpPos2 = jumpLink.endPoint + jumpLink.transform.position;
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
                endPos = jumpPos2 + Vector3.forward * jumpDistance * 2f;
            }
            else
            {
                startPos = jumpPos2;
                endPos = jumpPos1 + Vector3.forward * jumpDistance * -2f;
            }
            IsCanJump = false;
            IsJump = true;
            isGround = false;
            agent.ResetPath();
            agent.enabled = false;
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
            agent.enabled = true;
            return;
        }

        var pos = GetBetweenJumpPoints(startPos, endPos, jumpStep++ / 100f, 0.3f);
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

    public Vector3 GetBetweenJumpPoints(Vector3 start, Vector3 end, float t, float offsetMul)
    {
        Vector2 start2 = new Vector2(start.x, start.z);
        Vector2 end2 = new Vector2(end.x, end.z);

        float horizontalDist = Vector2.Distance(start2, end2);
        float h = horizontalDist * offsetMul;

        Vector3 pos = Vector3.Lerp(start, end, t);
        float baseY = Mathf.Lerp(start.y, end.y, t);
        float arcY = 4f * h * t * (1f - t);

        pos.y = baseY + arcY;
        return pos;
    }
}
