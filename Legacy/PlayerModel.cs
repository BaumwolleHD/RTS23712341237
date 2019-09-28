using Photon.Pun;
using UnityEditor;
using UnityEngine;

//JEDEM SPIELER EINEN CHUNK DER MAP ZUORDNEN!
/*
public class PlayerModel : PlayerMonoBehaviour
{
    [HideInInspector] public Rigidbody physicsComponent;

    public Transform WeaponSocket { get; private set; }

    private Vector3 correctPlayerPos;
    private Quaternion correctRot;
    [HideInInspector] public PlayerBodyPart torso;
    [HideInInspector] public PlayerHead head;

    public float jumpCooldown = 2;
    private Cooldown intJumpCooldown;

    private SpawnPoint chosenSpawnpoint;

    public void Awake()
    {
        transform.position = transform.parent.position;

        intJumpCooldown = new Cooldown(() => jumpCooldown);
        torso = transform.Find("Torso").GetComponent<PlayerBodyPart>();
        head = transform.Find("Head").GetComponent<PlayerHead>();

        WeaponSocket = transform.Find("WeaponSocket");
        physicsComponent = GetComponent<Rigidbody>();

        SpawnPoint randomSpawnPoint = SpawnPoint.activeSpawnPoints.GetRandom();
        transform.parent.transform.localPosition = randomSpawnPoint.transform.position;
        chosenSpawnpoint = randomSpawnPoint;


    }

    //BUG: Wenn man nicht host ist haben andere Spieler geschwindigkeit

    new public void Update()
    {
        base.Update();
    }

    public override void UpdateNotResponsible()
    {
        if (FirstStateUpdateReceived)
        {
            transform.parent.transform.position = Vector3.Lerp(transform.parent.transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.parent.transform.rotation = Quaternion.Lerp(transform.parent.transform.rotation, correctRot, Time.deltaTime * 5);
        }

        Debug.DrawRay(transform.parent.transform.position, Vector3.up, Color.red);

        var rigidbody = GetComponentInParent<Rigidbody>();

        if (CanJump(out Cooldown temp))
        {
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
        else
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }

    public override void UpdateResponsible()
    {
        foreach(Weapon weapon in FindObjectsOfType<Weapon>())
        {
            int closestPlayerId = weapon.FindClosestPlayer().photonView.Owner.ActorNumber;
            if(weapon.photonView.OwnerActorNr != closestPlayerId)
            {
                weapon.TransferOwnership(closestPlayerId);
            }
        }
    }

    public void GushaGusha()
    {
        if(PM.weapon)
        {
            PM.weapon.DropWeapon(PM);
        }
        PM.model.head.Detach();
        PM.model.torso.Detach();
    }

    public override void Deserialize(PhotonStream stream, PhotonMessageInfo info)
    {
        correctPlayerPos = (Vector3)stream.ReceiveNext();
        correctRot = Quaternion.AngleAxis((float)stream.ReceiveNext(), Vector3.up);
    }

    public override void AfterDeserializedFirstTime()
    {
        transform.parent.transform.position = correctPlayerPos;
        transform.parent.transform.rotation = correctRot;
    }

    public override void Serialize(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(transform.parent.transform.position);
        stream.SendNext(transform.parent.transform.rotation.eulerAngles.y);
    }

    public bool CanJump(out Cooldown cd)
    {
        Collider c = torso.GetComponents<Collider>()[0];
        var hit = Physics.Raycast(c.bounds.center-new Vector3(0,c.bounds.extents.y-0.1f,0), -Vector3.up, out RaycastHit hitInfo, 0.2f);
        cd = intJumpCooldown;
        return hit && !intJumpCooldown.IsOnCooldown;
    }    
}
*/