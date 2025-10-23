using Unity.VisualScripting;
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
    void FixedUpdate()
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
                var texture = mouseAimImages[(int)MouseAim.DoorWay];
                Cursor.SetCursor(texture, texture.Size() * 0.5f, CursorMode.Auto);
            }
        }
        else if (Physics.Raycast(ray, out var hitEnemy, float.MaxValue, LayerMask.GetMask(LayerName.Enemy)))
        {
            if (CheckMouseAimChange(MouseAim.Sword))
            {
                AimTarget = hitEnemy.collider.gameObject;
                Debug.Log(AimTarget.name);
                var texture = mouseAimImages[(int)MouseAim.Sword];
                Cursor.SetCursor(texture, texture.Size() * 0.5f, CursorMode.Auto);
            }
        }
        else if (Physics.Raycast(ray, out var hitInteractive, float.MaxValue, LayerMask.GetMask(LayerName.Interactive)))
        {
            if (CheckMouseAimChange(MouseAim.Interactive))
            {
                var texture = mouseAimImages[(int)MouseAim.Target];
                Cursor.SetCursor(texture, texture.Size() * 0.5f, CursorMode.Auto);
            }
        }
        else if (Physics.Raycast(ray, out var hitFloor, float.MaxValue, LayerMask.GetMask(LayerName.Floor)))
        {
            if(CheckMouseAimChange(MouseAim.Target))
            {
                var texture = mouseAimImages[(int)MouseAim.Target];
                Cursor.SetCursor(texture, texture.Size() * 0.5f, CursorMode.Auto);
            }
        }
        else
        {
            if (CheckMouseAimChange(MouseAim.Pointer))
            {
                var texture = mouseAimImages[(int)MouseAim.Pointer];
                Cursor.SetCursor(texture, texture.Size() * 0.5f, CursorMode.Auto);
            }
        }
    }
}
