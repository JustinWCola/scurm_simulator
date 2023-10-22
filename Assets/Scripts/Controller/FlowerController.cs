using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour
{
    public enum FlowerStatusType
    {
        Idle = 0,
        Attack = 1,
        Finish = 2,
    }
    public FlowerStatusType flowerStatus;
    private bool isAnyFanHit;
    private int[] fanNum;
    private int fanCount = 0;
    private float time = 0.0f;
    public FanController[] fan;

    // Start is called before the first frame update
    private void Start()
    {
        fanNum = new int[5];
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        switch (flowerStatus)
        {
            case FlowerStatusType.Idle:
                if (time > 3.0f)
                {
                    flowerStatus = FlowerStatusType.Attack;
                    fanNum = RandomNum(0, 4);
                    fanCount = 0;
                    time = 0;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        fan[i].fanStatus = FanController.FanStatusType.Off;
                        fan[i].isFanHit = false;
                    }
                }
                break;
            case FlowerStatusType.Attack:
                fan[fanNum[fanCount]].fanStatus = FanController.FanStatusType.Ready;
                isAnyFanHit = false;
                for (int i = 0; i < 5; i++)
                    isAnyFanHit |= fan[i].isFanHit;
                if (isAnyFanHit)
                {
                    if (fan[fanNum[fanCount]].isFanHit)
                    {
                        fan[fanNum[fanCount]].fanStatus = FanController.FanStatusType.On;
                        fan[fanNum[fanCount]].isFanHit = false;
                        fanCount++;
                        if (fanCount == 5)
                        {
                            fanCount = 0;
                            flowerStatus = FlowerStatusType.Finish;
                        }
                        time = 0;
                    }
                    else
                        flowerStatus = FlowerStatusType.Idle;
                }
                if (time > 2.5f)
                {
                    flowerStatus = FlowerStatusType.Idle;
                    time = 0;
                }
                break;
            case FlowerStatusType.Finish:
                isAnyFanHit = false;
                for (int i = 0; i < 5; i++)
                    isAnyFanHit |= fan[i].isFanHit;
                if (time > 3.0f || isAnyFanHit)
                {
                    flowerStatus = FlowerStatusType.Idle;
                    time = 0;
                }
                break;
        }
    }
    private int[] RandomNum(int min, int max)
    {
        int n = max - min + 1;
        int[] sequence = new int[n];
        int[] output = new int[n];
        for (int i = 0; i < n; i++)
        {
            sequence[i] = min + i;
        }
        int end = n - 1;
        for (int i = 0; i < n; i++)
        {
            int num = Random.Range(0, end + 1);
            output[i] = sequence[num];
            sequence[num] = sequence[end];
            end--;
        }
        return output;
    }
}
