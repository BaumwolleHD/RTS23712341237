using Photon.Pun;
using UnityEngine;

public class PlayerHead : PlayerBodyPart
{
    [HideInInspector] public float tilt;

    public override void UpdateNotResponsible()
    {
        base.UpdateNotResponsible();

        if (FirstStateUpdateReceived && !detached)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(tilt, 0, 0), Time.deltaTime * 5);
        }
    }

    public override void Deserialize(PhotonStream stream, PhotonMessageInfo info)
    {
        base.Deserialize(stream, info);
        if (!detached)
        {
            tilt = (float)stream.ReceiveNext();
        }
    }

    public override void AfterDeserializedFirstTime()
    {
        base.AfterDeserializedFirstTime();
        if(!detached)
        {
            transform.localRotation = Quaternion.Euler(tilt, 0, 0);
        }
    }

    public override void Serialize(PhotonStream stream, PhotonMessageInfo info)
    {
        base.Serialize(stream, info);
        if (!detached)
        {
            stream.SendNext(transform.localRotation.eulerAngles.x);
        }
    }
}
