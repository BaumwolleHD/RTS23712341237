using UnityEngine;
using UnityEditor;
using Photon.Pun;
using ExitGames.Client.Photon;

[RequireComponent(typeof(PhotonView))]

public class Damageable : UnitMonoBehaviour
{
    public float currentHp;
    public float maxHp = 1000f;
    private Unit lastDamageSource;
    
    public Vector3 initialScale;

    public bool isDead { get { return currentHp < 0f; } }

    void Start()
    {
        currentHp = maxHp;
        initialScale = transform.localScale;
        Debug.Log(initialScale);
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(initialScale * 0.1f, initialScale, currentHp/maxHp);

        if (isDead)
        {
            lastDamageSource.Killed(this);
            Destroy();
            if (GetComponent<NeutralUnit>())
            {
                GetComponentInParent<Camp>().CampNeutralUnitDies();
            }
        }
    }

    public void ApplyDamage(Unit source, float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Min(currentHp, maxHp);
        if(source) lastDamageSource = source;
    }

    public void Heal(float amount)
    {
        currentHp += amount;
    }

    public static readonly byte[] memVector2 = new byte[4];

    public static short Serialize(StreamBuffer outStream, object customobject)
    {
        Damageable vo = (Damageable)customobject;
        lock (memVector2)
        {
            byte[] bytes = memVector2;
            int index = 0;
            Protocol.Serialize(vo.photonView.ViewID, bytes, ref index);
            outStream.Write(bytes, 0, 4);
        }

        return 4;
    }

    public static object Deserialize(StreamBuffer inStream, short length)
    {
        lock (memVector2)
        {
            inStream.Read(memVector2, 0, 4);
            int index = 0;
            int viewId;
            Protocol.Deserialize(out viewId, memVector2, ref index);
            PhotonView view = PhotonView.Find(viewId);
            return view ? view.GetComponent<Damageable>() : null;
        }
    }
}