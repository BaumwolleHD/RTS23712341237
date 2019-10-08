using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HUD : NetMonoBehaviour
{
    public  void OnGUI()
    {
        PlayerManager playerManager = GetComponent<PlayerManager>();
        if(playerManager.basisBuilding)
        {
            FieldInfo[] fields = playerManager.basisBuilding.playerData.GetType().GetFields();
            int fieldNumber = 0;
            foreach (FieldInfo field in fields)
            {
                fieldNumber++;
                string fieldValueString = field.GetValue(playerManager.basisBuilding.playerData).ToString();
                string fieldNameString = field.Name;
                string fieldString = fieldNameString.ToUpper() + ": " + fieldValueString;
                GUI.Label(new Rect(new Vector2(20, 10 * fieldNumber), new Vector2(100, 100)), fieldString);
            }
        }
        string minutes = Mathf.Floor(gameManager.gameTime / 60).ToString("00");
        string seconds = (gameManager.gameTime % 60).ToString("00");
       
               GUI.Label(new Rect(new Vector2(Screen.width-100, 30), new Vector2( 100, 100)), minutes + ":" + seconds);
    }
}   