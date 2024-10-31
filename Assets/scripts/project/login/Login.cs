using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System.Security.Cryptography;
using System.Text;

using UnityEngine.SceneManagement;


using System.Threading.Tasks;
using UnityEngine.Events;

public class Login : MonoBehaviour
{
    GameObject currentGameObject;
    Transform currentTransform;
    public InputField userNameField, passwordField, ipField, portField;
    private string userName, password, ip, port;
    private Button loginButton;
    private Text errorTip;
    private string publicKey;
    // private SceneLoader sceneLoader;

    void Start()
    {
        currentGameObject = gameObject;
        currentTransform = transform;

        errorTip = currentTransform.Find("Login/InputPanel/ErrorTip").GetComponent<Text>();

        //获取按钮游戏对象
        loginButton = GameObject.Find ("Login/InputPanel/loginButton").GetComponent<Button>();
        //添加点击侦听
        loginButton.onClick.AddListener (LoginSystem);


        GameObject.Find("loginOut").GetComponent<Button>().onClick.AddListener(() => {
            Application.Quit();
        });


    }

    // 登录
    public void LoginSystem()
    {
        errorTip.text = "";
        userName = userNameField.text;
        password = passwordField.text;
        ip = ipField.text;
        port = portField.text;
        if(userName == "" || password == "" || ip == "" || port == "") {
            errorTip.text = "请完善登录信息！";
            return;
        }
        getPublicKey();
    }

    // 提交
    public void getPublicKey()
    {
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        string URL = "https://" + ip + "/evo-apigw/evo-oauth/1.0.0/oauth/public-key?t=" + timestamp;
        networkManager.getInstance()
        .Factory(
            URL,
            "GET",
            "",
            (webRequest) => {

                string text = webRequest.downloadHandler.text;

                publicKeyResponseClass res = JsonUtility.FromJson<publicKeyResponseClass>(text);

                publicKey = res.data.publicKey;
                getToken();
            });
    }


    public IEnumerator postFormData(string url, WWWForm formData, UnityAction<UnityWebRequest> callback = null)
    {

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

        // 发送请求并等待响应
        // yield return request.SendWebRequest();
        // 检查请求是否成功
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("请求失败: " + request);
        }
        else
        {
            // 获取响应内容 request.downloadHandler.text
            string text = request.downloadHandler.text;
            responseClass resObj = JsonUtility.FromJson<responseClass>(text);
            if(resObj.code == "27001007") { // token过期
                Debug.Log("token过期" + resObj);
                Token.getInstance().refreshToken(() =>{

                });
            } else {
                Debug.Log("token有效" + resObj);
                callback(request);
            }
        }
    }



    // 获取token
    public void getToken() 
    {
        string EncodeUserName = RSAEncrypt(userName, publicKey);
        string EncodePassword = RSAEncrypt(password, publicKey);

        // 创建表单数据
        WWWForm formData = new WWWForm();
        // 添加文本字段
        formData.AddField("username", EncodeUserName);
        formData.AddField("password", EncodePassword);
        formData.AddField("grant_type", "password");
        formData.AddField("client_id", "web_client");
        formData.AddField("client_secret", "web_client");
        formData.AddField("public_key", publicKey);

        // 调用协程发送请求
        StartCoroutine(
            postFormData("https://" + ip + "/evo-apigw//evo-oauth/oauth/token", formData, (response) => {
                string text = response.downloadHandler.text;
                tokenResponseClass tokenObj = JsonUtility.FromJson<tokenResponseClass>(text);
                tokenInfoClass tokenInfo = tokenObj.data;
                UpdateTokenInfo(tokenInfo);
                SceneJump();
            })
        );
    }

    // 更新token信息
    public void UpdateTokenInfo(tokenInfoClass tokenInfo)
    {
        gloabNetWorkConfig.accessToken = tokenInfo.token_type + " " + tokenInfo.access_token;
        gloabNetWorkConfig.refreshToken = tokenInfo.refresh_token;
        gloabNetWorkConfig.userId = tokenInfo.userId;
        gloabNetWorkConfig.magicId = tokenInfo.magicId;
    }

    public void SceneJump()
    {
        SceneManager.LoadScene("MainScene");
    }

    // 进行RSA加密
    public string RSAEncrypt(string _str, string _publicKey)
    {
        // 使用公钥加密密码
        byte[] encryptedData = Encrypt(Encoding.UTF8.GetBytes(_str), _publicKey);
        // 返回加密后的数据
        return Convert.ToBase64String(encryptedData);
    }
    public static byte[] Encrypt(byte[] data, string publicKeyString)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyString);
            // 将Base64解码后的公钥字节数组转换为RSAParameters对象
            RSAParameters publicKeyParams = new RSAParameters();
            publicKeyParams.Modulus = TrimZeroBytes(publicKeyBytes, 29, 128); // Modulus 部分从第29个字节开始
            publicKeyParams.Exponent = new byte[3] { 1, 0, 1 }; // Exponent 默认为 { 1, 0, 1 }
            rsa.ImportParameters(publicKeyParams);
            return rsa.Encrypt(data, false);
        }
    }
    // 用于修剪字节数组开头的零字节
    private static byte[] TrimZeroBytes(byte[] bytes, int offset, int length)
    {
        byte[] trimmedBytes = new byte[length];
        Buffer.BlockCopy(bytes, offset, trimmedBytes, 0, length);
        return trimmedBytes;
    }
    
}