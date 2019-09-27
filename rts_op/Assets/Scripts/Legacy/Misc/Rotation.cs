using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 30f;
    
    void Update()
    {
        float rotationSpeedPerFrame = rotationSpeed*Time.deltaTime;
        transform.Rotate (0,0,rotationSpeedPerFrame);
    }
}
