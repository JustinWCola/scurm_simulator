using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    public Object bullet;
    public float bulletSpeed;
    public float bulletFrequency;
    private float time = 0.0f;
    // Start is called before the first frame update
    private void Start()
    {

    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (time < bulletFrequency)
            time += Time.deltaTime;
        else if (GimbalController.inputActions.Gameplay.Shoot.IsPressed())
        {
            var bulletGameObject = Instantiate(bullet);
            bulletGameObject.GetComponent<Transform>().position = transform.position;
            bulletGameObject.GetComponent<Rigidbody>().velocity = transform.TransformDirection(0, 0, bulletSpeed);
            time = 0.0f;
        }
    }
}
