using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainController : MonoBehaviour
{

    mainView mainview;
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("mainController");
        mainview = this.GetComponent<mainView>();
        // Debug.Log("mainController"+ mainview);
        // Debug.Log("deviceModel.Instance =="+ deviceModel.Instance.getDeviceInfo("123321123321123$2$2$2").deviceName);
        mainview.upDateInfo(deviceModel.Instance);

        deviceModel.Instance.AddEventLister(updateDataInfo);


        this.Invoke("changeInfo", 5f);

    }

    void changeInfo(){
        deviceModel.Instance.updateDeviceInfo("123321123321123$2$2$2", "deviceStatus", "500");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateDataInfo(deviceModel data){
        // Debug.Log("mainController"+ deviceModel);
        if(mainview != null){
            mainview.upDateInfo(deviceModel.Instance);
        }
    }
}
