using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("mainView");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void upDateInfo(object data){
        Debug.Log("upDateInfo");
        // Debug.Log(data);
        deviceModel aa = (deviceModel)data;
        Debug.Log("aa =="+ aa.getDeviceInfo("123321123321123$2$2$2").deviceStatus);
    }
}
