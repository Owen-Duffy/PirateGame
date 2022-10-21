using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_camera : MonoBehaviour
{
    //public GameObject camera;

    public float mouseSpeed = 2.0f;
    public float walkSpeed = 2.0f;
    private float rPitch = 0.0f;
    private float rYaw = 0.0f;
    public Transform cameraPosition;
    public Transform orientation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        rPitch -= mouseSpeed * Input.GetAxis("Mouse Y");
        rYaw += mouseSpeed * Input.GetAxis("Mouse X");

        if (rPitch > 90f)
        {
            rPitch = 90f;
        }
        if (rPitch < -90f)
        {
            rPitch = -90f;
        }

        transform.eulerAngles = new Vector3(rPitch, rYaw, 0.0f);
        transform.position = cameraPosition.position;
        orientation.rotation = Quaternion.Euler(0, rYaw, 0);
    }
}
