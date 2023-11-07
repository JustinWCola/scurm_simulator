using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChassisController : MonoBehaviour
{
    public enum SpinType
    {
        None = 0,
        Uniform = 1,
        Sinusoidal = 2,
        Random = 3
    }
    public enum MoveType
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        Random = 3
    }
    private float spinSpeed;
    private float moveSpeedX;
    private float moveSpeedZ;
    private float time;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time > 3)
        {
            Reset();
            time = 0.0f;
        }
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        if (transform.position.x < 4 && transform.position.z < 4)
        {
            transform.position += new Vector3(moveSpeedX * Time.deltaTime, 0, moveSpeedZ * Time.deltaTime);
        }
    }
    private void Reset()
    {
        spinSpeed = Random.Range(-50, 50);
        moveSpeedX = Random.Range(-1, 1);
        moveSpeedZ = Random.Range(-1, 1);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Reset();
    }
}
