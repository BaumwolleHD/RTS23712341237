using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float cameraSpeed = 10;
    private float zoomValue = 1;
    public float startCameraSpeed = 10;
    void Update()
    {
        GetComponent<Camera>().fieldOfView -= startCameraSpeed * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * 40;
        zoomValue = GetComponent<Camera>().fieldOfView / 60;
        cameraSpeed = startCameraSpeed * zoomValue * 2;
        if (Input.mousePosition.x <= 1)
        {
            transform.localPosition += Quaternion.AngleAxis(-130, Vector3.up) *  new Vector3(-cameraSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.x >= Screen.width - 1)
        {
            transform.localPosition += Quaternion.AngleAxis(-130, Vector3.up) * new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.y <= 1)
        {
            transform.localPosition += Quaternion.AngleAxis(-130, Vector3.up) * new Vector3(0, 0, -cameraSpeed * Time.deltaTime);
        }
        if (Input.mousePosition.y >= Screen.height - 1)
        { 
            transform.localPosition += Quaternion.AngleAxis(-130, Vector3.up)* new Vector3(0, 0, cameraSpeed * Time.deltaTime); 
        }
        if (Input.GetMouseButtonDown(2))
        {
            GetComponent<Camera>().fieldOfView = 60;
            GetComponent<Transform>().position = new Vector3(60, 70, 60);
       }
    }
}

