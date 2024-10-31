using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class buildingAnimationController : MonoBehaviour
// {

// }



public class buildingAnimationController : baseManager<buildingAnimationController>
{

    private float shake;


    private Material buildingAlarmMaterial = Resources.Load<Material>("Materials/buildingMaterial/buildingActiveMaterial");
    private Material material_back = Resources.Load<Material>("Materials/buildingMaterial/floor_bl");


    private class floorInfo{
        public List<string> ALARM = new List<string>();
        public List<string> FAULT = new List<string>();
    }

    /// <summary>
    /// 每一层对应的 设备列表
    /// </summary>
    private Dictionary<string, List<string>> buildingAlarmObject = new Dictionary<string, List<string>>();

    private Dictionary<string, floorInfo> buildingStatusDictionary = new Dictionary<string, floorInfo>();
    public buildingAnimationController(){
        MonoManager.getInstance().AddUpdateListener(Update);


        // 接收 报警推送
        EventCenterOptimize.getInstance().AddEventListener<EquipmentBaseClass>(gloab_EventCenter_Name.UPDATE_EQUIPMENT_INFO, (res) => {
            Debug.Log("buildingAnimationController 接收 报警推送");
            // if(material_back == null){
            //     Material material = Resources.Load<Material>("Materials/floor_bl");
            //     material_back = new Material(material);
            // }
            upDateData(res);
            // upDateData<EquipmentBaseClass>(res);
            
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

                        // if((equipmentInfo.baseData as deviceInfoData).deviceStatus == EquipmentStatusDictionary.ALARM){
                        //     upDateInfo((equipmentInfo.baseData as deviceInfoData).imei);
                        //     // upDateInfo2((equipmentInfo.baseData as deviceInfoData).imei, "ALARM");
                        // }

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


    /// <summary>
    /// 当前字典中 的 object 里对用多个 list ，存入设备的 imei 如果从  ALARM 变为  FAULT 那么要从  ALARM 的 list 中移除， 并添加到 FAULT 的 list 中， 如果变为 NORMAL 那么从两个 list 中都移除
    /// </summary>
    /// <param name="imei"></param>
    /// <param name="statusName"></param>
    private void upDateInfo2(string imei, string statusName){
        deviceInfoData deviceInfo = WebGameMainManager.deviceInfoDataDictionary[imei];
        Type type = typeof(floorInfo);
        var publicProperties = type.GetProperties();

        if(!buildingStatusDictionary.ContainsKey(deviceInfo.parentModelId)){
            buildingStatusDictionary.Add(deviceInfo.parentModelId, new floorInfo());
        }

        foreach (var property in publicProperties)
        {
            if(string.Equals(property.Name, statusName)){

                if(deviceInfo.deviceStatus == EquipmentStatusDictionary.ALARM){

                    List<string> list = (property.GetValue(buildingStatusDictionary[deviceInfo.parentModelId]) as List<string>);
                    list.Add(imei);
                    property.SetValue(buildingStatusDictionary[deviceInfo.parentModelId], list);

                }
                else if(deviceInfo.deviceStatus != EquipmentStatusDictionary.ALARM){

                    // if(buildingStatusDictionary.ContainsKey(deviceInfo.parentModelId)){
                    //     // buildingStatusDictionary[deviceInfo.parentModelId].Remove(deviceInfo.imei);
                    //     // if(buildingStatusDictionary[deviceInfo.parentModelId].Count == 0){
                    //     //     changeMaterial(material_back);
                    //     //     buildingStatusDictionary.Remove(deviceInfo.parentModelId);
                    //     // }
                    // }

                    if((property.GetValue(buildingStatusDictionary[deviceInfo.parentModelId]) as List<string>).Contains(imei)){
                        (property.GetValue(buildingStatusDictionary[deviceInfo.parentModelId]) as List<string>).Remove(imei);
                    }
                    
                }

            }
        }

    }


    private void upDateInfo(string imei){
        deviceInfoData deviceInfo = WebGameMainManager.deviceInfoDataDictionary[imei];
        if(deviceInfo.deviceStatus == EquipmentStatusDictionary.ALARM){

            if(!buildingAlarmObject.ContainsKey(deviceInfo.parentModelId)){
                // buildingAlarmObject.Add(deviceInfo.orgName, deviceInfo.deviceStatus);
                buildingAlarmObject.Add(deviceInfo.parentModelId, new List<string>());
                buildingAlarmObject[deviceInfo.parentModelId].Add(deviceInfo.imei);
            } else {
                if(!buildingAlarmObject[deviceInfo.parentModelId].Contains(deviceInfo.imei)){
                    buildingAlarmObject[deviceInfo.parentModelId].Add(deviceInfo.imei);
                }
                
            }
            
        }
        else if(deviceInfo.deviceStatus != EquipmentStatusDictionary.ALARM){

            if(buildingAlarmObject.ContainsKey(deviceInfo.parentModelId)){
                buildingAlarmObject[deviceInfo.parentModelId].Remove(deviceInfo.imei);
                if(buildingAlarmObject[deviceInfo.parentModelId].Count == 0){
                    changeMaterial(material_back);
                    buildingAlarmObject.Remove(deviceInfo.parentModelId);
                }
            }
            
            // buildingAlarmObject.Remove(deviceInfo.orgName);
        }
    }

    // 延时执行
    public IEnumerator delayAction(float delayTime, System.Action action)
    {
        yield return new WaitForSeconds(delayTime);
        action();
    }

    public void startAction(){
        // StartCoroutine(delayAction(1, () => {

        // }));

        MonoManager.getInstance().StartCoroutine(delayAction(0.5f, () => {
            // Debug.Log(" 0.5秒 执行一次  startAction");
            changeMaterial(material_back);

        }));
    }


    private void changeMaterial(Material material){
        foreach (string item in buildingAlarmObject.Keys)
        {
            GameObject.Find(item).gameObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(material);
        }

    }

    public void Update(){

        if (shake >1)
        {
            // currentGameObject.GetComponent<MeshRenderer>().enabled=true;
            // Debug.Log(" 一秒 执行一次");

            // buildingAlarmObject.Keys.GetEnumerator().MoveNext();
            changeMaterial(buildingAlarmMaterial);

            startAction();

            shake = 0;
        }

        shake += Time.deltaTime;

    }
}
