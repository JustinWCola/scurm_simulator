# 24FullProcessSimulator

Justin.W.Cola

## 操作方式

- 底盘移动：WASD
- 云台移动：鼠标移动
- 射击：鼠标左键
- 锁定：鼠标中键
- 自瞄：鼠标右键
- 切换能量机关和前哨站：F
- 能量机关旋转方向调整：QE
- 能量机关切换模式：X

## ROS2节点

- 相机节点：/image_raw
- 相机信息：/camera_info
- IMU节点：/joint_states
- 偏移量节点：/offset

后两者的数据类型均为sensor_msgs.msg.JointState
