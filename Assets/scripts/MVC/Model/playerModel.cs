using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    //    deviceModel dm =  ;

    //    deviceModel.Instance.init();


       deviceModel.Instance.updateDeviceInfo("123321123321123$2$2$2", "deviceStatus", "500");


       deviceInfo df = deviceModel.Instance.getDeviceInfo("123321123321123$2$2$2");

       Debug.Log(df.deviceName + " " + df.deviceStatus + " " + df.imei);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
