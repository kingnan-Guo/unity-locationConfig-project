using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Token :baseManager<Token>
{
    public void refreshToken(UnityAction action) {
        MonoManager.getInstance().StartCoroutine(
            _refreshToken(action)
        );
    }
    public IEnumerator _refreshToken(UnityAction action)
    {
        string url = "https://" + gloabNetWorkConfig.ip + "/evo-apigw/evo-oauth/oauth/token";
        Debug.Log("url" + url);
        // 创建表单数据
        WWWForm formData = new WWWForm();
        // 添加文本字段
        formData.AddField("grant_type", "refresh_token");
        formData.AddField("client_id", "web_client");
        formData.AddField("client_secret", "web_client");
        formData.AddField("refresh_token", gloabNetWorkConfig.refreshToken);

        // 创建 UnityWebRequest 对象
        UnityWebRequest request = UnityWebRequest.Post(url, formData);
        // 设置请求头
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        // 发送请求前禁用证书验证
        request.certificateHandler = new WebRequestSkipCertificate();

        // 发送请求并等待响应
        var asyncOperation = request.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // 检查请求是否成功
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("refreshToken失败: " + request);
        }
        else
        {
            // 获取响应内容 request.downloadHandler.text
            string text = request.downloadHandler.text;
            tokenResponseClass tokenObj = JsonUtility.FromJson<tokenResponseClass>(text);
            tokenInfoClass tokenInfo = tokenObj.data;

            gloabNetWorkConfig.accessToken = tokenInfo.token_type + " " + tokenInfo.access_token;
            gloabNetWorkConfig.refreshToken = tokenInfo.refresh_token;
            gloabNetWorkConfig.userId = tokenInfo.userId;
            gloabNetWorkConfig.magicId = tokenInfo.magicId;
            Debug.Log("gloabNetWorkConfig.accessToken=" + gloabNetWorkConfig.accessToken);
            action();
        }
    }
}