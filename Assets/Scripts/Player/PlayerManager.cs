using UnityEngine;
using UnityEngine.AI;
using System.Threading;
using Cysharp.Threading.Tasks;

public class PlayerManager : MonoBehaviour
{
    private NavMeshAgent agent;
    public MouseAimManager aimManager;
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;
    public PlayerInteractive playerInteractive;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();

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

            switch ((MouseAim)currentMouseAim)
            {
                case MouseAim.DoorWay:
                    playerMove.MoveToAnoterDoor();
                    break;
                case MouseAim.Sword:
                    playerAttack.IsAttack = true;
                    agent.SetDestination(playerMove.GetClickedGroud(mousePos));
                    break;
                case MouseAim.Target:
                    agent.SetDestination(playerMove.GetClickedGroud(mousePos));
                    break;
                case MouseAim.Interactive:
                    playerInteractive.Interaction();
                    break;
                case MouseAim.Pointer:
                default:
                    return;
            }
        }
    }
}
