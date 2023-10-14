using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;

public class ROS2ImuPublisher : MonoBehaviour
{
    private ROS2UnityCore ros2Unity;
    private ROS2Node ros2Node;
    private IPublisher<sensor_msgs.msg.JointState> jointStatePub;

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
            ros2Node = ros2Unity.CreateNode("ROS2UnityImuNode");
            jointStatePub = ros2Node.CreatePublisher<sensor_msgs.msg.JointState>("/joint_states");
        }

        var timestamp = new builtin_interfaces.msg.Time();
        ros2Node.clock.UpdateROSClockTime(timestamp);

        var jointState = new sensor_msgs.msg.JointState();
        jointState.Header.Stamp = timestamp;
        jointState.Name = new string[] {
                "pitch_joint",
                "yaw_joint",
                "shoot_yaw_joint"};
        jointState.Position = new double[] {
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                transform.eulerAngles.x };
        jointStatePub.Publish(jointState);
    }

}

