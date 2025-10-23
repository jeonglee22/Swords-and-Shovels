using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;

            agent.SetDestination(GetClickedGroud(mousePos));
        }

        animator.SetFloat(AnimatorParameter.Speed, Vector3.Magnitude(agent.velocity));
    }

    private Vector3 GetClickedGroud(Vector3 mousePos)
    {
        var ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out var hitInfo, float.MaxValue, LayerMask.GetMask(LayerName.Floor)))
        {
            var pos = hitInfo.point;
            Debug.Log(pos);
            return pos;
        }
        Debug.Log(1);
        return transform.position;
    }
}
