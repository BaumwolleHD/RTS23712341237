using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public Axis rotaionAxis;
    
    void Update()
    {
        float rotationSpeedPerFrame = rotationSpeed*Time.deltaTime;
        switch (rotaionAxis)
        {
            case Axis.x:
                transform.Rotate(rotationSpeedPerFrame,0,0);
                break;
            case Axis.y:
                transform.Rotate(0,rotationSpeedPerFrame, 0);
                break;
            case Axis.z:
                transform.Rotate(0,0,rotationSpeedPerFrame);
                break;
        }
    }
}

public enum Axis
{
    x,y,z
}