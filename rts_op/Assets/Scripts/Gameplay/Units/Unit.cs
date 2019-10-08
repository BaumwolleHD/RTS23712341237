using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(TransformSynchronizer))]
[RequireComponent(typeof(Damageable))]
public class Unit : NetMonoBehaviour
{
    public int currentXp;

    [HideInInspector]
    public int lastChoiceIndex = -1;

    public UnitData trooperData;

    public void Start()
    {
        GetComponent<Damageable>().currentHp = trooperData.maxHp;

        if (GetComponent<NavMeshAgent>().speed != trooperData.movementSpeed)
        {
            GetComponent<NavMeshAgent>().speed = trooperData.movementSpeed;
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (hasOwner)
        {
            Material newMat = OwnerActorNumber == 1 ? gameManager.mapSettings.teamMaterials[0] : gameManager.mapSettings.teamMaterials[1];
            if (newMat != meshRenderer.sharedMaterial)
            {
                meshRenderer.sharedMaterial = newMat;
            }
        }
    }
    

    private Color c = Color.white;

    IEnumerator DrawPath(NavMeshPath path)
    {
        yield return new WaitForEndOfFrame();
        if (path.corners.Length >= 2)
        {
            switch (path.status)
            {
                case NavMeshPathStatus.PathComplete:
                    c = Color.white;
                    break;
                case NavMeshPathStatus.PathInvalid:
                    c = Color.red;
                    break;
                case NavMeshPathStatus.PathPartial:
                    c = Color.yellow;
                    break;
            }

            Vector3 previousCorner = path.corners[0];

            int i = 1;
            while (i < path.corners.Length)
            {
                Vector3 currentCorner = path.corners[i];
                Debug.DrawLine(previousCorner, currentCorner, c, 0.1f);
                previousCorner = currentCorner;
                i++;
            }
        }

    }

    void WalkToMouse()
    {
        if (!Camera.main)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.blue, 1f);
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            pos = hit.point;
        }
        GetComponent<NavMeshAgent>().destination = pos;
        IEnumerator e = DrawPath(GetComponent<NavMeshAgent>().path);
        StartCoroutine(e);

    }

    void Update()
    {
        if(photonView.IsMine && !Menu.instance.isPaused) WalkToMouse();
        HandleDeath();
        
    }

    void HandleDeath()
    {
        if (GetComponent<Damageable>().isDead)
        {
            Destroy();
        }
    }
}
/*
#if UNITY_EDITOR
[CustomEditor(typeof(Unit))]
public class TrooperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Unit thisTrooper = target as Unit;
        

        if (!Application.isPlaying && !GameObject.Find("GlobalData"))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity", OpenSceneMode.Additive);
        }

        Uni trooperTypeData = GameObject.Find("GlobalData").GetComponent<UnitTypes>();

        FieldInfo[] fields = typeof(UnitTypes).GetFields();
        List<string> trooperTypes = new List<string>();
        int count = 0;

        int currentIndex = 0;
        foreach (FieldInfo field in fields)
        {
            count++;
            if (field.Name != "instance")
            {
                string name = ((UnitData)field.GetValue(trooperTypeData)).name;
                trooperTypes.Add("(" + count.ToString() + ") " + (name != "" ? name : "Namenlos"));
                if (((UnitData)field.GetValue(trooperTypeData)) == thisTrooper.trooperDataType)
                {
                    currentIndex = count - 1;
                }

            }
        }
        if(thisTrooper.lastChoiceIndex == -1)
        {
            currentIndex = 0;
        }
        int choiceIndex = EditorGUILayout.Popup("Unit-type", currentIndex, trooperTypes.ToArray());
        if(choiceIndex != thisTrooper.lastChoiceIndex)
        {
            thisTrooper.trooperDataType = (UnitData)trooperTypeData.GetType().GetFields()[choiceIndex].GetValue(trooperTypeData);
            thisTrooper.lastChoiceIndex = choiceIndex;
        }

        if (!Application.isPlaying)
        {
            thisTrooper.Start();
        }

        thisTrooper.GetComponent<Damageable>().maxHp = thisTrooper.trooperDataType.maxHp;
    }
}
#endif

    */