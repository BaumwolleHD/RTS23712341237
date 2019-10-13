//https://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/

using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : PlayerMonoBehaviour
{
    bool isSelecting;
    Vector3 selectionStartPoint;
    HashSet<PlayerUnit> selectedUnits = new HashSet<PlayerUnit>();
    
    public static Texture2D WhiteTexture
    {
        get
        {
            Texture2D _whiteTexture = new Texture2D(1, 1);
            _whiteTexture.SetPixel(0, 0, Color.white);
            _whiteTexture.Apply();
            return _whiteTexture;
        }
    }

    public void CheckUnit(PlayerUnit playerUnit)
    {
        if (playerUnit.unit.unitOwner == playerManager && IsWithinSelectionBounds(playerUnit.transform.position))
        {
            selectedUnits.Add(playerUnit);
        }
    }

    public bool IsWithinSelectionBounds(Vector3 position)
    {
        if (!isSelecting) return false;

        var viewportBounds =
            Utils.GetViewportBounds(Camera.main, selectionStartPoint, Input.mousePosition);

        return viewportBounds.Contains(Camera.main.WorldToViewportPoint(position));
    }

    void HandleClickSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            selectedUnits.Clear();
            selectionStartPoint = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) isSelecting = false;
    }

    void Update()
    {
        if(playerManager.isRealPlayer) HandleClickSelection();
    }

    public void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            var rect = Utils.GetScreenRect(selectionStartPoint, Input.mousePosition);
            DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public bool IsSelected(PlayerUnit playerUnit)
    {
        return selectedUnits.Contains(playerUnit) && !isSelecting;
    }
}