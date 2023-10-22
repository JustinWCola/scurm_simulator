using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;

public class ROS2OffsetSubscriber : MonoBehaviour
{
    private ROS2UnityCore ros2Unity;
    private ROS2Node ros2Node;
    private ISubscription<sensor_msgs.msg.JointState> offsetSub;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        ros2Unity = new ROS2UnityCore();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ros2Unity.Ok())
            return;

        if (ros2Node == null)
        {
            ros2Node = ros2Unity.CreateNode("ROS2UnityOffsetNode");
            offsetSub = ros2Node.CreateSubscription<sensor_msgs.msg.JointState>("/offset", msg => OffsetCallback(msg));
        }
    }
    private void OffsetCallback(sensor_msgs.msg.JointState msg)
    {
        offset = new Vector3((float)msg.Position[0], (float)msg.Position[1], 0);
    }

}
