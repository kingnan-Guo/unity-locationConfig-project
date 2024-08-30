using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;



public class receiveDataFromNetworkController1 : baseManager<receiveDataFromNetworkController>
{
    public List<networkDeviceDataInfo> newWorkDeviceList = new List<networkDeviceDataInfo>();

    public List<networkDeviceDataInfo> newWorkCurrentPageDeviceList = new List<networkDeviceDataInfo>();
    public receiveDataFromNetworkController1(){

        //初始化
        // reductionFunction();
        // getDeviceList();
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
        Debug.Log("getDeviceList 获取数据 =="+ System.DateTime.Now);
        // gloabNetWorkConfig.accessToken = "bearer 1:FQxbGPzrf4fImkbgK3edTNbtdk1mpR6C";
        //获取设备列表
        string url = assembleUrl(gloab_URL.deviceList);
        // "/evo-apigw/evo-fdbu/1.0.0/" + gloab_URL.deviceList;//请求地址
        // string jsonParams = "{\"pageNum\": 1, \"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1}";
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0, \"isLabeled\": 1}";
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1, \"parentModelId\": \"123\"}";
        string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"isLabeled\": 1, \"deviceCategory\": 0}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
            networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
            Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
            // if(dataObj.data.totalCount > 0){
            //     // reductionFunction(newWorkDeviceList);
            // }

            newWorkDeviceList = dataObj.data.pageData.ToList<networkDeviceDataInfo>();
            Debug.Log("getDeviceList 拿到数据 =="+ System.DateTime.Now);
            // sendToDatase(newWorkDeviceList);
            reductionFunction(newWorkDeviceList);

            // Debug.Log("newWorkDeviceList =="+newWorkDeviceList.Count);

        });

        // JsonDataManager.getInstance().LoadData<>
    }

    // positionInfo
    public void positionInfo(string jsonParams, UnityAction action){
        string url = assembleUrl(gloab_URL.positionInfo);


        // 从 deviceInfoData 转换到  networkDeviceDataInfo
        // networkDeviceDataInfo networkDeviceDataInfo = new networkDeviceDataInfo();
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0,\"isLabeled\": 1}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
            // newWorkDeviceListClass dataObj = JsonUtility.FromJson<newWorkDeviceListClass>(webRequest.downloadHandler.text);
            // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
            action();
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
                deviceInfoData.deviceCategory = 0;
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



    // public void getDeviceList22222(){

    //     //获取设备列表
    //     string url = assembleUrl(gloab_URL.deviceList);
    //     // "/evo-apigw/evo-fdbu/1.0.0/" + gloab_URL.deviceList;//请求地址
    //     // string jsonParams = "{\"pageNum\": 1, \"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1}";
    //     // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0, \"isLabeled\": 1}";
    //     // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1, \"parentModelId\": \"123\"}";
    //     string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 1,}";
    //     networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
    //         // Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
    //         networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
    //         // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
    //         newWorkCurrentPageDeviceList = dataObj.data.pageData.ToList<networkDeviceDataInfo>();
    //         foreach (networkDeviceDataInfo item in newWorkCurrentPageDeviceList)
    //         {
    //             Debug.Log("networkDeviceDataInfo =="+ item.deviceName +"===" + item.imei);
    //         }

    //         Debug.Log("请求成功 newWorkCurrentPageDeviceList =="+ newWorkCurrentPageDeviceList.Count());
    //     });
    // }





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

                deviceInfoData.positionX = (float)item.position.positionX;
                deviceInfoData.positionY = (float)item.position.positionY;
                deviceInfoData.positionZ = (float)item.position.positionZ;

                deviceInfoData.scaleX = (float)item.scale.scaleX;
                deviceInfoData.scaleY = (float)item.scale.scaleY;
                deviceInfoData.scaleZ = (float)item.scale.scaleZ;

                deviceInfoData.rotateX = (float)item.rotate.rotateX;
                deviceInfoData.rotateY = (float)item.rotate.rotateY;
                deviceInfoData.rotateZ = (float)item.rotate.rotateZ;
                UpDateListBaseData.Add(deviceInfoData);
            }
            Debug.Log("getDeviceList 拿到数据完成数据处理 =="+ System.DateTime.Now);


            // 发送数据给  receiveDataController 进行渲染数据


            EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(
                gloab_EventCenter_Name.DATABASE_OPERATE, 
                "InsertOrReplace", 
                globalUtils.getInstance().createMyEventArgs(UpDateListBaseData, "InsertOrReplace", "deviceInfoData")
            );

    }





    // Delete positionDel
    public void delete(string jsonParams, UnityAction action){
         string url = assembleUrl(gloab_URL.positionDel);
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 1,}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            Debug.Log("delete 请求成功" + webRequest.downloadHandler.text);
            // networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
            // // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
            // Debug.Log("请求成功 delete =="+ dataObj);
            action();
        });
    }







}


