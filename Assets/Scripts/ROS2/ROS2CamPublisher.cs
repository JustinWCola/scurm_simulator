using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;
using UnityEngine.Rendering;
using JetBrains.Annotations;

public class ROS2CamPublisher : MonoBehaviour
{
    private ROS2UnityCore ros2Unity;
    private ROS2Node ros2Node;
    private IPublisher<sensor_msgs.msg.Image> camImagePub;
    private IPublisher<sensor_msgs.msg.CameraInfo> camInfoPub;
    private sensor_msgs.msg.CameraInfo camInfo;
    private sensor_msgs.msg.Image img;
    private Camera cam;
    private RenderTexture rt;
    private AsyncGPUReadbackRequest req;
    private byte[] imgData;

    // Start is called before the first frame update
    private void Start()
    {
        ros2Unity = new ROS2UnityCore();
        cam = GetComponent<Camera>();

        rt = new RenderTexture(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        cam.targetTexture = rt;

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
        camInfo.Width = (uint)cam.pixelWidth;
        camInfo.Height = (uint)cam.pixelHeight;
        camInfo.D = new double[5];

        img = new sensor_msgs.msg.Image();
        imgData = new byte[] { };
        if (ros2Unity.Ok())
        {
            ros2Node = ros2Unity.CreateNode("ROS2UnityCamNode");
            var qos = new QualityOfServiceProfile(QosPresetProfile.DEFAULT);
            camImagePub = ros2Node.CreatePublisher<sensor_msgs.msg.Image>("/image_raw", qos);
            // camImagePub = ros2Node.CreateSensorPublisher<sensor_msgs.msg.Image>("/image_raw");
            camInfoPub = ros2Node.CreateSensorPublisher<sensor_msgs.msg.CameraInfo>("/camera_info");
        }
    }
    // Update is called once per frame
    private void Update()
    {
        //获取时间戳
        UnityEngine.Profiling.Profiler.BeginSample("IMU Sample");
        
        var timestamp = new builtin_interfaces.msg.Time();
        ros2Node.clock.UpdateROSClockTime(timestamp);

        //获取图像
        RenderTexture.active = rt;
        cam.Render();
        // Graphics.Blit(cam.targetTexture, rt, new Vector2(1, -1), new Vector2(0, 1));
        AsyncGPUReadback.Request(rt, 0, TextureFormat.RGB24, OnCompleteReadback);
        req.WaitForCompletion();

        //转换为ROS2数据包
        img.Header.Frame_id = "camera_link";
        img.Header.Stamp = timestamp;
        img.Height = (uint)rt.height;
        img.Width = (uint)rt.width;
        img.Encoding = "rgb8";
        img.Step = (uint)(rt.width * 3);
        img.Data = imgData;

        //更新时间戳
        camInfo.Header = img.Header;

        //发布话题
        camImagePub.Publish(img);
        camInfoPub.Publish(camInfo);

        // Debug.Log("Publishing image");

        UnityEngine.Profiling.Profiler.EndSample();
    }
    void OnCompleteReadback(AsyncGPUReadbackRequest req)
    {
        if (req.hasError)
        {
            Debug.Log("GPU readback error detected.");
            return;
        }
        // 获取读回的数据
        byte[] rawData = req.GetData<byte>().ToArray();

        // 翻转图像
        imgData = FlipImageVertically(rawData, rt.width, rt.height);
    }
    byte[] FlipImageVertically(byte[] rawData, int width, int height)
    {
        int rowSize = width * 3; // 每行的字节数 (RGB 每个像素3字节)
        byte[] flippedData = new byte[rawData.Length];

        for (int y = 0; y < height; y++)
        {
            int srcIndex = y * rowSize;
            int dstIndex = (height - 1 - y) * rowSize;
            System.Array.Copy(rawData, srcIndex, flippedData, dstIndex, rowSize);
        }

        return flippedData;
    }
}
