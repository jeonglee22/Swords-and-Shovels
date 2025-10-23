using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public MouseAimManager aimManager;
    private PlayerAttack playerAttack;

    public Transform[] doorMoveEndPos;
    public Collider[] doorColliders;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        playerAttack = GetComponent<PlayerAttack>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(AnimatorParameter.Speed, Vector3.Magnitude(agent.velocity));
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
}
