using UnityEngine;
using Photon.Pun;

//TODO: Add Force on inpact

//JEDEM SPIELER EINEN CHUNK DER MAP ZUORDNEN!

    //TODO: Emancipate on awake OR make it undisableable by parent

    //Bei terrain unload alle views auf neuen Owner machen oder pausieren

public class Weapon : SyncedMonoBehaviour
{
    //TODO: Add categories

    public int rpm = 120;
    public float baseDmg = 40;
    public float recoil = 10;
    public bool fullAuto = false;
    public int spareRounds = 40;
    public float reloadTime = 3f;
    public float timeToPickup = 1f;
    public int magazineRounds = 20;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip lastMagReloadSound;
    public AudioClip outOfAmmoSound;
    public float reloadVolume;
    public bool hideCrosshair;

    public PlayerManager WeaponHolder;

    int maxMagazineRounds;

    private Cooldown shootCooldown;
    private Cooldown pickupCooldown;
    private Cooldown reloadCooldown;

    [ReadOnly] public Vector3 correctPosition = Vector3.zero;
    [ReadOnly] public Quaternion correctRotation = Quaternion.identity;


    public BoxCollider MainColldier {get; private set; }

    public Rigidbody PhysicsComponent {get;  private set;}

    private AudioSource audioSource;

    public bool CanPickup(PlayerManager PM)
    {
        if (PM)
        {
            return CanPickup() && PM.CanPickup(this);
        }
        else
        {
            return CanPickup();
        }
    }

    public bool CanReload(PlayerManager PM)
    {
        return (WeaponHolder == GetLocalPlayer() || !reloadCooldown.IsOnCooldown) && PM.CanUseGun(this);
    }

    public bool CanShoot(PlayerManager PM)
    {
        //TODO: Check other players not shooting faster then allowed and not shooting while reloading
        return (WeaponHolder==GetLocalPlayer() || (!shootCooldown.IsOnCooldown && !reloadCooldown.IsOnCooldown)) && magazineRounds > 0 && PM.CanUseGun(this);
    }

    public bool CanDrop(PlayerManager PM)
    {
        return PM.CanDropGun(this);
    }

    public bool CanPickup()
    {
        return !WeaponHolder && !pickupCooldown.IsOnCooldown;
    }

    public bool IsReloading()
    {
        return reloadCooldown.IsOnCooldown;
    }

    float CalculateShootCooldown()
    {
        float result = 1f / ((float)rpm / 60);
        if(!WeaponHolder.IsMine)
        {
            result -= (1f / PhotonNetwork.SendRate) * 2f;
        }
        return result;
    }

    public void Awake()
    {
        pickupCooldown = new Cooldown(()=>timeToPickup);
        shootCooldown = new Cooldown(CalculateShootCooldown);
        reloadCooldown = new Cooldown(() => reloadTime);

        correctPosition = transform.localPosition;
        correctRotation = transform.localRotation;

        MainColldier = GetComponent<BoxCollider>();
        PhysicsComponent = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        maxMagazineRounds = magazineRounds;
    }

    public void EnableGravity()
    {
        PhysicsComponent.useGravity = true;
        PhysicsComponent.isKinematic = false;
    }

    private void DisableGravity()
    {
        PhysicsComponent.useGravity = false;
        PhysicsComponent.isKinematic = true;
    }

    public override void StartNotOwned()
    {
        photonView.RequestOwnership();
        DisableGravity();
    }

    public override void OnBecomeResponsible()
    {
        if (WeaponHolder)
        {
            DisableGravity();
        }
        else
        {
            EnableGravity();
        }
    }
    
    public override void UpdateNotResponsible()
    {
        if (!WeaponHolder)
        {
            float lerpExponent;

            lerpExponent = pickupCooldown.IsOnCooldown ? 5f : 1f;

            if (FirstStateUpdateReceived)
            {
                transform.position = Vector3.Lerp(transform.position, correctPosition, Time.deltaTime * lerpExponent);
                transform.rotation = Quaternion.Lerp(transform.rotation, correctRotation, Time.deltaTime * lerpExponent);
            }
        }
        else
        {
            correctPosition = transform.position;
            correctRotation = transform.rotation;
        }
    }


    public void RequestShootSingle(PlayerManager PM)
    {
        if(!fullAuto)
        {
            RequestShoot(PM);
        }
    }

    public void RequestShootAuto(PlayerManager PM)
    {
        if (fullAuto)
        {
            RequestShoot(PM);
        }
    }

    void RequestShoot(PlayerManager PM)
    {

        if (IsReloading())
        {
            return;
        }
        if(!shootCooldown.IsOnCooldown && CanShoot(PM))
        {
            CheckShootHit(PM);
            PM.TryShoot();
        }
        else if(magazineRounds == 0)
        {
            PM.TryReload();
        }

        var TurnWeaponWhileShooting = GetComponentInChildren<TurnWeaponWhileShooting>();
        if (TurnWeaponWhileShooting)
        {
            TurnWeaponWhileShooting.TurnWeapon();
        }
    }
    
    public void ShootNoise()
    {
        audioSource.PlayOneShot(shootSound, baseDmg / 95f);
        var LaserScript = GetComponentInChildren<Laser>();
        if (LaserScript)
        {
            LaserScript.LaserShotAnimation();
        }
    }

    public void CheckShootHit(PlayerManager PM)
    {
        var shooterCamera = PM.playerCamera;
        if (Physics.Raycast(shooterCamera.transform.position, shooterCamera.transform.forward, out RaycastHit hit))
        {
            var healthComponent = hit.transform.gameObject.GetComponentInParent<PlayerHealth>();
            if (healthComponent)
            {
                RPC("ShootHit", RpcTarget.AllViaServer, healthComponent);
            }
        }
    }

    [PunRPC]
    public void ShootHit(int healthComponentId)
    {
        //TODO: Bullet hole if hit wall
        PlayerHealth PlayerHealth = Find<PlayerHealth>(healthComponentId);
        if (PlayerHealth)
        {
            PlayerManager PM = PlayerHealth.PM;
            if (CanShoot(WeaponHolder) && PM.CanHit(PM))
            {
                PlayerHealth.ShootHit(this);
            }
            else
            {
                Debug.LogWarning("Shoot rejected " + shootCooldown.cooldownLength());
            }
        }
    }

    [PunRPC]
    public void Shoot(int playerId)
    {

        PlayerManager PM = Find<PlayerManager>(playerId);
        magazineRounds--;        
        ShootNoise();
        Cooldown(shootCooldown);
    }

    [PunRPC]
    public void Reload(int playerId)
    {
        PlayerManager PM = Find<PlayerManager>(playerId);
        if(!CanReload(PM))
        {
            Debug.LogWarning("Reload refused");
            return;
        }
        if(spareRounds == 0)
        {
            if (!audioSource.isPlaying || audioSource.clip != outOfAmmoSound)
            {
                audioSource.PlayOneShot(outOfAmmoSound);
            }
            return;
        }

        spareRounds -= maxMagazineRounds - magazineRounds;
        magazineRounds = maxMagazineRounds;

        if(spareRounds < 0)
        {
            magazineRounds += spareRounds;
            spareRounds = 0;
        }

        if(spareRounds > 0)
        {
            audioSource.clip = reloadSound;
            audioSource.volume = reloadVolume;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = lastMagReloadSound;
            audioSource.volume = reloadVolume;
            audioSource.Play();
        }

        Cooldown(reloadCooldown, () => audioSource.Stop());
        
    }

    private void OnCollisionStay(Collision collision)
    {
        //BAD FOR PERFORMANCE
        var collidedPlayer = collision.collider.GetComponentInParent<PlayerManager>();
        if(collidedPlayer)
        {
            collidedPlayer.TryPickupWeapon(this);
        }
    }

    [PunRPC]
    public void PickupWeapon(int playerId, PhotonMessageInfo info)
    {
        PlayerManager collidedPlayer = Find<PlayerManager>(playerId);
        if (CanPickup(collidedPlayer))
        {
            PickupWeapon(collidedPlayer);
        }
    }

    public void PickupWeapon(PlayerManager collidedPlayer)
    {
        if (collidedPlayer)
        {
            //TODO: Prevent this RPC getting send 3 - 4 times at once
            transform.parent = collidedPlayer.model.WeaponSocket;
            GetComponent<BoxCollider>().enabled = false;
            DisableGravity();
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            WeaponHolder = collidedPlayer;
            collidedPlayer.weapon = this;
        }
    }

    [PunRPC]
    public void DropWeapon(int playerId)
    {
        DropWeapon(Find<PlayerManager>(playerId));
    }

    public void DropWeapon(PlayerManager PM)
    {
        if (CanDrop(PM))
        {
            transform.parent = null;
            WeaponHolder = null;
            GetComponent<BoxCollider>().enabled = true;
            EnableGravity();
            Cooldown(pickupCooldown, () => Start());
            PM.weapon = null;
        }
    }

    public override void AfterDeserializedFirstTime()
    {
        if(!WeaponHolder)
        {
            transform.position = correctPosition;
            transform.rotation = correctRotation;
        }
        else
        {
            PickupWeapon(WeaponHolder);
        }
    }

    public override void Deserialize(PhotonStream stream, PhotonMessageInfo info)
    {
        int id = (int)stream.ReceiveNext();
        WeaponHolder = Find<PlayerManager>(id);
        magazineRounds = (int)stream.ReceiveNext();
        spareRounds = (int)stream.ReceiveNext();
        if(id != 0 && !WeaponHolder)
        {
            Debug.LogWarning("It happend!");
        }
        if (!WeaponHolder)
        {
            correctPosition = (Vector3)stream.ReceiveNext();
            correctRotation = Quaternion.Euler((Vector3)stream.ReceiveNext());
        }
    }

    public override void Serialize(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(WeaponHolder ? WeaponHolder.ViewID : 0);
        stream.SendNext(magazineRounds);
        stream.SendNext(spareRounds);
        if (!WeaponHolder)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation.eulerAngles);
        }
    }

    public PlayerManager FindClosestPlayer()
    {
        float closestDistance = 0f;
        PlayerManager result = null;
        var players = FindObjectsOfType<PlayerManager>();
        foreach(PlayerManager PM in players)
        {
            float dist = Vector3.Distance(PM.transform.position, transform.position);

            if(!result || dist < closestDistance)
            {
                result = PM;
                closestDistance = dist;
            }
        }

        return result;
    }
}

//Bug: Weapon rubberbands back when people become hosts

public interface IDamageable
{
    void ApplyDamage(float damage);
}