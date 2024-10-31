using System;
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
    public InputField SearchField;
    private string searchKey;
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
                case "searchIcon":
                    page = 1;
                    GetData();
                    break;
                default:
                    break;
            };
        });
        EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.CLICK_TWO_D_DEVICE, (transform) => {
            Debug.Log("deviceName == "+ transform.name);
            switch(transform.name) {
                default:
                    break;
            };
        });
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.REFRESH_CANVAS_DEVICE_LIST, (data) => {
            Debug.Log("gloab_EventCenter_Name.REFRESH_CANVAS_DEVICE_LIST == "+ data);
            GetData();
        });
        // 定义列表容器
        transform = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS)?.transform.Find("deviceListContainer/Panel/Content")?.transform;
        // EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.HOVER_DEVICE_BOX, (transform) => {
        //     Debug.Log("name == "+ transform.name);
        // });
        GetData();
    }

    public async void GetData()
    {
        SearchField = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS)?.transform.Find("deviceListContainer/Condition/SearchBox").GetComponent<InputField>();
        searchKey = SearchField.text;
        deviceInfoParams infoParams = new deviceInfoParams();
        infoParams.pageNum = page;
        infoParams.deviceCategory = deviceCategory;
        infoParams.pageSize = 10;
        if(searchKey != "") {
            infoParams.deviceName = "\"" + searchKey + "\"";
        }
        receiveDataFromNetworkController.getInstance().getDeviceObject(infoParams,(dataObj) =>{
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
        if (!IsArrayWithLength(data))
        {
            Debug.Log("无数据");
            return;
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
            if(data[i].isLabeled == 1) {
                listItemPrefabGameObject.tag = gloab_TagName.CANVAS_DEVICE_DISABLE;
                setUIBorder(listItemPrefabGameObject);
            }
        }


        Scrollbar scrollbar = transform.parent.Find("Scrollbar").GetComponent<Scrollbar>();
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

        Debug.Log("NextClick nextPage =="+ nextPage);
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
                    Debug.Log(imei + "===" + pageData[i]);
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

    public static bool IsArrayWithLength(object variable)
    {
        // 判断变量是否为数组类型
        if (variable.GetType().IsArray)
        {
            // 判断数组是否具有长度
            Array array = (Array)variable;
            if (array.Length > 0)
            {
                return true;
            }
        }

        return false;
    }
}
