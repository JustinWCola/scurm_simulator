using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour
{
    public enum FanStatusType
    {
        Off = 0,
        Ready = 1,
        On = 2,
    }
    public FanStatusType fanStatus;
    public bool isFanHit;
    public int ringNum = 1;
    public LightController[] fanLight;
    //0 准星 1 外环 2 中间 3流水
    public LightController[] ringLight;
    // Start is called before the first frame update

    private void Start()
    {

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
                break;
            case FanStatusType.Ready:
                fanLight[0].TurnOn();
                fanLight[1].TurnOn();
                fanLight[3].TurnOn();
                break;
            case FanStatusType.On:
                if (ringNum == 10)
                    for (int i = 0; i < 10; i += 2)
                        ringLight[i].TurnOn();
                else
                    ringLight[ringNum - 1].TurnOn();
                fanLight[0].TurnOff();
                fanLight[2].TurnOn();
                fanLight[3].TurnOff();
                break;
        }
    }
}
