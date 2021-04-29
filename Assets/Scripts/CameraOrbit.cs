using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public PlayerMovement moveScript;
    private Camera cam;

    private float yaw = 0;
    private float pitch = 0;

    public float cameraSenX = 10;
    public float cameraSenY = 10;


    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        PlayerOrbitCamera();
        if (moveScript.state != PlayerMovement.States.Dead && moveScript) transform.position = moveScript.transform.position;
        else if (moveScript.state == PlayerMovement.States.Dead && moveScript.ragDollRef) transform.position = moveScript.ragDollRef.transform.position;
    }

    private void PlayerOrbitCamera()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        yaw += mx * cameraSenX;
        pitch -= my * cameraSenY;

        pitch = Mathf.Clamp(pitch, 0, 15);
        transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .001f);
    }
}
