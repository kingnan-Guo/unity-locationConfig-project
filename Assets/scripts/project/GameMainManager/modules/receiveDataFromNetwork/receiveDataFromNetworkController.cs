using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;



public class receiveDataFromNetworkController : baseManager<receiveDataFromNetworkController>
{
    public receiveDataFromNetworkController(){}

    /// <summary>
    /// 组装 数据
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string assembleUrl(string url){
        StringBuilder stringBuilder= new StringBuilder();
        stringBuilder.Append(gloabNetWorkConfig.networkProtocol);
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
    // public void getAllDeviceList(){
    //     List<networkDeviceDataInfo> newWorkDeviceList = new List<networkDeviceDataInfo>();
    //     Debug.Log("getDeviceList 获取数据 =="+ System.DateTime.Now);
    //     //获取设备列表
    //     string url = assembleUrl(gloab_URL.deviceList);
    //     // "/evo-apigw/evo-fdbu/1.0.0/" + gloab_URL.deviceList;//请求地址
    //     // string jsonParams = "{\"pageNum\": 1, \"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1}";
    //     // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0, \"isLabeled\": 1}";
    //     // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 0, \"isLabeled\": 1, \"parentModelId\": \"123\"}";
    //     string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"isLabeled\": 1}";
    //     networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
    //         // Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
    //         networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
    //         // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
    //         newWorkDeviceList = dataObj.data.pageData.ToList<networkDeviceDataInfo>();
    //         Debug.Log("getDeviceList 拿到数据 =="+ System.DateTime.Now);
    //     });
    // }




    /// <summary>
    /// getDeviceObject 获取设备列表 object
    /// </summary>
    /// <param name="deviceInfoParams"></param>
    /// <param name="action"></param>
    public void getDeviceObject(deviceInfoParams infoParams = null, UnityAction<networkDeviceListClass> action = null){
        List<networkDeviceDataInfo> newWorkDeviceList = new List<networkDeviceDataInfo>();
        string url = assembleUrl(gloab_URL.deviceList);
        string jsonParams = "{}";
        StringBuilder stringBuilder = new StringBuilder();
        Type type = typeof(networkDeviceDataInfo);
        stringBuilder.Append("{");
        PropertyInfo[] publicProperties =  typeof(deviceInfoParams).GetProperties();
        foreach (var property in publicProperties)
        {
            if(property.GetValue(infoParams) != null){
                stringBuilder.Append($"{property.Name}: {property.GetValue(infoParams)},");
            }
        }
        stringBuilder.Length--;
        stringBuilder.Append("}");
        jsonParams = stringBuilder.ToString();
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
            action(dataObj);
        });
    }

    /// <summary>
    /// 获取设备列表 callback
    /// </summary>
    /// <param name="deviceInfoParams"></param>
    /// <param name="action"></param>
    public void getDeviceList(deviceInfoParams deviceInfoParams = null, UnityAction<List<networkDeviceDataInfo>> action = null){
        List<networkDeviceDataInfo> newWorkDeviceList = new List<networkDeviceDataInfo>();
        // Debug.Log("getDeviceList 获取数据 =="+ System.DateTime.Now);
        //获取设备列表
        string url = assembleUrl(gloab_URL.deviceList);
        deviceInfoParams Params = new deviceInfoParams();
        string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"isLabeled\": 1, \"deviceCategory\": 0}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
            newWorkDeviceList = dataObj.data.pageData.ToList<networkDeviceDataInfo>();
            action(newWorkDeviceList);
        });
    }





    

    /// <summary>
    /// 获取设备列表 异步
    /// </summary>
    /// <param name="infoParams"></param>
    /// <returns></returns>
    public async Task<List<networkDeviceDataInfo>> aysncGetDeviceList(deviceInfoParams infoParams){
        string url = assembleUrl(gloab_URL.deviceList);
        List<networkDeviceDataInfo> newWorkDeviceList = new List<networkDeviceDataInfo>();
        string jsonParams = "{\"pageNum\": 1,\"pageSize\": 1, \"deviceCategory\": 0, \"deviceName\": \"67896808769\"}";

        StringBuilder stringBuilder = new StringBuilder();
        Type type = typeof(networkDeviceDataInfo);
        stringBuilder.Append("{");
        PropertyInfo[] publicProperties =  typeof(deviceInfoParams).GetProperties();
        foreach (var property in publicProperties)
        {
            if(property.GetValue(infoParams) != null){
                stringBuilder.Append($"{property.Name}: {property.GetValue(infoParams)},");
            }
        }
        stringBuilder.Length--;
        stringBuilder.Append("}");
        // Debug.Log("stringBuilder.ToString() ==="+ stringBuilder.ToString());
        jsonParams = stringBuilder.ToString();

        UnityWebRequest webRequest = await networkManager.getInstance().aysncFactory(url, "POST", jsonParams);
        if(webRequest != null){
            // Debug.Log("webRequest.downloadHandler.text == "+ webRequest.downloadHandler.text);
            networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(webRequest.downloadHandler.text);
            newWorkDeviceList = dataObj.data.pageData.ToList<networkDeviceDataInfo>();
        }
        return newWorkDeviceList;
    }




    // positionInfo =========================
    /// <summary>
    /// 更新点位
    /// </summary>
    /// <param name="jsonParams"></param>
    /// <param name="action"></param>
    public void positionInfo(string jsonParams, UnityAction action){
        string url = assembleUrl(gloab_URL.positionInfo);
        // 从 deviceInfoData 转换到  networkDeviceDataInfo
        // networkDeviceDataInfo networkDeviceDataInfo = new networkDeviceDataInfo();
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0,\"isLabeled\": 1}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            // Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
            // newWorkDeviceListClass dataObj = JsonUtility.FromJson<newWorkDeviceListClass>(webRequest.downloadHandler.text);
            // Debug.Log("DataObj =="+dataObj.data.totalCount + " =="+dataObj.data.pageData);
            action();
        });
    }

    /// <summary>
    /// 更新代为  aysnc
    /// </summary>
    /// <param name="jsonParams"></param>
    /// <returns></returns>
    public async Task<int> aysncPositionInfo(string jsonParams){
        string url = assembleUrl(gloab_URL.positionInfo);
        UnityWebRequest webRequest = await networkManager.getInstance().aysncFactory(url, "POST", jsonParams);
        if(webRequest != null){
            return 0;
        }
        return -1;
    }



    // Delete positionDel ===========================
    /// <summary>
    /// 删除点位
    /// </summary>
    /// <param name="jsonParams"></param>
    /// <param name="action"></param>
    public void delete(string jsonParams, UnityAction action){
         string url = assembleUrl(gloab_URL.positionDel);
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"deviceCategory\": 1,}";
        networkManager.getInstance().Factory(url, "POST", jsonParams, (webRequest) => {
            // Debug.Log("delete 请求成功" + webRequest.downloadHandler.text);
            action();
        });
    }
    /// <summary>
    /// 删除点位  aysnc
    /// </summary>
    /// <param name="jsonParams"></param>
    /// <returns></returns>
    public async Task<int> aysncDelete(string jsonParams){
        string url = assembleUrl(gloab_URL.positionDel);
        UnityWebRequest webRequest = await networkManager.getInstance().aysncFactory(url, "POST", jsonParams);
        if(webRequest != null){
            return 0;
        }
        return -1;
    }





}


