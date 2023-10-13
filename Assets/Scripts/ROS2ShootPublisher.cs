using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;

public class ROS2ShootPublisher : MonoBehaviour
{
    private ROS2UnityCore ros2Unity;
    private ROS2Node ros2Node;
    private IPublisher<std_msgs.msg.Char> enemyColorPub;
    private IPublisher<std_msgs.msg.Float64> bulletSpeedPub;
    public char enemyColor;
    public char bulletSpeed;
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
            ros2Node = ros2Unity.CreateNode("ROS2UnityShootNode");
            enemyColorPub = ros2Node.CreatePublisher<std_msgs.msg.Char>("/color");
            bulletSpeedPub = ros2Node.CreatePublisher<std_msgs.msg.Float64>("/bullet_speed");
        }

        var timestamp = new builtin_interfaces.msg.Time();
        ros2Node.clock.UpdateROSClockTime(timestamp);

        var enemyColorMsg = new std_msgs.msg.Char();
        enemyColorMsg.Data = (byte)enemyColor;
        enemyColorPub.Publish(enemyColorMsg);


        var bulletSpeedMsg = new std_msgs.msg.Float64();
        bulletSpeedMsg.Data = (double)bulletSpeed;
        bulletSpeedPub.Publish(bulletSpeedMsg);
    }
}
