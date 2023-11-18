using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlowerSpinController : MonoBehaviour
{
    public enum SpinModeType
    {
        Normal = 0,
        Sinusoidal = 1
    }
    public SpinModeType spinMode;
    public enum SpinDirectionType
    {
        ClockWise = -1,
        CounterClockWise = 1
    }
    public SpinDirectionType spinDirection;
    public bool isSpin;
    private float a, w, b;
    private float time = 0.0f;
    // Start is called before the first frame update
    private void Start()
    {
        SinReset();
    }

    // Update is called once per frame
    private void Update()
    {
        //键盘控制
        if (Keyboard.current.fKey.wasPressedThisFrame)
            isSpin = !isSpin;
        if (Keyboard.current.xKey.wasPressedThisFrame)
            spinMode = spinMode == SpinModeType.Normal ? SpinModeType.Sinusoidal : SpinModeType.Normal;
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            spinDirection = SpinDirectionType.CounterClockWise;
            SinReset();
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            spinDirection = SpinDirectionType.ClockWise;
            SinReset();
        }
    }
    private void FixedUpdate()
    {
        if (isSpin)
        {
            if (spinMode == SpinModeType.Sinusoidal)
                transform.Rotate(0, 0, SinusoidalSpin());
            else if (spinMode == SpinModeType.Normal)
                transform.Rotate(0, 0, NormalSpin());
        }
    }
    private void SinReset()
    {
        isSpin = false;
        a = Random.Range(0.780f, 1.045f);
        w = Random.Range(1.884f, 2.000f);
        b = 2.090f - a;
        isSpin = true;
    }
    private float SinusoidalSpin()
    {
        time += Time.deltaTime;
        return (a * Mathf.Sin(w * time) + b) * (float)spinDirection;
    }
    private float NormalSpin()
    {
        return (float)spinDirection * 1 / 3 * Mathf.PI;
    }
}
