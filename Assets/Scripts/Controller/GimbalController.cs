using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GimbalController : MonoBehaviour
{
    public Transform yawTransform;
    public Transform pitchTransform;
    public Rigidbody chassisRigidbody;
    public ROS2OffsetSubscriber offsetSub;
    public float pitchAngleMax = 60;
    public float pitchAngleMin = -30;
    public float chassisSpeed = 3;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        var keyboardV = Input.GetAxis("Vertical");
        var keyboardH = Input.GetAxis("Horizontal");

        if (Input.GetButton("Fire2"))
        {
            offsetSub.enabled = true;
            yawTransform.Rotate(0, offsetSub.offset.y, 0);
            pitchTransform.Rotate(-offsetSub.offset.x, 0, 0);
        }
        else
        {
            offsetSub.enabled = false;
            yawTransform.Rotate(0, mouseX, 0);
            pitchTransform.Rotate(-mouseY, 0, 0);
        }

        var pitchAngle = pitchTransform.localEulerAngles.x;
        if (pitchAngle > 180)
            pitchAngle -= 360;
        pitchAngle = Mathf.Clamp(pitchAngle, pitchAngleMin, pitchAngleMax);
        pitchTransform.localEulerAngles = new Vector3(pitchAngle, 180, 0);

        if (keyboardV != 0 || keyboardH != 0)
            chassisRigidbody.velocity = yawTransform.TransformDirection(-keyboardH * chassisSpeed, chassisRigidbody.velocity.y, -keyboardV * chassisSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && chassisRigidbody.velocity.y < 0.01f)
            chassisRigidbody.AddForce(0, 75, 0, ForceMode.Impulse);
    }
}
