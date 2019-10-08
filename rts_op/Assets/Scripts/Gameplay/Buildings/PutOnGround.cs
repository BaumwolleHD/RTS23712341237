using UnityEditor;
using UnityEngine;

public static class Extensions
{
    public static void PutOnGround(this MonoBehaviour obj)
    {
        RaycastHit hit;

        Ray ray = new Ray(new Vector3(obj.transform.position.x, 1000f, obj.transform.position.z), Vector3.down);


        if (Physics.Raycast(ray, out hit, 2000f))
        {
            EditorGUIUtility.PingObject(hit.transform);

            if (hit.transform.parent && hit.transform.parent.GetComponent<MapMagic.MapMagic>())
            {
                obj.transform.position = hit.point;
                obj.transform.Translate(new Vector3(0, 4f));
                Debug.Log("Hit point: " + hit.point);
            }
        }
    }
}