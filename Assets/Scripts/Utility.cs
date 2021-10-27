using UnityEngine;

public class Utility
{

    public static bool GetPointUnderCursor(LayerMask groundLayer, out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, groundLayer))
        {
            point = hit.point;
            return true;
        }
        else
        {
            point = hit.point;
            return false;
        }

    }
}