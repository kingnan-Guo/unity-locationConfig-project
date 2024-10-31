using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class imitateAlarm : baseManager<imitateAlarm>
{
    public imitateAlarm(){

    }

    public void imitateAlarmFunction0(){
        Debug.Log("模拟报警" + WebGameMainManager.deviceInfoDataDictionary.Count);


        if(WebGameMainManager.deviceInfoDataDictionary.Count > 0){
            string imei = WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Key;
            // deviceInfoData deviceInfoData0 = WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value;


            deviceInfoData deviceInfoData0 = globalUtils.DeepCopy<deviceInfoData>(WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value);// WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value;
            deviceInfoData0.deviceStatus = EquipmentStatusDictionary.ALARM;
            EquipmentBaseClass data = new EquipmentBaseClass();
            data.keyword = imei;
            data.baseData = deviceInfoData0;
            data.equipmentEvent = new EquipmentEvent();
            data.equipmentEvent.keyword = imei;
            data.equipmentEvent.equipmentEventType = EquipmentEventType.UPDATE;

            Debug.Log("imitateAlarmFunction0 data ==== " + data.keyword);
            
            EventCenterOptimize.getInstance().EventTrigger<EquipmentBaseClass>(gloab_EventCenter_Name.UPDATE_EQUIPMENT_INFO, data);
        }
    }

    // unimitateAlarmFunction0
    public void unimitateAlarmFunction0(){
        // Debug.Log("模拟报警" + WebGameMainManager.deviceInfoDataDictionary.Count);
        if(WebGameMainManager.deviceInfoDataDictionary.Count > 0){
            string imei = WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Key;
            deviceInfoData deviceInfoData0 = globalUtils.DeepCopy<deviceInfoData>(WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value);// WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value;
            deviceInfoData0.deviceStatus = EquipmentStatusDictionary.NORMAL;
            EquipmentBaseClass data = new EquipmentBaseClass();
            data.keyword = imei;
            data.baseData = deviceInfoData0;
            data.equipmentEvent = new EquipmentEvent();
            data.equipmentEvent.keyword = imei;
            data.equipmentEvent.equipmentEventType = EquipmentEventType.UPDATE;

            Debug.Log("imitateAlarmFunction0 data ==== " + data.keyword);
            
            EventCenterOptimize.getInstance().EventTrigger<EquipmentBaseClass>(gloab_EventCenter_Name.UPDATE_EQUIPMENT_INFO, data);
        }
    }

    // 将视角 跳转 到  对应的报警设备
    public void changeCameraPisition0(){
        if(WebGameMainManager.deviceInfoDataDictionary.Count > 0){
            string imei = WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Key;
            deviceInfoData deviceInfoData0 = globalUtils.DeepCopy<deviceInfoData>(WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value);// WebGameMainManager.deviceInfoDataDictionary.FirstOrDefault().Value;
            
            new DragTo3D().FindDevicePositionInTreeDScene(imei);
        }
    }
}
