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
    public float spinSpeed;
    public float moveSpeed;
    private float moveDirectionX;
    private float moveDirectionZ;
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
        transform.position += new Vector3(moveSpeed * moveDirectionX * Time.deltaTime, 0, moveSpeed * moveDirectionX * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 3 && Mathf.Abs(transform.position.z) > 3)
            FlipDirection();
    }
    private void Reset()
    {
        spinSpeed = Random.Range(-50, 50);
        RandomDirection();
    }
    private void RandomDirection()
    {
        Vector2 direction = Random.insideUnitCircle;
        moveDirectionX = direction.x;
        moveDirectionZ = direction.y;
    }
    private void FlipDirection()
    {
        moveDirectionX = -moveDirectionX;
        moveDirectionZ = -moveDirectionZ;
    }
}
