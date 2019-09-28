using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public  void OnGUI()
    {
        PlayerManager playerManager = GetComponent<PlayerManager>();
        FieldInfo[] fields = playerManager.playerData.GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            bool firstFieldNamed;
            string nameFirstField;
            string fieldName = field.Name;
            int fieldNumber;
            int numberOfStrings;
            if (firstFieldNamed = false)
            {
                nameFirstField = fieldName;
                firstFieldNamed = true;
                fieldNumber = 1;
            }
            if (nameFirstField == fieldName)
            {
                numberOfStrings = fieldNumber;
            }
            else
            {
                fieldNumber = fieldNumber + 1;
                object fieldValue = field.GetValue(playerManager.playerData);
                string fieldString = fieldValue.ToString();
                GUI.Label(new Rect(new Vector2(20, 10*fieldNumber), new Vector2(100, 100)), fieldString);
            }
            
        }
    }
}