using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Trooper : MonoBehaviour
{
    public int currentHp;
    public int currentXp;

    public TrooperDataType trooperDataType;

    public void Start()
    {
        currentHp = trooperDataType.maxHp;
    }

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit!");
            pos = hit.point;
        }
        if(Application.isPlaying)
        {
            GetComponent<NavMeshAgent>().destination = pos;
        }
        if(GetComponent<NavMeshAgent>().speed != trooperDataType.movementSpeed)
        {
            GetComponent<NavMeshAgent>().speed = trooperDataType.movementSpeed;
        }
    }
}

[CustomEditor(typeof(Trooper))]
public class TrooperEditor : Editor
{
    int choiceIndex;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Trooper thisTrooper = target as Trooper;
        

        if (!Application.isPlaying && !GameObject.Find("GlobalData"))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity", OpenSceneMode.Additive);
        }

        TrooperTypeData trooperTypeData = GameObject.Find("GlobalData").GetComponent<TrooperTypeData>();

        FieldInfo[] fields = typeof(TrooperTypeData).GetFields();
        List<string> trooperTypes = new List<string>();
        foreach (FieldInfo field in fields)
        {
            if (field.Name != "instance")
            {
                string name = ((TrooperDataType)field.GetValue(trooperTypeData)).name;
                trooperTypes.Add(name != "" ? name : "Namenlos");
            }
        }
        choiceIndex = EditorGUILayout.Popup("Unit-type",choiceIndex,trooperTypes.ToArray());

        thisTrooper.trooperDataType = (TrooperDataType)trooperTypeData.GetType().GetFields()[choiceIndex + 1].GetValue(trooperTypeData);
        

        if (!Application.isPlaying)
        {
            thisTrooper.Start();
            thisTrooper.Update();
        }

    }
}