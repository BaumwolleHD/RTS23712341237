using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Trooper : MonoBehaviour
{
    public int currentHp;
    public int currentXp;

    [HideInInspector]
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
        DrawDefaultInspector();
        FieldInfo[] fields = typeof(TrooperTypeData).GetFields();
        List<string> trooperTypes = new List<string>();
        foreach (FieldInfo field in fields)
        {
            if (field.Name != "instance")
            {
                trooperTypes.Add(field.Name);
            }
        }
        choiceIndex = EditorGUILayout.Popup("Unit-type",choiceIndex,trooperTypes.ToArray());
        Trooper thisTrooper = target as Trooper;
        if(TrooperTypeData.instance)
        {
            thisTrooper.trooperDataType = (TrooperDataType)TrooperTypeData.instance.GetType().GetFields()[choiceIndex+1].GetValue(TrooperTypeData.instance);
        }
    }
}