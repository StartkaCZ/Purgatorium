using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    int horizontalMovement;
    int verticalMovement;
    Camera mainCamera;
    Vector3 cameraMovement;
    Vector3 cameraRotation;
    Quaternion cRotation;
    public int cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        //Moves Camera
        cameraMovement = new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime);
        mainCamera.transform.position += cameraMovement;

        //Rotates Camera
        cameraRotation = new Vector3(0, Input.GetAxis("Rotate") * cameraSpeed * Time.deltaTime, 0);
        cRotation.y = cameraRotation.y;
        cRotation.x = mainCamera.transform.rotation.x;
        cRotation.z = mainCamera.transform.rotation.z;
        mainCamera.transform.rotation = cRotation;

    }
}
