using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trooper : MonoBehaviour
{
    public int currentHp;
    public int currentXp;
    
    public TrooperDataType trooperDataType;

    void Start()
    {
       currentHp = trooperDataType.maxHp;
    }
}

[CustomEditor(typeof(Trooper))]
public class TrooperEditor : Editor
{
    int choiceIndex;
    public override void OnInspectorGUI()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity", OpenSceneMode.Additive);
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
        Trooper thisTrooper = target as Trooper;

        thisTrooper.trooperDataType = (TrooperDataType)trooperTypeData.GetType().GetFields()[choiceIndex + 1].GetValue(trooperTypeData);

        if(!Application.isPlaying)
        {
            thisTrooper.currentHp = thisTrooper.trooperDataType.maxHp;
        }

        DrawDefaultInspector();
    }
}