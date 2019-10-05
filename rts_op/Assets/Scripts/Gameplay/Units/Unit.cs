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

    public Material team1mat;
    public Material team2mat;

    [HideInInspector]
    public int lastChoiceIndex = -1;

    public UnitData trooperDataType;

    public void Start()
    {
        GetComponent<Damageable>().currentHp = trooperDataType.maxHp;

        if (GetComponent<NavMeshAgent>().speed != trooperDataType.movementSpeed)
        {
            GetComponent<NavMeshAgent>().speed = trooperDataType.movementSpeed;
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        Material newMat = OwnerActorNumber == 1 ? team1mat : team2mat;

        if(newMat != meshRenderer.sharedMaterial)
        {
            meshRenderer.sharedMaterial = newMat;
        }
    }

    void WalkToMouse()
    {
        if (!Camera.main)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            pos = hit.point;
        }
        GetComponent<NavMeshAgent>().destination = pos;
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

        UnitTypes trooperTypeData = GameObject.Find("GlobalData").GetComponent<UnitTypes>();

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