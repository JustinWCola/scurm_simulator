using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinController : MonoBehaviour
{
    public enum SpinModeType
    {
        Uniform = 0,
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
    private float a, w, b, time;
    // Start is called before the first frame update
    void Start()
    {
        SinReset();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (isSpin)
            transform.Rotate(0, 0, SinusoidalSpin());

        //暂时键盘控制
        if (Input.GetKeyDown(KeyCode.X))
            SinReset();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            spinDirection = SpinDirectionType.CounterClockWise;
            SinReset();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            spinDirection = SpinDirectionType.ClockWise;
            SinReset();
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
        return (a * Mathf.Sin(w * time) + b) * (float)spinDirection;
    }
}
