using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public  void OnGUI()
    {
        PlayerManager playerManager = GetComponentInParent<PlayerManager>();
        FieldInfo[] fields = playerManager.playerData.GetType().GetFields();
        int fieldNumber = 0;
        foreach (FieldInfo field in fields)
        {
            fieldNumber++;
            string fieldValueString = field.GetValue(playerManager.playerData).ToString();
            string fieldNameString = field.Name;
            string fieldString =  fieldNameString.ToUpper() + ": " + fieldValueString;
            GUI.Label(new Rect(new Vector2(20, 10*fieldNumber), new Vector2(100, 100)), fieldString);
        }
    }
}   