using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System;

using UnityEngine.Events;

//// <summary>
/// 作为一个唯一的数据模型 ， 一般情况 要不自己是个单例对象
/// 要么 自己存在一个单例模式 的 对象
/// </summary>

[System.Serializable]
public class deviceInfo{
        public string deviceName;
        public int deviceId;
        public string deviceType;
        public string deviceStatus;
        public string imei;
        public string orgCode;
        public string orgName;
        public Vector3 position;
}

public class deviceData{
    public deviceInfo[] data;
    public int code;
}




public class deviceModel
{
    // unity 自带的委托； 通知 外部更新的 事件;
    // 通过 委托 与外部 建立 关系
    private event UnityAction<deviceModel> updateEvent;

    private Dictionary<string, object> dataDic = new Dictionary<string, object>();
    

    // 使用 单例 模式 实现 数据唯一性
    private static deviceModel instance = null;
    public static deviceModel Instance{
        get{
            if (instance == null){
                instance = new deviceModel();
            }
            return instance;
        }
    }

    // 构造函数 私有化
    private deviceModel(){
        init();
    }
    
    public void init(){
        TextAsset textAsset = Resources.Load<TextAsset>("json/test");
        // Debug.Log(textAsset.text);
        // 调用 同级 JsonDataManager.cs 文件下 的JsonDataManager 类
        deviceData info = JsonDataManager.getInstance().LoadData<deviceData>(textAsset.text);
        // Debug.Log(info.data.Length);

        for (int i = 0; i < info.data.Length; i++)
        {
            dataDic.Add(info.data[i].imei, info.data[i]);
        }

        // deviceInfo deviceinfo = (deviceInfo)dataDic["123321123321123$1$1$1"];
        // Debug.Log(deviceinfo.position);

        UpdateInfo();
    }
    
    public deviceInfo getDeviceInfo(string imei){
        return (deviceInfo)dataDic[imei];
    }

    // public void setDeviceInfo(string imei, deviceInfo deviceinfo){
    //     dataDic[imei] = deviceinfo;
    // }

    public void updateDeviceInfo(string imei, string key, string value){
        deviceInfo di = (deviceInfo)dataDic[imei];

        Type deviceInfoClass = typeof(deviceInfo);
        
        // MemberInfo[] memberInfos = deviceInfoClass.GetMembers();
        // foreach (var item in memberInfos)
        // {
        //     Debug.Log("GetMembers = " + item);
        // }

        
        FieldInfo fieldInfo_key = deviceInfoClass.GetField(key);
        // Debug.Log("fieldInfo_key = " + fieldInfo_key);

        // 获取到 指定的 key 值
        // string b = fieldInfo_key.GetValue(di) as string;
        // Debug.Log("获取到的 deviceInfo 中的key 的值 = " + b);

        // 反射 为 指定的 key 赋值
        fieldInfo_key.SetValue(di, value);
        // Debug.Log("给 key 对象 赋值 = " + di.deviceStatus);

        UpdateInfo();
    }


    // 添加  
    public void AddEventLister(UnityAction<deviceModel> fun){
        updateEvent += fun;
    }

    public void RemoveEventLister(UnityAction<deviceModel> fun){
        updateEvent -= fun;
    }


    public void UpdateInfo(){
        if(updateEvent != null){
            updateEvent(this);
        }
    }

}
