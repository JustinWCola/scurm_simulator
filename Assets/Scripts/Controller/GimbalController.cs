using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GimbalController : MonoBehaviour
{
    public static PlayerInputActions inputActions;
    public static Vector2 sensitivity = new Vector2(0.1f, 0.1f);
    public Transform yawTransform;
    public Transform pitchTransform;
    public Rigidbody chassisRigidbody;
    public ROS2OffsetSubscriber offsetSub;
    public float pitchAngleMax = 60;
    public float pitchAngleMin = -30;
    public float chassisSpeed = 3;
    public bool isGimbalLock = false;
    public bool isAutoAim = false;
    private Vector2 moveControl, cameraControl;
    // Start is called before the first frame update
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.Gameplay.Enable();
    }
    private void OnDisable()
    {
        inputActions.Gameplay.Disable();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        moveControl = inputActions.Gameplay.Move.ReadValue<Vector2>();
        cameraControl = inputActions.Gameplay.Camera.ReadValue<Vector2>() * sensitivity;

        if (inputActions.Gameplay.Lock.WasPressedThisFrame())
            isGimbalLock = !isGimbalLock;
        if (inputActions.Gameplay.Aim.WasPressedThisFrame())
            isAutoAim = !isAutoAim;
        // if (Input.GetKeyDown(KeyCode.Space))
        //     chassisRigidbody.AddForce(0, 75, 0, ForceMode.Impulse);
    }
    private void FixedUpdate()
    {
        if (isAutoAim)
        {
            if (isGimbalLock)
                Cursor.lockState = CursorLockMode.Confined;
            offsetSub.enabled = true;
            yawTransform.Rotate(0, offsetSub.offset.y, 0);
            pitchTransform.Rotate(-offsetSub.offset.x, 0, 0);
        }
        else
        {
            if (isGimbalLock)
                Cursor.lockState = CursorLockMode.Confined;
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                offsetSub.enabled = false;
                yawTransform.Rotate(0, cameraControl.x, 0);
                pitchTransform.Rotate(-cameraControl.y, 0, 0);
            }
        }

        var pitchAngle = pitchTransform.localEulerAngles.x;
        if (pitchAngle > 180)
            pitchAngle -= 360;
        pitchAngle = Mathf.Clamp(pitchAngle, pitchAngleMin, pitchAngleMax);
        pitchTransform.localEulerAngles = new Vector3(pitchAngle, 180, 0);

        if (moveControl != Vector2.zero)
            chassisRigidbody.velocity = yawTransform.TransformDirection(-moveControl.x * chassisSpeed, chassisRigidbody.velocity.y, -moveControl.y * chassisSpeed);
    }
}
