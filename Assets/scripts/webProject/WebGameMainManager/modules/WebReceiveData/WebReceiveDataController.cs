using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WebReceiveDataController : baseManager<WebReceiveDataController>
{
    public WebReceiveDataController(){
        MonoManager.getInstance().AddUpdateListener(Update);
        listenReceiveData();
        // receiveDataFromNetworkController.getInstance();
    }

    void Update(){
        
    }

    // 监听接收数据
    public void listenReceiveData(){
        // 地图加载完成 开始获取 数据
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.MAIN_MAP_LOAD_DONE, (res) =>{
            // 第一步获取 所有的 已经拖拽的 设备 入库 
            getAllIsLabeledDataListFromNetwork();
        });

        // 接收 从 web端发送过来的数据
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.RECEIVE_DATA_FROM_WEB, (res) =>{
            receiveDateFormCommunication(res);
        });

    }



    private async void getAllIsLabeledDataListFromNetwork(){
        deviceInfoParams infoParams = new deviceInfoParams();
        infoParams.pageSize = 0;
        infoParams.isLabeled = 1;
        List<networkDeviceDataInfo> data = await receiveDataFromNetworkController.getInstance().aysncGetDeviceList(infoParams);
        // Debug.Log("getAllIsLabeledDataListFromNetwork data == "+ data.Count());
        if(data.Count() > 0){
            // 入库
            reductionFunction(data, "getAllIsLabeledDataListFromNetwork");
            // sendToDatase(data, "getAllIsLabeledDataListFromNetwork");
        }
    }


    private void reductionFunction(List<networkDeviceDataInfo> newWorkDeviceList, string MethodName= null){
        // 多线程 
        // globalUtils.getInstance().creatThreadingPool(() =>{
        //     // getDeviceList();
        //     Debug.Log("多线程 个入库 =="+ System.DateTime.Now);
        //     sendToDatase(newWorkDeviceList, MethodName);
        // });
        sendToDatase(newWorkDeviceList, MethodName);
    }

    private void sendToDatase(List<networkDeviceDataInfo> newWorkDeviceList, string MethodName = null){

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

            deviceInfoData.localPosition = new Vector3((float)item.position.positionX, (float)item.position.positionY, (float)item.position.positionZ);
            deviceInfoData.localRotate = new Vector3((float)item.rotate.rotateX, (float)item.rotate.rotateY, (float)item.rotate.rotateZ);
            deviceInfoData.localScale = new Vector3((float)item.scale.scaleX, (float)item.scale.scaleY, (float)item.scale.scaleZ);

            // 存入全局变量
            WebGameMainManager.deviceInfoDataDictionary.Add(deviceInfoData.imei, deviceInfoData);
            UpDateListBaseData.Add(deviceInfoData);
        }
        // Debug.Log("getDeviceList 拿到数据完成数据处理 =="+ System.DateTime.Now);
        // Debug.Log(" UpDateListBaseData =="+ UpDateListBaseData.Count());
        receiveDataView.getInstance().AddModelToSecene<deviceInfoData>(UpDateListBaseData);
    }

    public class revCass {
        public string deviceName;
        public string imei;
        public long deviceCategory;
    };
    public void receiveDateFormCommunication(string data){
        
        communicateMessage communicateMessage = JsonUtility.FromJson<communicateMessage>(data);
        string keyWord = communicateMessage.keyWord;
        Debug.Log("communicationToWeb ReceiveMessageFromWeb keyWord =="+ keyWord);
        switch (keyWord)
        {
            // 推送的报警信息
            case "WarningInfo":
                Debug.Log("communicationToWeb ReceiveMessageFromWeb WarningInfo =="+ communicateMessage.message);

                revCass revData = JsonUtility.FromJson<revCass>(communicateMessage.message);
                Debug.Log("communicationToWeb ReceiveMessageFromWeb WarningInfo =="+ revData.imei + "====" + revData.deviceName + "====");

                deviceInfoData deviceInfoData = new deviceInfoData();
                deviceInfoData.imei = revData.imei;
                deviceInfoData.deviceCategory = revData.deviceCategory;
                deviceInfoData.deviceName = revData.deviceName;


                string imei = deviceInfoData.imei;
                // deviceInfoData deviceInfoData0 = WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value;

                if(WebGameMainManager.deviceInfoDataDictionary.ContainsKey(imei)){

                    // deviceInfoData deviceInfoData = globalUtils.DeepCopy<deviceInfoData>(WebGameMainManager.deviceInfoDataDictionary[deviceInfoData.imei].Value);// WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value;
                    deviceInfoData.deviceStatus = EquipmentStatusDictionary.ALARM;
                    EquipmentBaseClass edata = new EquipmentBaseClass();
                    edata.keyword = imei;
                    edata.baseData = deviceInfoData;
                    edata.equipmentEvent = new EquipmentEvent();
                    edata.equipmentEvent.keyword = imei;
                    edata.equipmentEvent.equipmentEventType = EquipmentEventType.UPDATE;
                    EventCenterOptimize.getInstance().EventTrigger<EquipmentBaseClass>(gloab_EventCenter_Name.UPDATE_EQUIPMENT_INFO, edata);
                }


                break;

            case "clearnWarningInfo":
                Debug.Log("communicationToWeb ReceiveMessageFromWeb clearnWarningInfo =="+ communicateMessage.message);
                deviceInfoData deviceInfoData2 = JsonUtility.FromJson<deviceInfoData>(communicateMessage.message);
                Debug.Log("communicationToWeb ReceiveMessageFromWeb clearnWarningInfo =="+ deviceInfoData2.imei + "====" + deviceInfoData2.deviceName + "====");

                string imei2 = deviceInfoData2.imei;
                deviceInfoData2.deviceStatus = EquipmentStatusDictionary.NORMAL;
                EquipmentBaseClass data2 = new EquipmentBaseClass();
                data2.keyword = imei2;
                data2.baseData = deviceInfoData2;
                data2.equipmentEvent = new EquipmentEvent();
                data2.equipmentEvent.keyword = imei2;
                data2.equipmentEvent.equipmentEventType = EquipmentEventType.UPDATE;
                EventCenterOptimize.getInstance().EventTrigger<EquipmentBaseClass>(gloab_EventCenter_Name.UPDATE_EQUIPMENT_INFO, data2);

                break;
            default:
                break;
        }



    }



}
