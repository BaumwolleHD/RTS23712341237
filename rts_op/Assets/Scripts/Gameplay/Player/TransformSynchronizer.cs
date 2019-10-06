using Photon.Pun;
using UnityEngine;

public class TransformSynchronizer : NetMonoBehaviour, IPunObservable
{
    public SmoothingType smoothingType;
    public float rubberbandStrenght = 5f;
    public AxisSelection positionSettings = new AxisSelection(true);
    public AxisSelection rotationSettings = new AxisSelection(false);
    public AxisSelection scaleSettings = new AxisSelection(false);

    private Vector3 receivedPosition;
    private Vector3 receivedRotation;
    private Vector3 receivedScale;

    private bool firstTransformApplied;
    private bool firstTransformReceived;

    void Awake()
    {
        AddObservedComponent(this);
    }

    void Update()
    {
        if(!photonView.IsMine && firstTransformReceived)
        {
            ApplyExternalChanges();
        }
    }

    void ApplyExternalChanges()
    {

        Vector3 newPosition = receivedPosition;
        Vector3 newRotation = receivedRotation;
        Vector3 newScale = receivedScale;

        if(firstTransformApplied)
        {
            switch (smoothingType)
            {
                case SmoothingType.Rubberband:
                    newPosition = Vector3.Lerp(transform.localPosition, receivedPosition, rubberbandStrenght * Time.deltaTime);
                    newRotation = Vector3.Lerp(transform.localRotation.eulerAngles, receivedRotation, rubberbandStrenght * Time.deltaTime);
                    newScale = Vector3.Lerp(transform.localScale, receivedScale, rubberbandStrenght * Time.deltaTime);
                    break;
            }
        }

        firstTransformApplied = true;

        Vector3 tempPosition = transform.localPosition;
        Vector3 tempRotation = transform.localRotation.eulerAngles;
        Vector3 tempScale = transform.localScale;

        if (positionSettings.syncX) tempPosition.x = newPosition.x;
        if (positionSettings.syncY) tempPosition.y = newPosition.y;
        if (positionSettings.syncZ) tempPosition.z = newPosition.z;
        if (rotationSettings.syncX) tempRotation.x = newRotation.x;
        if (rotationSettings.syncY) tempRotation.y = newRotation.y;
        if (rotationSettings.syncZ) tempRotation.z = newRotation.z;
        if (scaleSettings.syncX) tempScale.x = newScale.x;
        if (scaleSettings.syncY) tempScale.y = newScale.y;
        if (scaleSettings.syncZ) tempScale.z = newScale.z;

        transform.localPosition = tempPosition;
        transform.localRotation = Quaternion.Euler(tempRotation);
        transform.localScale = tempScale;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (positionSettings.syncX) stream.SendNext(transform.localPosition.x);
            if (positionSettings.syncY) stream.SendNext(transform.localPosition.y);
            if (positionSettings.syncZ) stream.SendNext(transform.localPosition.z);
            if (rotationSettings.syncX) stream.SendNext(transform.localRotation.eulerAngles.x);
            if (rotationSettings.syncY) stream.SendNext(transform.localRotation.eulerAngles.y);
            if (rotationSettings.syncZ) stream.SendNext(transform.localRotation.eulerAngles.z);
            if (scaleSettings.syncX) stream.SendNext(transform.localScale.x);
            if (scaleSettings.syncY) stream.SendNext(transform.localScale.y);
            if (scaleSettings.syncZ) stream.SendNext(transform.localScale.z);
        }
        else
        {
            Debug.Log("Receiving");

            if (positionSettings.syncX) receivedPosition.x = (float)stream.ReceiveNext();
            if (positionSettings.syncY) receivedPosition.y = (float)stream.ReceiveNext();
            if (positionSettings.syncZ) receivedPosition.z = (float)stream.ReceiveNext();
            if (rotationSettings.syncX) receivedRotation.x = (float)stream.ReceiveNext();
            if (rotationSettings.syncY) receivedRotation.y = (float)stream.ReceiveNext();
            if (rotationSettings.syncZ) receivedRotation.z = (float)stream.ReceiveNext();
            if (scaleSettings.syncX) receivedScale.x = (float)stream.ReceiveNext();
            if (scaleSettings.syncY) receivedScale.y = (float)stream.ReceiveNext();
            if (scaleSettings.syncZ) receivedScale.z = (float)stream.ReceiveNext();

            firstTransformReceived = true;
        }
    }
}

public enum SmoothingType
{
    Rubberband, None
}

[System.Serializable]
public struct AxisSelection
{
    public bool syncX;
    public bool syncY;
    public bool syncZ;

    public AxisSelection(bool value)
    {
        syncX = value;
        syncY = value;
        syncZ = value;
    }
}