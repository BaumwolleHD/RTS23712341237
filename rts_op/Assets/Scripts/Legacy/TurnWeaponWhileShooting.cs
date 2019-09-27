using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnWeaponWhileShooting : AdvancedMonoBehaviour
{
    private bool weaponTurned;
    public void TurnWeapon()
    {
        if (Input.GetMouseButton(0))
        {
            if (!weaponTurned)
            {
                transform.Rotate(0f, -90f, 0f);
                weaponTurned = true;
            };
        }
    }
    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            if (weaponTurned)
            {
                transform.Rotate(0f, 90f, 0f);
                weaponTurned = false;
            }
        }
    }
}
