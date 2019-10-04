using Unity.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed = 10;
    public float zoomSpeed = 30f;

    void Update()
    {
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");           

        float R = ScrollWheelChange * zoomSpeed;  //This little peece of code is written by JelleWho https://github.com/jellewie
        float PosX = transform.eulerAngles.x + 90;              //Get up and down
        float PosY = -1 * (transform.eulerAngles.y - 90);       //Get left to right
        PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
        PosY = PosY / 180 * Mathf.PI;                                       //^
        float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
        float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
        float Y = R * Mathf.Cos(PosX);
        transform.localPosition = transform.localPosition + new Vector3(X, Y, Z);//Move the main camera

        if (Input.mousePosition.x <= 10)
        {
            transform.localPosition += Quaternion.AngleAxis(-130, Vector3.up) *  new Vector3(-cameraSpeed * GetCameraHeight() * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.x >= Screen.width - 10)
        {
            transform.localPosition += Quaternion.AngleAxis(-130, Vector3.up) * new Vector3(cameraSpeed * GetCameraHeight() * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.y <= 10)
        {
            transform.localPosition += Quaternion.AngleAxis(-130, Vector3.up) * new Vector3(0, 0, -cameraSpeed * GetCameraHeight() * Time.deltaTime);
        }
        if (Input.mousePosition.y >= Screen.height - 10)
        { 
            transform.localPosition += Quaternion.AngleAxis(-130, Vector3.up)* new Vector3(0, 0, cameraSpeed * GetCameraHeight() * Time.deltaTime); 
        }
        if (Input.GetMouseButtonDown(2))
        {
            GetComponent<Transform>().position = new Vector3(60, 70, 60);
        }
    }

    float GetCameraHeight()
    {
        return transform.localPosition.y;
    }
}

