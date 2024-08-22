using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ModelInfo : Attribute
{
    public string Imei { get; set; }// 是否可以创建 字段

    public ModelInfo(string imei){
        Imei = imei;
    }
}


public class otherTempTest: baseManager<otherTempTest>
{



[System.Serializable]

public class deviceInfo{
        public string deviceName;
        public string deviceId;
        public long deviceCategory;
        public string? deviceStatus;
        public int? isOnline;
        // public string _imei;
        public string modelType;
        public string modelTypeName;

        // [ModelInfo("imei")]
        public string imei;
        public string parentModelId;
        public string parentModelName;

        // public Vector3 position;
        // public float? positionX, positionY, positionZ;
        // public Vector3 scale;
        // public float? scaleX, scaleY, scaleZ;
        // public Vector3 rotate;
        // public float? rotateX, rotateY, rotateZ;


        // private float? _x;
        // public float? x {
        //     get{
        //         return _x;
        //     } set{
        //         _x = value;
        //     }
        // }
        // private float? _y;
        // public float? y {
        //     get{
        //         return _y;
        //     } set{
        //         _y = value;
        //     }
        // }
        // private float? _z;
        // [ModelHelp(true, "z", "REAL", false, true)]
        // public float? z {
        //     get{
        //         return _z;
        //     } set{
        //         _z = value;
        //     }
        // }





}
    public class testJsonData{
        public deviceInfo[] data;
        public int code;
    }

    // private deviceInfo[] deviceInfoData = new deviceInfo()[10];
    private  List<deviceInfo> device_info_data_list = new List<deviceInfo>();
    public otherTempTest(){
        globalUtils.getInstance().receiveJsonDateFormResources<testJsonData>("json/test", (res) =>{
            // Debug.Log("receiveJsonDateFormResources =="+res.data);
            // deviceInfoData[] data = res.data;
            res.data.ToList().ForEach((item) => {
                // Debug.Log("receiveJsonDateFormResources item =="+item.imei);
                device_info_data_list.Add(item);
            });
        });
    }


    public deviceInfo getDeviceInfoDataFormImei(string imei){
        deviceInfo deviceInfoList = new deviceInfo();
        device_info_data_list.Where((item) => item.imei == imei).ToList().ForEach((item) => {
            Debug.Log("getDeviceInfoDataFormImei  =="+item.imei + " ==  deviceName == "+item.deviceName + " == item.deviceCategory  =="+ item.deviceCategory );
            // deviceInfoList.Add(item);
            deviceInfoList = item;
        });
        return deviceInfoList;

    }



    public networkDeviceDataInfo getnetworkDeviceDataInfo(string imei){
        Debug.Log("getnetworkDeviceDataInfo =="+ imei);
        networkDeviceDataInfo deviceInfoList = new networkDeviceDataInfo();
        // Debug.Log("getnetworkDeviceDataInfo == count=="+receiveDataFromNetworkController.getInstance().newWorkCurrentPageDeviceList.Count());
        // receiveDataFromNetworkController.getInstance().newWorkCurrentPageDeviceList.Where((item) => item.imei == imei).ToList().ForEach((item) => {
        //     Debug.Log("getnetworkDeviceDataInfo  =="+item.imei + " ==  deviceName == "+item.deviceName + " == item.deviceCategory  =="+ item.deviceCategory );
        //     // deviceInfoList.Add(item);
        //     deviceInfoList = item;
        // });

        foreach (networkDeviceDataInfo item in receiveDataFromNetworkController.getInstance().newWorkCurrentPageDeviceList)
        {
            if(item.imei == imei){
                deviceInfoList = item;
            }
            // Debug.Log("networkDeviceDataInfo =="+ item.deviceName + " == " + item.imei);
        }

        // Debug.Log("getnetworkDeviceDataInfo =deviceInfoList= "+ deviceInfoList.deviceName);


        
        return deviceInfoList;
    }


    

    //  public deviceInfoData deviceInfoToDeviceInfoData(deviceInfo deviceInfo, deviceInfoData deviceInfoData){}
    public deviceInfoData deviceInfoToDeviceInfoData( deviceInfoData deviceInfoData){
        Debug.Log("deviceInfoToDeviceInfoData =="+ deviceInfoData);
        // deviceInfoData  did = new deviceInfoData();
        networkDeviceDataInfo deviceInfo = getnetworkDeviceDataInfo(deviceInfoData.imei);
        
        deviceInfoData.deviceName = deviceInfo.deviceName;
        // deviceInfoData.deviceId = deviceInfo.deviceId;
        deviceInfoData.deviceCategory = deviceInfo.deviceCategory;
        // deviceInfoData.deviceStatus = deviceInfo.deviceStatus;
        deviceInfoData.modelType = deviceInfo.modelType;
        deviceInfoData.modelTypeName = deviceInfo.modelTypeName;
        // deviceInfoData.imei = deviceInfo.imei;
        // deviceInfoData.orgCode = deviceInfo.orgCode;
        // deviceInfoData.orgName = deviceInfo.orgName;
        // did.position = deviceInfo.position;
        return deviceInfoData;
    }
}
