using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



public class receiveDataFromNetworkController : baseManager<receiveDataFromNetworkController>
{
    public List<networkDeviceDataInfo> newWorkDeviceList = new List<networkDeviceDataInfo>();

    public List<networkDeviceDataInfo> newWorkCurrentPageDeviceList = new List<networkDeviceDataInfo>();
    public receiveDataFromNetworkController(){
        //初始化
        // reductionFunction();

        getDeviceList();
        getDeviceList22222();
        // test2();
        // reductionFunction();
    }

    public string assembleUrl(string url){
        StringBuilder stringBuilder= new StringBuilder();
        stringBuilder.Append("https://");
        stringBuilder.Append(gloabNetWorkConfig.ip);
        if(gloabNetWorkConfig.port != null){
            stringBuilder.Append($":{gloabNetWorkConfig.port}");
        }
        if(gloabNetWorkConfig.platform == "icc"){
            stringBuilder.Append("/evo-apigw/evo-fdbu/1.0.0/");
        }
        stringBuilder.Append(url);//请求地址
        Debug.Log("请求地址：" + stringBuilder.ToString());
        return stringBuilder.ToString();
    }
    // 获取所有 已经打点 的设备； 先获取主场景 然后再去 获取所有
    public void getDeviceList(){
        gloabNetWorkConfig.accessToken = "bearer 1:XWI74PgJxuEgY4O7Fp9QDohxc3gRzx7G";
        //获取设备列表
        string url = assembleUrl(gloab_URL.deviceList);
        // "/evo-apigw/evo-fdbu/1.0.0/" + gloab_URL.deviceList;//请求地址
        // string jsonParams = "{\"pageNum\": 1, \"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1}";
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0, \"isLabeled\": 1}";
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1, \"parentModelId\": \"123\"}";
        string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"isLabeled\": 1}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
            networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
            // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
            newWorkDeviceList = dataObj.data.pageData.ToList<networkDeviceDataInfo>();
            // sendToDatase(newWorkDeviceList);
            reductionFunction(newWorkDeviceList);
            // Debug.Log("newWorkDeviceList =="+newWorkDeviceList.Count);

        });

        // JsonDataManager.getInstance().LoadData<>
    }

    // positionInfo
    public void positionInfo(){
        string url =  assembleUrl(gloab_URL.positionInfo);


        // 从 deviceInfoData 转换到  networkDeviceDataInfo
        string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0,\"isLabeled\": 1}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
            // newWorkDeviceListClass dataObj = JsonUtility.FromJson<newWorkDeviceListClass>(webRequest.downloadHandler.text);
            // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
        });
    }



    // 多线程
    // 获取所有的 已经拖过 的点位
    // 存储到数据库


    public void reductionFunction(List<networkDeviceDataInfo> newWorkDeviceList){
        // 多线程 
        globalUtils.getInstance().creatThreadingPool(() =>{
            // getDeviceList();
            sendToDatase(newWorkDeviceList);
        });
    }


    public void test2(){
        globalUtils.getInstance().receiveJsonDateFormResources<networkDeviceListClass>("json/test2", (res) =>{
            // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
            newWorkDeviceList = res.data.pageData.ToList<networkDeviceDataInfo>();
            Debug.Log("newWorkDeviceList == " + newWorkDeviceList.Count);

            // 转成 deviceInfoData 然后 调用 InsertOrReplace
            List<BaseData>  UpDateListBaseData = new List<BaseData>();
            foreach (networkDeviceDataInfo item in newWorkDeviceList)
            {
                deviceInfoData deviceInfoData = new deviceInfoData();
                deviceInfoData.deviceName = item.deviceName;
                deviceInfoData.imei = item.imei;
                deviceInfoData.deviceCategory = item.deviceCategory;
                deviceInfoData.modelType = item.modelType;
                deviceInfoData.modelTypeName = item.modelTypeName;
                deviceInfoData.parentModelId = item.parentModelId;
                deviceInfoData.parentModelName = item.parentModelId;

                deviceInfoData.positionX = item.position.positionX;
                deviceInfoData.positionY = item.position.positionY;
                deviceInfoData.positionZ = item.position.positionZ;

                deviceInfoData.scaleX = item.scale.scaleX;
                deviceInfoData.scaleY = item.scale.scaleY;
                deviceInfoData.scaleZ = item.scale.scaleZ;

                deviceInfoData.rotateX = item.rotate.rotateX;
                deviceInfoData.rotateY = item.rotate.rotateY;
                deviceInfoData.rotateZ = item.rotate.rotateZ;
                UpDateListBaseData.Add(deviceInfoData);
            }


            EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(
                gloab_EventCenter_Name.DATABASE_OPERATE, 
                "InsertOrReplace", 
                globalUtils.getInstance().createMyEventArgs(UpDateListBaseData, "Select", "deviceInfoData")
            );


        });

    }



    public void getDeviceList22222(){

        //获取设备列表
        string url = assembleUrl(gloab_URL.deviceList);
        // "/evo-apigw/evo-fdbu/1.0.0/" + gloab_URL.deviceList;//请求地址
        // string jsonParams = "{\"pageNum\": 1, \"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1}";
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0, \"isLabeled\": 1}";
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1, \"parentModelId\": \"123\"}";
        string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 1,}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            // Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
            networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
            // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
            newWorkCurrentPageDeviceList = dataObj.data.pageData.ToList<networkDeviceDataInfo>();

            Debug.Log("请求成功 newWorkCurrentPageDeviceList =="+ newWorkCurrentPageDeviceList.Count());
        });
    }





    public void sendToDatase(List<networkDeviceDataInfo> newWorkDeviceList){

            // newWorkDeviceList = res.data.pageData.ToList<networkDeviceDataInfo>();
            // Debug.Log("newWorkDeviceList == " + newWorkDeviceList.Count);
            // 转成 deviceInfoData 然后 调用 InsertOrReplace
            List<BaseData>  UpDateListBaseData = new List<BaseData>();
            foreach (networkDeviceDataInfo item in newWorkDeviceList)
            {
                deviceInfoData deviceInfoData = new deviceInfoData();
                deviceInfoData.deviceName = item.deviceName;
                deviceInfoData.imei = item.imei;
                deviceInfoData.deviceCategory = item.deviceCategory;
                deviceInfoData.modelType = item.modelType;
                deviceInfoData.modelTypeName = item.modelTypeName;
                deviceInfoData.parentModelId = item.parentModelId;
                deviceInfoData.parentModelName = item.parentModelId;

                deviceInfoData.positionX = item.position.positionX;
                deviceInfoData.positionY = item.position.positionY;
                deviceInfoData.positionZ = item.position.positionZ;

                deviceInfoData.scaleX = item.scale.scaleX;
                deviceInfoData.scaleY = item.scale.scaleY;
                deviceInfoData.scaleZ = item.scale.scaleZ;

                deviceInfoData.rotateX = item.rotate.rotateX;
                deviceInfoData.rotateY = item.rotate.rotateY;
                deviceInfoData.rotateZ = item.rotate.rotateZ;
                UpDateListBaseData.Add(deviceInfoData);
            }

            EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(
                gloab_EventCenter_Name.DATABASE_OPERATE, 
                "InsertOrReplace", 
                globalUtils.getInstance().createMyEventArgs(UpDateListBaseData, "Select", "deviceInfoData")
            );

    }
}


