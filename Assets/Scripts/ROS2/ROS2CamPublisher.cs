using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;
using UnityEngine.Rendering;

public class ROS2CamPublisher : MonoBehaviour
{
    private ROS2UnityCore ros2Unity;
    private ROS2Node ros2Node;
    private IPublisher<sensor_msgs.msg.Image> camImagePub;
    private IPublisher<sensor_msgs.msg.CameraInfo> camInfoPub;
    private sensor_msgs.msg.CameraInfo camInfo;
    private Camera cam;
    private RenderTexture rt;
    private Texture2D img;
    private AsyncGPUReadbackRequest req;
    private byte[] imgData;

    // Start is called before the first frame update
    private void Start()
    {
        ros2Unity = new ROS2UnityCore();
        cam = GetComponent<Camera>();

        rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0);
        img = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGBA32, false);

        var fx = cam.fieldOfView * Mathf.Deg2Rad * cam.pixelWidth / (2 * Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2));
        var fy = fx;
        var cx = cam.pixelWidth / 2;
        var cy = cam.pixelHeight / 2;

        camInfo = new sensor_msgs.msg.CameraInfo();
        camInfo.K[0] = fx;
        camInfo.K[2] = cx;
        camInfo.K[4] = fy;
        camInfo.K[5] = cy;
        camInfo.K[8] = 1;
        camInfo.D = new double[5];
    }
    // Update is called once per frame
    private void Update()
    {
        if (!ros2Unity.Ok())
            return;

        if (ros2Node == null)
        {
            ros2Node = ros2Unity.CreateNode("ROS2UnityCamNode");
            camImagePub = ros2Node.CreateSensorPublisher<sensor_msgs.msg.Image>("/image_raw");
            camInfoPub = ros2Node.CreateSensorPublisher<sensor_msgs.msg.CameraInfo>("/camera_info");
        }
        //获取时间戳
        var timestamp = new builtin_interfaces.msg.Time();
        ros2Node.clock.UpdateROSClockTime(timestamp);

        //获取图像
        Graphics.Blit(cam.targetTexture, rt, new Vector2(1, -1), new Vector2(0, 1));
        AsyncGPUReadback.Request(rt, 0, TextureFormat.RGB24, OnCompleteReadback);
        req.WaitForCompletion();

        //转换为ROS2数据包
        var msg = new sensor_msgs.msg.Image();
        msg.Header.Frame_id = "camera_optical_frame";
        msg.Header.Stamp = timestamp;
        msg.Height = (uint)rt.height;
        msg.Width = (uint)rt.width;
        msg.Encoding = "rgb8";
        msg.Step = (uint)(rt.width * 3);
        msg.Data = imgData;

        //更新时间戳
        camInfo.Header = msg.Header;

        //发布话题
        camImagePub.Publish(msg);
        camInfoPub.Publish(camInfo);
    }
    void OnCompleteReadback(AsyncGPUReadbackRequest req)
    {
        if (req.hasError)
            return;
        imgData = req.GetData<byte>().ToArray();
    }
}
