using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;

public class ROS2ImuPublisher : MonoBehaviour
{
    private ROS2UnityCore ros2Unity;
    private ROS2Node ros2Node;
    private IPublisher<sensor_msgs.msg.JointState> jointStatePub;

    private builtin_interfaces.msg.Time timestamp = new builtin_interfaces.msg.Time();

    private sensor_msgs.msg.JointState jointState = new sensor_msgs.msg.JointState();

    

    // Start is called before the first frame update
    void Start()
    {
        ros2Unity = new ROS2UnityCore();
        if (ros2Unity.Ok())
        {
            ros2Node = ros2Unity.CreateNode("ROS2UnityImuNode");
            jointStatePub = ros2Node.CreatePublisher<sensor_msgs.msg.JointState>("/joint_states");
        }
        jointState.Name = new string[] {
                "pitch_joint",
                "yaw_joint",
                "shoot_yaw_joint"};
        jointState.Position = new double[] {
                0.0,
                0.0,
                0.0 };
    }

    // Update is called once per frame
    void Update()
    {

        // if (!ros2Unity.Ok())
        //     return;
        
        // var timestamp = new builtin_interfaces.msg.Time();
        ros2Node.clock.UpdateROSClockTime(timestamp);
        

        // var jointState = new sensor_msgs.msg.JointState();
        jointState.Header.Stamp = timestamp;
        jointState.Position[0] = transform.rotation.x;
        jointState.Position[1] = transform.rotation.y;
        jointState.Position[2] = transform.rotation.z;
        // jointState.Name = new string[] {
        //         "pitch_joint",
        //         "yaw_joint",
        //         "shoot_yaw_joint"};
        // jointState.Position = new double[] {
        //         transform.rotation.x,
        //         transform.rotation.y,
        //         transform.rotation.x };
        
        
        jointStatePub.Publish(jointState);

        
    }

}

