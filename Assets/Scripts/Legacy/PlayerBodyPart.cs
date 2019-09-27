using Photon.Pun;
using UnityEngine;

public class PlayerBodyPart : PlayerMonoBehaviour
{
    [ReadOnly] public bool detached = false;

    [HideInInspector] public Vector3 realPosition;
    [HideInInspector] public Quaternion realRotation;

    public override void OnBecomeResponsible()
    {
        if(detached)
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    public void Detach()
    {
        PM.Start(); //Since RPCs replayed from buffer get called before Start this is needed to reference the PlayerManager for later use

        realPosition = transform.position;
        realRotation = transform.rotation;
        detached = true;
        transform.parent = null;
        if (gameObject.GetComponent<Collider>())
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }
        if (IsMine)
        {
            TransferOwnership(0);
        }

        if (IsMasterClient)
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    public override void UpdateNotResponsible()
    {
        if (detached && FirstStateUpdateReceived)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, realPosition, Time.deltaTime * 5);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, realRotation, Time.deltaTime * 5);
        }
    }

    public override void Deserialize(PhotonStream stream, PhotonMessageInfo info)
    {
        detached = (bool)stream.ReceiveNext();
        if (detached)
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = Quaternion.Euler((Vector3)stream.ReceiveNext());
        }
    }

    public override void AfterDeserializedFirstTime()
    {
        if(detached)
        {
            Detach();
            transform.position = realPosition;
            transform.rotation = realRotation;
        }
    }

    public override void Serialize(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(detached);
        if (detached)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation.eulerAngles);
        }
    }
}
