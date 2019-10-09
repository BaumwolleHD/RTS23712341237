using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Damageable))]
public class Building : NetMonoBehaviour
{
    public bool isBuilt
    {
        get
        {
            return remainingBuildTime < 0;
        }
    }

    [ReadOnly]
    public float remainingBuildTime;

    public float buildTime = 2f;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        remainingBuildTime = buildTime;
    }

    void Update()
    {
        remainingBuildTime -= Time.deltaTime;
        if(!isBuilt)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, 1-(remainingBuildTime/buildTime));
        }
    }
}