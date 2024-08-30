using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;



// #region 
// // 特性
// // 本质上 就是在调用 特性类 的构造函数
// //类 函数 变量 上一行， 表示他们具有 该提醒信息
// #endregion

// class myCustomAttribute : Attribute
// {
//     public string info;
//     public myCustomAttribute(string info)
//     {
//         this.info = info;
//     }

//     public void TestFun()
//     {

//         Debug.Log("TestFun myCustomAttribute this.info = " + this.info);
//     }
// }

public class WebRequestSkipCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}


// [myCustomAttribute("这是一个自定义特性1")]
public class networkManager : baseManager<networkManager>
{

    public class keyInfo{
        public  string url;
        public string timestamp;
    }

    public class dataInfo {
        public  int status = 0;

        public IEnumerator enumerator;
    }
    private Dictionary<keyInfo, dataInfo> urlDictionary = new Dictionary<keyInfo, dataInfo>();

    /// <summary>
    /// 网络管理器
    /// 
    /// 调接 http 接口
    /// 
    /// 1、获取要连接 的 ip 地址
    /// 2、post get 请求
    /// </summary>
    /// 

    public networkManager(){

        //初始化
        MonoManager.getInstance().AddUpdateListener(Update);
        Debug.Log(" == networkManager  的 构造函数");

        // MonoManager.getInstance().StartCoroutine(HTTPMethod("https://124.160.108.62/evo-apigw/evo-brm/version", "GET", a));

        // string jsonParams = "{\"checkStat\": 1,\"id\":\"001001\"}";
        // MonoManager.getInstance().StartCoroutine(HTTPMethod("https://124.160.108.62/evo-apigw/evo-brm/1.2.1/tree", "POST", jsonParams));

        // MonoManager.getInstance().StartCoroutine(
        //     HTTPMethod("https://124.160.108.62/evo-apigw/evo-brm/1.2.1/dictionary?itemType=field_unit", "GET", "", (webRequest) => {
        //         Debug.Log("请求成功" + webRequest.downloadHandler.text);
        //     })
        // );

        // MonoManager.getInstance().StartCoroutine(GET("https://www.baidu.com"));
        

        // 错误
        // string jsonParams = "{\"itemType\":\"field_unit\"}";
        // MonoManager.getInstance().StartCoroutine(HTTPMethod("https://124.160.108.62/evo-apigw/evo-brm/1.2.1/dictionary", "GET", jsonParams));
        

        
        
    }


    /// <summary>
    /// Factory 调接口的方法
    /// </summary>
    /// <param name="url">url 地址</param>
    /// <param name="method">GET POST</param>
    /// <param name="jsonParams">"{\"params1\": 1,\"params2\":\"001001\"}"</param>
    /// <param name="callback">回调</param>
    public void Factory(string url, string method, string jsonParams, Action<UnityWebRequest> callback){
        // MonoManager.getInstance().StartCoroutine(
        //     HTTPMethod(url, method, jsonParams, (webRequest) => {
        //         callback(webRequest);
        //     })
        // );
        keyInfo keyInfo = new keyInfo();
        keyInfo.url = url;
        keyInfo.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        dataInfo dataInfo = new dataInfo();
        dataInfo.enumerator =  HTTPMethod(url, method, jsonParams, (webRequest) => { 
            urlDictionary.Remove(keyInfo);
            // Debug.Log("urlDictionary =="+ urlDictionary.Count);
            callback(webRequest); 
        });


        urlDictionary.Add(keyInfo, dataInfo);
    }




    void Update(){
        

        _run();


    }

    private void _run(){

        

        // Debug.Log("urlDictionary =="+ urlDictionary.Count);

        foreach (keyInfo key in urlDictionary.Keys)
        {
            if(urlDictionary[key].status == 0){
                urlDictionary[key].status = 1;
                IEnumerator fun = urlDictionary[key].enumerator;
                MonoManager.getInstance().StartCoroutine(fun);
            }


            // if(item.status != 0){
            //     item.status = 1;
            //     IEnumerator fun =urlDictionary[item];
            //     MonoManager.getInstance().StartCoroutine(fun);
            // }
            
        }
    }



    // IEnumerator  GET(string url){
    //     using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
    //     {
    //         webRequest.certificateHandler = new WebRequestSkipCertificate();
    //         // Request and wait for the desired page.
    //         yield return webRequest.SendWebRequest();

    //         Debug.Log("success"+ webRequest);
    //         if (webRequest.responseCode == 200)//检验是否成功
    //         {
    //             string text = webRequest.downloadHandler.text;//打印获得值
    //             Debug.Log("请求成功" + text);
    //         }
    //         else
    //         {
    //             Debug.Log("请求失败：" + webRequest.responseCode);
    //         }

    //     }
    // }

    // IEnumerator  POST<T>(string url, string Params)
    // {
    //     using (UnityWebRequest webRequest = UnityWebRequest.Post(url, Params))
    //     {
    //         webRequest.certificateHandler = new WebRequestSkipCertificate();
    //         // Request and wait for the desired page.
    //         yield return webRequest.SendWebRequest();

    //         Debug.Log("success"+ webRequest);

    //     }
    // }


    // IEnumerator HTTPMethod(string url, string type)
    // {
    //     UnityWebRequest webRequest = new UnityWebRequest(url, type);
    //     webRequest.certificateHandler = new WebRequestSkipCertificate();
    //     // webRequest 的请求头
    //     webRequest.SetRequestHeader("Content-Type", "application/json");

    //     webRequest.SetRequestHeader("Authorization", "bearer 1:wgVFSOzWDTtNoLeElZGBuwygnc0WnPtm");
    //     webRequest.downloadHandler = new DownloadHandlerBuffer();//实例化下载存贮器
    //     if(type == "POST"){
    //         // string jsonParams = "{\"param1\":\"value1\",\"param2\":\"value2\"}";
    //         // checkStat: 1,id: '001001'
    //         string jsonParams = "{\"checkStat\": 1,\"id\":\"001001\"}";
    //         byte[] jsonToBytes = new System.Text.UTF8Encoding().GetBytes(jsonParams);
    //         Debug.Log("jsonToBytes ==:" + jsonToBytes);
    //         // new UploadHandlerRaw(jsonToBytes);
    //         // webRequest 带参数
    //         webRequest.uploadHandler = new UploadHandlerRaw(jsonToBytes);//实例化上传缓存器
    //         // Debug.Log("webRequest.uploadHandler ==:" + webRequest.uploadHandler.data);
    //     }
    //     yield return webRequest.SendWebRequest();
    //     if (webRequest.responseCode == 200)//检验是否成功
    //     {
    //         // string text = webRequest.downloadHandler.text;//打印获得值
    //         Debug.Log("请求成功" + webRequest.downloadHandler.text);
    //     }
    //     else
    //     {
    //         Debug.Log("请求失败：" + webRequest.responseCode);
    //     }
    // }




    IEnumerator HTTPMethod(string url, string type, string jsonParams = "{}", UnityAction<UnityWebRequest> callback = null)
    {
        UnityWebRequest webRequest = new UnityWebRequest(url, type);
        webRequest.certificateHandler = new WebRequestSkipCertificate();
        // webRequest 的请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", gloabNetWorkConfig.accessToken);
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        // Debug.Log("timestamp =="+ timestamp);
        webRequest.SetRequestHeader("Timestamp", (timestamp).ToString());
        webRequest.downloadHandler = new DownloadHandlerBuffer();//实例化下载存贮器
        byte[] jsonToBytes = new System.Text.UTF8Encoding().GetBytes(jsonParams);
        if(type == "POST"){
            Debug.Log("jsonToBytes ==:" + jsonToBytes);
            // webRequest 带参数
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToBytes);//实例化上传缓存器
        }
        yield return webRequest.SendWebRequest();


        if (webRequest.responseCode == 200)//检验是否成功
        {
            // string text = webRequest.downloadHandler.text;//打印获得值
            // Debug.Log("请求成功" + webRequest.downloadHandler.text);

            callback(webRequest);
        }
        else
        {
            Debug.Log("请求失败：" + webRequest.responseCode);
        }


    }





    public async Task<UnityWebRequest> aysncFactory(string url, string method, string jsonParams){
        return await RequestMethod(url, method, jsonParams);
    }

    async Task<UnityWebRequest> RequestMethod(string url, string type, string jsonParams = "{}"){

        UnityWebRequest webRequest = new UnityWebRequest(url, type);

        webRequest.certificateHandler = new WebRequestSkipCertificate();
        // webRequest 的请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", gloabNetWorkConfig.accessToken);
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        // Debug.Log("timestamp =="+ timestamp);
        webRequest.SetRequestHeader("Timestamp", (timestamp).ToString());
        webRequest.downloadHandler = new DownloadHandlerBuffer();//实例化下载存贮器
        if(type == "POST"){
            byte[] jsonToBytes = new System.Text.UTF8Encoding().GetBytes(jsonParams);
            Debug.Log("jsonToBytes ==:" + jsonToBytes);
            // webRequest 带参数
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToBytes);//实例化上传缓存器
        }
        await webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)//检验是否成功
        {
            // string text = webRequest.downloadHandler.text;//打印获得值
            // Debug.Log("请求成功" + webRequest.downloadHandler.text);
            return  webRequest;
        }
        else
        {
            Debug.Log("请求失败：" + webRequest.responseCode);
            return  null;
        }

        



    }







    // Update is called once per frame
    // void Update()
    // {
        
    // }
}

public class networkManagerAysnc{
    // TaskCompletionSource<object> Tasks = new TaskCompletionSource<object>();
    public networkManagerAysnc(string url, string method, string jsonParams){
    }
}