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
    public void TurnOn()
    {
        lightRenderer.enabled = true;
    }
    public void TurnOff()
    {
        lightRenderer.enabled = false;
    }
}
