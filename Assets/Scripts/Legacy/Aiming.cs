using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : PlayerMonoBehaviour
{
    Vector3 weaponSocketPosition;
    public float zoomPower;
    Vector2 sensitivity;

    public new void Start()
    {
        weaponSocketPosition = PM.model.WeaponSocket.transform.localPosition;
        sensitivity = PM.movement.sensitivity;
    }
    // Update is called once per frame
    public new void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            transform.localPosition = PM.model.head.transform.localPosition;
            PM.playerCamera.fieldOfView = PM.playerCamera.fieldOfView / zoomPower * 2;
           
            sensitivity = PM.movement.sensitivity / zoomPower;
            PM.movement.sensitivity = sensitivity;
            PM.movement.speed = PM.movement.speed / 2;
        }
        if (Input.GetMouseButtonUp(1))
        {
            transform.localPosition = weaponSocketPosition;
            PM.playerCamera.fieldOfView = PM.playerCamera.fieldOfView * zoomPower / 2;
            sensitivity = PM.movement.sensitivity * zoomPower;
            PM.movement.sensitivity = sensitivity;
            PM.movement.speed = PM.movement.speed * 2;
        }
        if (PM.weapon)
        {
            PM.weapon.hideCrosshair = Input.GetMouseButton(1);
        }
        
    }
}