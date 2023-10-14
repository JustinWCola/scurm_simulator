using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ArmorController : MonoBehaviour
{
    private bool isArmorHit;
    private Collider armor;
    public LightController light;
    // Start is called before the first frame update
    private void Start()
    {
        armor = GetComponent<Collider>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (isArmorHit)
        {
            light.Blink();
            isArmorHit = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == armor)
            isArmorHit = true;
    }
}
