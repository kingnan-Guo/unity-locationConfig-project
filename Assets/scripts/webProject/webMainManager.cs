using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// web 
/// 打包成 web 端需要  先获取 ip  还有 token信息 等
/// </summary>
public class webMainManager : MonoBehaviour
{

    void Awake()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer){
            communicationToWeb.getInstance();
        }
    }
    void Start()
    {
        WebGameMainManager.GetInstance();
        WebMainMap.getInstance();



    }

    public void ReceiveMessageFromWeb(string data){
        // Debug.Log("testFuntion key = " + data);
        communicationToWeb.getInstance().ReceiveMessageFromWeb(data);
    }
}
