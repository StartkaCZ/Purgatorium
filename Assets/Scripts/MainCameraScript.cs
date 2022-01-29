using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public int speed;
    public int rotationSpeed;
    public int zoomSpeed;
    const float MAXDISTANCE = 50;
    const float MINDISTANCE = 20;
    Vector3 direction;
    Vector3 moveDir;
    Transform objectHit;
    // Update is called once per frame
    void Update()
    {
        //Moves Camera
        direction = Quaternion.AngleAxis(-45, Vector3.up) * direction;

        direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (direction.magnitude >= 0.1f)
        {
            moveDir = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * direction;
            transform.position += (moveDir.normalized * speed * Time.deltaTime);
        }

        //Rotates Camera
        ProcessRotation();

        //Zooming in and out with the Camera
        processZooming();

    }

    private void ProcessRotation()
    {
        //Rotating the camera along the Y axis
        float pitch = transform.eulerAngles.x;
        float yaw = transform.eulerAngles.y + Input.GetAxis("Rotate") * rotationSpeed * Time.deltaTime;
        float roll = 0;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void processZooming()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            objectHit = hit.transform;

            float distance = Vector3.Distance(objectHit.position, Camera.main.transform.position);

            float zoomDistance = zoomSpeed * Input.mouseScrollDelta.y * Time.deltaTime;

            //Zooming in and out
            if (zoomDistance < 0 && distance < MAXDISTANCE)
            {
                transform.Translate(ray.direction * zoomDistance, Space.World);
            }
            else if (zoomDistance > 0 && distance > MINDISTANCE)
            {
                transform.Translate(ray.direction * zoomDistance, Space.World);
            }

        }

    }
}
