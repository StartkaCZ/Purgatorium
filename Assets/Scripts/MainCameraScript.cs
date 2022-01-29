using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public int speed;
    public int rotationSpeed;
    Vector3 direction;
    Vector3 moveDir;
    // Start is called before the first frame update
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

    }

    private void ProcessRotation()
    {
        //Rotating the camera along the Y axis
        float pitch = transform.eulerAngles.x;
        float yaw = transform.eulerAngles.y + Input.GetAxis("Rotate") * rotationSpeed * Time.deltaTime;
        float roll = 0;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
}
