using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private MeshRenderer light;
    // Start is called before the first frame update
    private void Start()
    {
        light = GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    private void Update()
    {

    }
    public void Blink()
    {
        float blinkTime = 0.0f;
        blinkTime += Time.deltaTime;
        if (blinkTime % 1 > 0.2f)
            light.enabled = false;
        else
            light.enabled = true;
    }
    public void TurnOn()
    {
        light.enabled = true;
    }
    public void TurnOff()
    {
        light.enabled = false;
    }
}
