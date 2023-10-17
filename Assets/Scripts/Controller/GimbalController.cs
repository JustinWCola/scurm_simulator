using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class GimbalController : MonoBehaviour
{
    public Transform yawTransform;
    public Transform pitchTransform;
    public Rigidbody chassisRigidbody;
    public float pitchAngleMax = 60;
    public float pitchAngleMin = -30;
    public float chassisSpeedMax = 3;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        var keyboardV = Input.GetAxis("Vertical");
        var keyboardH = Input.GetAxis("Horizontal");

        yawTransform.Rotate(0, mouseX, 0);

        pitchTransform.Rotate(-mouseY, 0, 0);
        var pitchAngle = pitchTransform.localEulerAngles.x;
        if (pitchAngle > 180)
            pitchAngle -= 360;
        pitchAngle = Mathf.Clamp(pitchAngle, pitchAngleMin, pitchAngleMax);
        pitchTransform.localEulerAngles = new Vector3(pitchAngle, 180, 0);

        chassisRigidbody.velocity = yawTransform.TransformDirection(new Vector3(0, 0, -1)) * keyboardV * chassisSpeedMax +
                                    yawTransform.TransformDirection(new Vector3(-1, 0, 0)) * keyboardH * chassisSpeedMax;
    }
}
