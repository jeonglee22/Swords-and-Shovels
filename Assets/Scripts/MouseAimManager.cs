using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MouseAimManager : MonoBehaviour
{
    private Vector3 mousePosition;
    private MouseAim currentMouseAim = MouseAim.Pointer;
    public int CurrentMouseAim { get { return (int) currentMouseAim; } }
    public GameObject AimTarget { get; private set; }
    public Texture2D[] mouseAimImages;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(mouseAimImages[(int)MouseAim.Pointer], Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;

        ChangeMouseAim(mousePosition);
    }

    private bool CheckMouseAimChange(MouseAim aim)
    {
        var diffAim = currentMouseAim != aim;

        if (diffAim)
        {
            currentMouseAim = aim;
            return true;
        }

        return false;
    }

    private void ChangeMouseAim(Vector3 pos)
    {
        var ray = Camera.main.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out var hitDoor, float.MaxValue, LayerMask.GetMask(LayerName.Door)))
        {
            if (CheckMouseAimChange(MouseAim.DoorWay))
            {
                AimTarget = hitDoor.collider.gameObject;
                Cursor.SetCursor(mouseAimImages[(int)MouseAim.DoorWay], Vector2.zero, CursorMode.Auto);
            }
        }
        else if (Physics.Raycast(ray, out var hitEnemy, float.MaxValue, LayerMask.GetMask(LayerName.Enemy)))
        {
            if (CheckMouseAimChange(MouseAim.Sword))
            {
                AimTarget = hitEnemy.collider.gameObject;
                Cursor.SetCursor(mouseAimImages[(int)MouseAim.Sword], Vector2.zero, CursorMode.Auto);
            }
        }
        else if (Physics.Raycast(ray, out var hitFloor, float.MaxValue, LayerMask.GetMask(LayerName.Floor)))
        {
            if(CheckMouseAimChange(MouseAim.Target))
            {
                AimTarget = hitFloor.collider.gameObject;
                Cursor.SetCursor(mouseAimImages[(int)MouseAim.Target], Vector2.zero, CursorMode.Auto); 
            }
        }
        else
        {
            currentMouseAim = MouseAim.Pointer;
            Cursor.SetCursor(mouseAimImages[(int)MouseAim.Pointer], Vector2.zero, CursorMode.Auto);
        }
    }
}
