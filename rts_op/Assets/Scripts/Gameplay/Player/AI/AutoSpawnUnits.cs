﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class AutoSpawnUnits : MonoBehaviour
{
    float interval = 4f;
    float timer = 0f;
    public Unit unitToSpawn;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            timer = 0;
            GetComponent<PlayerManager>().SpawnUnit(unitToSpawn);
        }
    }
}