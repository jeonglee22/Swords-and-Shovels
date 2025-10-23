using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerInteractive : MonoBehaviour
{
    public void Interaction()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInteractive, float.MaxValue, LayerMask.GetMask(LayerName.Interactive)))
        {
            hitInteractive.collider.gameObject.GetComponent<InteractiveObject>().Interaction();
        }
    }
}
