using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceMeasure : MonoBehaviour
{
    // Start is called before the first frame update
    private Ray ray;
    public Transform origin;
    private TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    // Update is called once per frame
    void Update()
    {
        ray = new Ray(origin.position, origin.TransformDirection(0, 0, 1));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point);
            text.text = "Distance: " + hit.distance.ToString();
        }
    }
}
