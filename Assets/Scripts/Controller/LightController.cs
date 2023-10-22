using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private MeshRenderer lightRenderer;
    // Start is called before the first frame update
    private void Start()
    {
        lightRenderer = GetComponent<MeshRenderer>();
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
            lightRenderer.enabled = false;
        else
            lightRenderer.enabled = true;
    }
    public void TurnOn()
    {
        lightRenderer.enabled = true;
    }
    public void TurnOff()
    {
        lightRenderer.enabled = false;
    }
}
