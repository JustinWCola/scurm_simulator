# 24FullProcessSimulator

Justin.W.Cola

## 操作方式

- 底盘移动：WASD空格
- 云台移动：鼠标移动
- 射击：鼠标左键
- 自瞄：鼠标右键
- 能量机关旋转方向调整：QE

## ROS2节点

- 相机节点：/image_raw
- IMU节点：/joint_states
- 偏移量节点：/offset

后两者的数据类型均为sensor_msgs.msg.JointState
