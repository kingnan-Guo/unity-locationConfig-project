using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deviceListController : baseManager<deviceListController>
{
    Transform transform;
    private int page = 1;
    private int nextPage = -1;
    private int deviceCategory = 0; // 0:主机，1：探测器
    private networkDeviceDataInfo[] pageData;
    public deviceListController() {

        EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.CANVAS_BUTTON, (transform) => {
            Debug.Log("name == "+ transform.name);
            switch(transform.name) {
                case "preBtn":
                    PreClick();
                    break;
                case "nextBtn":
                    NextClick();
                    break;
                case "device":
                    page = 1;
                    deviceCategory = 0;
                    GetData();
                    break;
                case "detector":
                    page = 1;
                    deviceCategory = 1;
                    GetData();
                    break;
                default:
                    break;
            };
        });
        EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.CANVAS_BUTTON, (transform) => {
            Debug.Log("deviceName == "+ transform.name);
            switch(transform.name) {
                default:
                    break;
            };
        });
        // 定义列表容器
        transform = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS).transform.Find("deviceListContainer/Panel/Content").transform;
        // EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.HOVER_DEVICE_BOX, (transform) => {
        //     Debug.Log("name == "+ transform.name);
        // });
        GetData();
    }

    public void GetData()
    {
        // ,\"isLabeled\": 0
        string jsonParams = "{\"pageNum\": " + page + ",\"pageSize\": 10,\"deviceCategory\": " + deviceCategory + "}";
        Debug.Log("jsonParams" + jsonParams);
        networkManager.getInstance().Factory("https://10.56.21.135/evo-apigw/evo-fdbu/1.0.0/threeD/deviceList", "POST", jsonParams, (webRequest) =>{

            string text = webRequest.downloadHandler.text;

            //打印获得值
            Debug.Log("请求结果" + webRequest.downloadHandler.text);

            networkDeviceListClass dataObj = JsonUtility.FromJson<networkDeviceListClass>(text);

            pageData = dataObj.data.pageData;

            nextPage = dataObj.data.nextPage;

            SetData(dataObj.data.pageData);

        });

    }
    public Color32 hexColor = new Color32(255, 186, 0, 255);
    public Color32 defaultBgColor = new Color32(255, 255, 255, 0);
    public Color32 activeBgColor = new Color32(255, 255, 255, 20);
    public void SetData(networkDeviceDataInfo[] data)
    {

        // resources 找到预制体
        var listItemPrefab = Resources.Load<GameObject>("Prefabs/canvas/components/devicePrefabInList");

        foreach (Transform TransformChild in transform)
        {
            GameObject.Destroy(TransformChild.gameObject);
        }
        for (int i = 0; i < data.Length; i++)
        {
            GameObject listItemPrefabGameObject = GameObject.Instantiate(listItemPrefab);

            listItemPrefabGameObject.transform.SetParent(transform);
            listItemPrefabGameObject.name = data[i].imei;

            // 设置名称
            listItemPrefabGameObject.transform.Find("DeviceName").GetComponent<Text>().text = data[i].deviceName;
            listItemPrefabGameObject.transform.Find("IMEI").GetComponent<Text>().text = data[i].imei;
            
            // 设置图片
            // listItemPrefabGameObject.transform.Find("RawImage").GetComponent<RawImage>().text = data[i].deviceName;

            // 设置样式（是否已打点isLabeled：0-未打点，1-已打点）
            // Debug.Log("data[i] == " + data[i] +" = data[i].isLabeled == "+ (int)data[i].isLabeled);
            if((int)data[i].isLabeled == 1) {
                
                listItemPrefabGameObject.tag = gloab_TagName.CANVAS_DEVICE_DISABLE;
                setUIBorder(listItemPrefabGameObject);
            }
        }


        Scrollbar scrollbar = transform.parent.Find("Scrollbar").GetComponent<Scrollbar>();
        Debug.Log("scrollbar" + scrollbar);
        // 滚动条置顶
        if (scrollbar != null)
        {
            scrollbar.value = 1.0f;
        }
    }

    public Color borderColor = Color.black;
    public float borderSize = 2f;
    
    public void setUIBorder(GameObject obj)
    {
        obj.transform.Find("DeviceName").GetComponent<Text>().color = hexColor;
    }

    public void PreClick()
    {
        if(page > 1) {
            page--;
            Debug.Log("上一页");
            GetData();
        }
    }

    public void NextClick()
    {
        if(nextPage > page) {
            page = nextPage;
            Debug.Log("下一页");
            GetData();
        }
    }

    // 根据IMEI获取列表中设备数据
    public networkDeviceDataInfo getDeviceInfoByIMEI(string imei)
    {
        networkDeviceDataInfo data = new networkDeviceDataInfo();
        if(pageData.Length > 0) {
            int i = 0;
            int count = pageData.Length;
            for(i = 0; i < count; i++) {
                if(pageData[i].imei == imei) {

                    string json = globalUtils.getInstance().dataToJson<networkDeviceDataInfo>(pageData[i]);
                    Debug.Log("networkDeviceDataInfo =="+ imei + "===" + json);
                    data = pageData[i];
                }
            }
        } else {
            return data;
        }
        return data;
    }

    // transform === 2dDevice

    private Transform transformBackup;
    public void deviceHover(Transform transform)
    {

        // if(transform.imei == transformBackUp.imei){


        // } else {
        //     // if(transform.gameObject.tag == gloab_TagName.CANVAS_DEVICE){
        //     //     transform.gameObject.GetComponent<Image>().color = bac;
        //     // }
        //     // else if(transform.parent.gameObject.tag == gloab_TagName.CANVAS_DEVICE){
        //     //     transform.parent.gameObject.GetComponent<Image>().color = Color.red;
        //     // }
        // }

        if(transform != null && (transform.gameObject.tag == gloab_TagName.CANVAS_DEVICE || transform.gameObject.tag == gloab_TagName.CANVAS_DEVICE_DISABLE)){
            
            // Debug.Log("aaa=="+transform.name);
            //  transformBackUp 全局
            if(transformBackup != null )
            {
                if(transformBackup == transform){

                } else {
                    transformBackup.gameObject.GetComponent<Image>().color = defaultBgColor;
                }
            }

            

            transform.gameObject.GetComponent<Image>().color = activeBgColor;
            transformBackup = transform;
        }
    }
}
