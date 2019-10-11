﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Unit))]
public class PlayerUnit : UnitMonoBehaviour
{
    private void Start()
    {
        unit.SetMaterial(gameManager.mapSettings.teamMaterials[unit.unitOwner.playerNumber]);

        unit.WalkToBase();
    }

    // Update is called once per frame
    void Update()
    {
        if (unit.unitOwner == ownerPlayerManager && !Menu.instance.isPaused && !unit.attackTarget) WalkToMouse();
        DebugUtils.DrawPath(pathfinder.path, Color.green);
    }

    void WalkToMouse()
    {
        if (!Camera.main)
        {
            return;
        }

        int groundLayer = 1 << 8;

        RaycastHit hit;
        Utils.GetMouseTargetOfLayer(groundLayer, out hit);
        
        if (Vector3.Distance(pathfinder.destination, hit.point) > 0.1f)
        {
            unit.WalkTo(hit.point);
        }


    }
}
