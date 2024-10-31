using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class communicateMessage{
    public string keyWord;
    public string message;

}

/// <summary>
/// 与 web 端通信的模块
/// </summary>
public class communicationToWeb : baseManager<communicationToWeb>
{
    [DllImport("__Internal")]
    private static extern string GetProtocol();
    
    [DllImport("__Internal")]
    private static extern string GetIP();

    [DllImport("__Internal")]
    private static extern string GetPort();
    [DllImport("__Internal")]
    private static extern string  GetLocalStorage(string keyWord);

    // [DllImport("__Internal")]
    // private static extern void UnityReceiveMessage(string keyWord,string Message);


    [DllImport("__Internal")]
    private static extern void UnitySendMessageToWeb(string Message);
    // Start is called before the first frame update
    public  communicationToWeb()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer){
            MonoManager.getInstance().AddUpdateListener(Update);
            init();
        }
        EventCenterOptimizes.getInstance().AddEventListener<string, string>(gloab_EventCenter_Name.SEND_DATA_TO_COMMUNICATION_TO_WEB, (keyword, message) => {
            UnitySendMessageToWebFuntion(keyword, message);
        });
    }

    private void init(){
        if(GetIP() != ""){
            gloabNetWorkConfig.ip = GetIP();
        }
        if(GetProtocol() != ""){
           gloabNetWorkConfig.networkProtocol = GetProtocol() + "//"; 
        }
        // 空字符串判断
        if(GetPort() != ""){
            gloabNetWorkConfig.port = int.Parse(GetPort());
        } else {
            if(GetProtocol() == "http"){
                gloabNetWorkConfig.port = 80;
            } else {
                gloabNetWorkConfig.port = 443;
            }
        }
    }

    // 接收 数据
    public void ReceiveMessageFromWeb(string data){
        Debug.Log("communicationToWeb ReceiveMessageFromWeb =="+ data);
        EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.RECEIVE_DATA_FROM_WEB, data);
    }

    public void UnitySendMessageToWebFuntion(string keyWord, string Message){
        Debug.Log("communicationToWeb UnitySendMessageToWebFuntion =="+ keyWord + "Message =="+ Message);
        communicateMessage communicateMessage = new communicateMessage();
        communicateMessage.keyWord = keyWord;
        communicateMessage.message = Message;
        // string json = globalUtils.getInstance().dataToJson<communicateMessage>(communicateMessage);
        string json = JsonUtility.ToJson(communicateMessage);
        Debug.Log("communicationToWeb UnitySendMessageToWeb =="+ json);
        UnitySendMessageToWeb(json);
    }
    void Update()
    {
        // 每十秒钟执行一次
        if (Time.frameCount % 60 == 0)
        {
            if(GetLocalStorage("accessToken")  != ""){
                gloabNetWorkConfig.accessToken = "bearer "+ GetLocalStorage("accessToken");
                gloabNetWorkConfig.refreshToken = GetLocalStorage("refreshToken");
            }
            // Debug.Log("communicationToWeb Update =="+ Time.frameCount);
            // Debug.Log("communicationToWeb accessToken =="+ gloabNetWorkConfig.ip + "gloabNetWorkConfig.accessToken =="+ gloabNetWorkConfig.accessToken + "gloabNetWorkConfig.refreshToken =="+ gloabNetWorkConfig.refreshToken);
        }
    }






}
