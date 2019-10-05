using Photon.Pun;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Damageable))]
public class Building : MonoBehaviour
{
    public bool isBuilt
    {
        get
        {
            return remainingBuildTime < 0;
        }
    }

    [ReadOnly]
    public float remainingBuildTime;

    public float buildTime = 2f;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        remainingBuildTime = buildTime;
    }

    void Update()
    {
        remainingBuildTime -= Time.deltaTime;
        if(!isBuilt)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, 1-(remainingBuildTime/buildTime));
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(Building)), CanEditMultipleObjects]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Building targetBuilding = (target as Building);
        if (!Application.isPlaying)
        {
            targetBuilding.remainingBuildTime = targetBuilding.buildTime;
        }
        GUI.enabled = false;
        EditorGUILayout.Toggle("Is built", targetBuilding.isBuilt);
        GUI.enabled = true;
        DrawDefaultInspector();
    }
}
#endif
