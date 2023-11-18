using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockStatus : MonoBehaviour
{
    public GimbalController gimbal;
    public Image lockImage;
    public Image aimImage;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        lockImage.enabled = gimbal.isGimbalLock;
        aimImage.enabled = gimbal.isAutoAim;
    }
}
