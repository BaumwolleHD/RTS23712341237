using Photon.Pun;
using UnityEngine;

public class PlayerManager : PlayerMonoBehaviour
{
    //TODO: Ersten Frame ausblenden (bzw. alles bevor Scene fertig aufgebaut)
    //TODO: Clean up RPC calls on leave
    [HideInInspector] public Camera playerCamera;
    [HideInInspector] MonoBehaviour movementScript;
    [HideInInspector] AudioListener playerMic;
    public bool isPlayed;
    
    [HideInInspector] private Weapon weaponInternal;
    [HideInInspector] public Weapon weapon
    {
        get { return weaponInternal; }
        set
        {
            if(!value && weaponInternal && weaponInternal.WeaponHolder == this)
            {
                TryDropWeapon();
            }
            if(value && !value.WeaponHolder)
            {
                TryPickupWeapon(value);
            }
            weaponInternal = value;
        }
    }

    public Texture crosshairTexture;

    [HideInInspector] public PlayerModel model;
    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerHealth healthManagement;

    public void Awake()
    {
        model = GetComponentInChildren<PlayerModel>();
        movement = GetComponent<PlayerMovement>();
        healthManagement = GetComponent<PlayerHealth>();

        playerCamera = GetComponentInChildren<Camera>();
        movementScript = GetComponentInChildren<PlayerMovement>();
        playerMic = GetComponentInChildren<AudioListener>();
        weapon = GetComponentInChildren<Weapon>();

        if(isPlayed)
        {
            TakeControl();
        }
    }

    public override void StartHasOwner()
    {
        photonView.Owner.TagObject = this;
    }

    public void TakeControl()
    {
        isPlayed = true;
        PM.playerCamera.enabled = true;
        PM.movementScript.enabled = true;
        PM.playerMic.enabled = true;
    }

    public void TryDropWeapon()
    {
        if (weapon && weapon.CanDrop(this))
        {
            weapon.RPC("DropWeapon", RpcTarget.All, photonView);
        }
    }

    public void TryPickupWeapon(Weapon weapon)
    {
        if(weapon.CanPickup(this))
        {
            weapon.RPC("PickupWeapon", RpcTarget.AllViaServer, photonView);
        }
    }

    public override void OnGUIResponsible()
    {
        if (weapon && !weapon.hideCrosshair)
        {
            float xMin = (Screen.width / 2) - (crosshairTexture.width / 2);
            float yMin = (Screen.height / 2) - (crosshairTexture.height / 2);
            GUI.DrawTexture(new Rect(xMin, yMin, crosshairTexture.width, crosshairTexture.height), crosshairTexture);
        }
    }

    public void TryKill()
    {
        if(!healthManagement.isDead)
        {
            healthManagement.RPC("Kill", RpcTarget.AllViaServer);
        }
    }

    public void TryShoot()
    {
        weapon.RPC("Shoot", RpcTarget.All, this);
    }

    public bool CanPickup(Weapon weapon)
    {
        return !this.weapon && !healthManagement.isDead;
    }

    public bool CanUseGun(Weapon weapon)
    {
        return this.weapon == weapon && !healthManagement.isDead;
    }

    public bool CanDropGun(Weapon weapon)
    {
        return this.weapon == weapon;
    }

    public bool CanHit(PlayerManager otherPlayer)
    {
        //TODO: Implement hit possible check
        return true;
    }
    
    public void TryReload()
    {
        if(weapon && weapon.CanReload(this))
        {
            weapon.RPC("Reload", RpcTarget.All, this);
        }
    }
}
