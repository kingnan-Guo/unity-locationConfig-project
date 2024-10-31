using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelStatusController : baseManager<modelStatusController>
{

    Dictionary<string, object> alarmBallDic = new Dictionary<string, object>();
    Dictionary<string, object> statusDic = new Dictionary<string, object>();



    public modelStatusController(){

        EventCenterOptimize.getInstance().AddEventListener<EquipmentBaseClass>(gloab_EventCenter_Name.UPDATE_EQUIPMENT_INFO, (equipmentInfo) => {

            // updateEquipmentInfo(equipmentInfo);
            upDateData(equipmentInfo);
        });
    }




   private void upDateData(EquipmentBaseClass equipmentInfo)
   {

        var type = equipmentInfo.equipmentEvent.equipmentEventType;
        switch (type)
        {
            case EquipmentEventType.UPDATE:


                string classTypeName = equipmentInfo.baseData.GetType().Name;
                // Debug.Log("classTypeName == " + classTypeName);

                if(classTypeName == "deviceInfoData"){

                    // 更改信息
                    if(equipmentInfo.equipmentEvent.equipmentEventType == EquipmentEventType.UPDATE){

                        upDateInfo((equipmentInfo.baseData as deviceInfoData).imei);
                    }
                }

                break;
            case EquipmentEventType.ADD:

                break;
            default:
                break;
        }
   }

    public void upDateInfo(string imei){
        Debug.Log("upDateInfo === "+ imei);
        deviceInfoData deviceInfoData = WebGameMainManager.deviceInfoDataDictionary[imei];
        if(deviceInfoData.deviceStatus == EquipmentStatusDictionary.ALARM){
            if(!alarmBallDic.ContainsKey(imei+ "_alarmBall")){
                addModelToMap(deviceInfoData);
            }
        }
        else if(deviceInfoData.deviceStatus != EquipmentStatusDictionary.ALARM){
            deleteModel(deviceInfoData);
        }
    }


   private void addModelToMap(deviceInfoData value){
        Debug.Log("addModelToMap === "+ value.imei);
        // 烟感 Assets/resources/Prefab/3d/烟感.prefab
        addModel("Prefabs/Alarm/ALARM", value.position, value.imei + "_alarmBall", value.imei, value.deviceStatus);
   }


   private void addModel(string modelPath, Vector3 position, string name = "model", string parentName = "", string deviceStatus = "0")
   {

        // 烟感 Assets/resources/Prefab/3d/烟感.prefab
        GameObject obj = ResourcesMgr.getInstance().Load<GameObject>(modelPath);
        
        obj.transform.parent = globalUtils.getInstance().FindGameObjectRecursive(GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP).transform, parentName).transform; //GameObject.Find(parentName).transform;
        // obj.transform.position = position;
        obj.transform.localPosition = new Vector3(0,0,0);
        // obj.transform.localPosition = position;
        obj.name = name;
        // 添加标签
        // obj.tag = "alarmBall";
        alarmBallDic.Add(name, obj);
        // obj.AddComponent<BoxCollider>();
   }

    private void deleteModel(deviceInfoData value){
        GameObject.Destroy((alarmBallDic[value.imei+ "_alarmBall"] as GameObject));
        alarmBallDic.Remove(value.imei+ "_alarmBall");
    }



}
