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

    public int lastChoiceIndex;

    public TrooperDataType trooperDataType;

    public void Start()
    {
        currentHp = trooperDataType.maxHp;

        if (GetComponent<NavMeshAgent>().speed != trooperDataType.movementSpeed)
        {
            GetComponent<NavMeshAgent>().speed = trooperDataType.movementSpeed;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            pos = hit.point;
        }
        if(Application.isPlaying)
        {
            GetComponent<NavMeshAgent>().destination = pos;
        }
    }
}

[CustomEditor(typeof(Trooper))]
public class TrooperEditor : Editor
{
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
        int count = 0;

        int currentIndex = 0;
        foreach (FieldInfo field in fields)
        {
            count++;
            if (field.Name != "instance")
            {
                string name = ((TrooperDataType)field.GetValue(trooperTypeData)).name;
                trooperTypes.Add("(" + count.ToString() + ") " + (name != "" ? name : "Namenlos"));
            }
            if(((TrooperDataType)field.GetValue(trooperTypeData)) == thisTrooper.trooperDataType)
            {
                currentIndex = count-1;
            }
        }
        int choiceIndex = EditorGUILayout.Popup("Unit-type", currentIndex, trooperTypes.ToArray());
        if(choiceIndex != thisTrooper.lastChoiceIndex)
        {
            thisTrooper.trooperDataType = (TrooperDataType)trooperTypeData.GetType().GetFields()[choiceIndex].GetValue(trooperTypeData);
            thisTrooper.lastChoiceIndex = choiceIndex;
        }

        if (!Application.isPlaying)
        {
            thisTrooper.Start();
        }

    }
}