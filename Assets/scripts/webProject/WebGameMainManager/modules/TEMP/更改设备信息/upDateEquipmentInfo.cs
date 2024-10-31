using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upDateEquipmentInfo : baseManager<upDateEquipmentInfo>
{
    public upDateEquipmentInfo(){
        // 接收信息
        EventCenterOptimize.getInstance().AddEventListener<EquipmentBaseClass>(gloab_EventCenter_Name.UPDATE_EQUIPMENT_INFO, (equipmentInfo) => {
            // 更改信息
            // Debug.Log("更改信息");
            updateEquipmentInfo(equipmentInfo);
        });
    }


    // 第一步判断设备设备，第二部判断 要做什么操作 新增删除 修改 、第三步 进行操作，然后通知 相关模型 如果是报警 那么添加报警动画
    public void updateEquipmentInfo(EquipmentBaseClass equipmentInfo){
        // Debug.Log("equipmentInfo.baseData.GetType().Name == " + equipmentInfo.baseData.GetType().Name);

        string classTypeName = equipmentInfo.baseData.GetType().Name;
        // Debug.Log("classTypeName == " + classTypeName);

        if(classTypeName == "deviceInfoData"){


            // 更改信息
            if(equipmentInfo.equipmentEvent.equipmentEventType == EquipmentEventType.UPDATE){
                updateDeviceInfo(equipmentInfo);
            }

        }
    }



    // 定制
    public void updateDeviceInfo(EquipmentBaseClass equipmentInfo){
        
        // 找到 设备
        deviceInfoData deviceInfoData = WebGameMainManager.deviceInfoDataDictionary[equipmentInfo.keyword];

        //deviceInfoData.deviceStatus = EquipmentStatusDictionary.ALARM;//(equipmentInfo.baseData as deviceInfoData).deviceStatus;

        Debug.Log("WebGameMainManager.deviceInfoDataDictionary[equipmentInfo.keyword] 11== " + 
            WebGameMainManager.deviceInfoDataDictionary[equipmentInfo.keyword].imei + "==" + 
            WebGameMainManager.deviceInfoDataDictionary[equipmentInfo.keyword].deviceStatus
        );

        deviceInfoData.deviceStatus = (equipmentInfo.baseData as deviceInfoData).deviceStatus;
        Debug.Log("WebGameMainManager.deviceInfoDataDictionary[equipmentInfo.keyword] 222== " + 
            WebGameMainManager.deviceInfoDataDictionary[equipmentInfo.keyword].imei + "==" + 
            WebGameMainManager.deviceInfoDataDictionary[equipmentInfo.keyword].deviceStatus
        );
    }


}
