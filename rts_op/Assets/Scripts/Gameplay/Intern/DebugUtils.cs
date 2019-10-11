using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections;

public class DebugUtils
{
    public static void DrawPath(NavMeshPath path, Color c)
    {

        IEnumerator e = DrawPathInternal(path, c);
        Menu.instance.StartCoroutine(e);
    }

    static IEnumerator DrawPathInternal(NavMeshPath path, Color color)
    {
        yield return new WaitForEndOfFrame();
        if (path.corners.Length >= 2)
        {
            switch (path.status)
            {
                case NavMeshPathStatus.PathComplete:
                    color = Color.white;
                    break;
                case NavMeshPathStatus.PathInvalid:
                    color = Color.red;
                    break;
                case NavMeshPathStatus.PathPartial:
                    color = Color.yellow;
                    break;
            }

            Vector3 previousCorner = path.corners[0];

            int i = 1;
            while (i < path.corners.Length)
            {
                Vector3 currentCorner = path.corners[i];
                Debug.DrawLine(previousCorner, currentCorner, color, 0.1f);
                previousCorner = currentCorner;
                i++;
            }
        }

    }
}