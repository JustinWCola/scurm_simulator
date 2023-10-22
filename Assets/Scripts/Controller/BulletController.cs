using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime = 1.0f;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0.0f)
            Destroy(gameObject);
    }
}
