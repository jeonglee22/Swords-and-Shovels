using UnityEngine;
using UnityEngine.AI;
using System.Threading;
using Cysharp.Threading.Tasks;

public class PlayerManager : MonoBehaviour
{
    private NavMeshAgent agent;
    public MouseAimManager aimManager;
    private PlayerAttack playerAttack;
    private PlayerMove playerMove;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMove = GetComponent<PlayerMove>();

        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;

            var currentMouseAim = aimManager.CurrentMouseAim;
            var aimTarget = aimManager.AimTarget;
            Debug.Log(currentMouseAim);
            switch ((MouseAim)currentMouseAim)
            {
                case MouseAim.DoorWay:
                    if (playerAttack.AttackCts != null)
                    {
                        playerAttack.CancelCts();
                    }
                    playerMove.MoveToAnoterDoor();
                    break;
                case MouseAim.Sword:
                    var target = aimTarget.GetComponent<LivingEntity>();
                    playerAttack.Attack(target).Forget();
                    break;
                case MouseAim.Target:
                    if (playerAttack.AttackCts != null)
                    {
                        playerAttack.CancelCts();
                    }
                    agent.SetDestination(playerMove.GetClickedGroud(mousePos));
                    break;
                case MouseAim.Pointer:
                default:
                    return;
            }
        }
    }
}
