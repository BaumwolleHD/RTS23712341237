using UnityEngine;
using UnityEditor;

public class Utils
{
    public static bool GetMouseTargetOfLayer(int layerMask, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 4000f, layerMask);
    }
}