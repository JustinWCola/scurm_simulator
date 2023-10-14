using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FanController : MonoBehaviour
{
    public enum FanStatusType
    {
        Off = 0,
        Ready = 1,
        On = 2
    }
    public FanStatusType fanStatus;
    public bool isFanHit;
    private bool[] isRingHit;
    private int ringNum;
    public Collider[] ring;
    public LightController[] fanLight;
    //0 准星 1 外环 2 中间 3流水
    public LightController[] ringLight;
    // Start is called before the first frame update

    private void Start()
    {
        isRingHit = new bool[10];
        ringNum = 5;
    }
    // Update is called once per frame
    private void Update()
    {
        switch (fanStatus)
        {
            case FanStatusType.Off:
                for (int i = 0; i < 10; i++)
                    ringLight[i].TurnOff();
                for (int i = 0; i < 4; i++)
                    fanLight[i].TurnOff();
                isFanHit = false;//添加碰撞检测之后可以删掉
                break;
            case FanStatusType.Ready:
                fanLight[0].TurnOn();
                fanLight[1].TurnOn();
                fanLight[3].TurnOn();
                break;
            case FanStatusType.On:
                if (isRingHit[9])
                    for (int i = 0; i < 10; i += 2)
                        ringLight[i].TurnOn();
                else
                    ringLight[ringNum].TurnOn();
                fanLight[0].TurnOff();
                fanLight[2].TurnOn();
                fanLight[3].TurnOff();
                isFanHit = false;
                break;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        // for (int i = 0; i < 10; i++)
        // {
        //     if (collision.collider == ring[i])
        //     {
        //         isRingHit[i] = true;
        //         ringNum = i;
        //     }
        //     isFanHit = true;
        // }
    }
}
