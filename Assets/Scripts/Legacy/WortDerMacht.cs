using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortDerMacht : SyncedMonoBehaviour
{
    private bool wortDerMachtCast;
    private float wortDerMachtCounter;
    private bool wortDerMacht;
    public float wortDerMachtChannelTime;
    public float wortDerMachtExplosionSpeed;
    public Vector3 orbSize;
    public Vector3 startOrbSize;
    private float playerSpeed;
    public float castSlow;
    private float timer;
    private float castBiginnTime;
    public float reichweite;
    private bool ausgelutschterStab = false;
    Weapon weaponScript;
    public float cooldown;
    private float timeOfCooldown;
    private float beginnCooldown;
    public float rotationsStaerke;

    public Weapon WeaponScript { get => WeaponScript1; set => WeaponScript1 = value; }
    public Weapon WeaponScript1 { get => weaponScript; set => weaponScript = value; }

    new void Start()
    {
        orbSize = transform.localScale;
        startOrbSize = orbSize;
        StartCoroutine(TimeOfMyLifeRip());
        timer = 0;
        WeaponScript = transform.parent.GetComponent<Weapon>();
        timeOfCooldown = cooldown;
        beginnCooldown = -cooldown;
        cooldown = 0;
        base.Start();
    }

    new void Update()
    {
        transform.Rotate(0f, rotationsStaerke, 0f); // Idee, bei zufälliger rotationsStaerke rotieren, sieht aber so noch blöd aus: UnityEngine.Random.Range(-rotationsStaerke, rotationsStaerke)
        cooldown = beginnCooldown+timeOfCooldown-timer;
        if (cooldown > 0)
        {
            ausgelutschterStab = true;
        }
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            castBiginnTime = timer;
            playerSpeed = WeaponScript.WeaponHolder.movement.speed;
            wortDerMachtCast = true;
        }
        if (orbSize.x >= reichweite)
        {
            wortDerMacht = false;
            Invoke(ResetWortDerMacht, 0.05f);
        }
        if (wortDerMachtCast)
        {
            WeaponScript.WeaponHolder.movement.speed = playerSpeed / castSlow;
            if (Input.GetMouseButtonUp(0))
            {
                wortDerMachtCast = false;
                wortDerMachtCounter = 0;
            }
            wortDerMachtCounter = timer - castBiginnTime;
            if (ausgelutschterStab == false)
            {
                transform.localScale = new Vector3(orbSize.x * 36f, orbSize.y * 36f, orbSize.z * 36f);
            }
            if (wortDerMachtCounter >= wortDerMachtChannelTime)
            {
                if (ausgelutschterStab == false)
                {
                    wortDerMacht = true;
                }

            }
        }
        if (wortDerMacht)
        {
            transform.localScale = new Vector3(wortDerMachtExplosionSpeed * orbSize.x, wortDerMachtExplosionSpeed * orbSize.y, wortDerMachtExplosionSpeed * orbSize.z);
            orbSize = transform.localScale;
            WeaponScript.WeaponHolder.movement.speed = playerSpeed / castSlow;
            KillPlayersInRange();
            cooldown = timeOfCooldown;
            beginnCooldown = timer;
        }
        if (Input.GetMouseButtonUp(0))
        {
            wortDerMacht = false;
            orbSize = startOrbSize;
            transform.localScale = orbSize;
            WeaponScript.WeaponHolder.movement.speed = playerSpeed;
            wortDerMachtCounter = 0;
            ausgelutschterStab = false;
        }
    }
    void ResetWortDerMacht()
    {
        wortDerMacht = false;
        orbSize = startOrbSize;
        transform.localScale = orbSize;
        WeaponScript.WeaponHolder.movement.speed = playerSpeed;
        wortDerMachtCounter = 0;
        ausgelutschterStab = true;
    }
    IEnumerator TimeOfMyLifeRip()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        timer = timer + 0.1f;
        Debug.Log(wortDerMachtCounter);
        StartCoroutine(TimeOfMyLifeRip());
    }
    public void KillPlayersInRange()
    {
        var players = FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager PM in players)
        {
            if (PM != WeaponScript.WeaponHolder)
            {
                float dist = Vector3.Distance(PM.transform.position, transform.position);
                if (dist <= reichweite)
                {
                    WeaponScript.RPC("ShootHit", RpcTarget.AllViaServer, PM.healthManagement);
                }
            }
        }
    }
}