using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OutpostSpinController : MonoBehaviour
{
    public enum SpinDirectionType
    {
        ClockWise = -1,
        CounterClockWise = 1
    }
    public SpinDirectionType spinDirection;
    public bool isSpin;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
            isSpin = !isSpin;
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            spinDirection = SpinDirectionType.CounterClockWise;
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            spinDirection = SpinDirectionType.ClockWise;
        }
    }
    private void FixedUpdate()
    {
        if (isSpin)
        {
            transform.Rotate(0, NormalSpin(), 0);
        }
    }
    private float NormalSpin()
    {
        return (float)spinDirection * 0.4f * Mathf.PI;
    }
}
