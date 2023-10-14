using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;

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

    // Start is called before the first frame update
    void Start()
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
    void Update()
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
        img = RTImage(cam);
        img = VerticalFlipTexture(img);

        //转换为ROS2数据包
        var msg = new sensor_msgs.msg.Image();
        msg.Header.Frame_id = "camera_optical_frame";
        msg.Header.Stamp = timestamp;
        msg.Height = (uint)rt.height;
        msg.Width = (uint)rt.width;
        msg.Encoding = "rgb8";
        msg.Step = (uint)(rt.width * 3);
        msg.Data = Rgba2Rgb(img.GetRawTextureData<byte>().ToArray());

        //更新时间戳
        camInfo.Header = msg.Header;

        //发布话题
        camImagePub.Publish(msg);
        camInfoPub.Publish(camInfo);
    }
    //相机截屏为2D材质
    private Texture2D RTImage(Camera camera)
    {
        RenderTexture renderTexture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0);

        //设置渲染目标
        camera.targetTexture = renderTexture;
        //渲染相机
        camera.Render();
        //恢复渲染目标为默认值
        camera.targetTexture = null;

        RenderTexture.active = renderTexture;

        //读取图像
        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = null;
        return image;
    }
    // 垂直翻转
    public static Texture2D VerticalFlipTexture(Texture2D texture)
    {
        //得到图片的宽高
        int width = texture.width;
        int height = texture.height;

        Texture2D flipTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        for (int i = 0; i < height; i++)
        {
            flipTexture.SetPixels(0, i, width, 1, texture.GetPixels(0, height - i - 1, width, 1));
        }
        flipTexture.Apply();
        return flipTexture;
    }

    //RGBA转RGB，丢失Alpha通道
    private byte[] Rgba2Rgb(byte[] rgba)
    {
        var rgb = new byte[rgba.Length / 4 * 3];
        for (int i = 0; i < rgba.Length / 4; i++)
        {
            rgb[i * 3] = rgba[i * 4];
            rgb[i * 3 + 1] = rgba[i * 4 + 1];
            rgb[i * 3 + 2] = rgba[i * 4 + 2];
        }
        return rgb;
    }
}
