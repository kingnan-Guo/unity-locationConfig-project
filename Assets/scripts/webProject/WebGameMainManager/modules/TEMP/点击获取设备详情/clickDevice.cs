using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class clickDevice : baseManager<clickDevice>
{
    public clickDevice(){
        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.MOUSE_POSITION_PHYSICS, (res) => {
            if(res.CompareTag(gloab_TagName.DEVICE)){
                getDeiveInfo(res.name);
            } else{
         
            }
        });
    }

    public async void getDeiveInfo(string imei){

        List<networkDeviceDataInfo> dataInfos = await getDataInfoOfIMEI(imei);
        dataInfos.ToList().ForEach((deviceInfoData) => {
            networkDeviceDataInfo deviceInfo = deviceInfoData as networkDeviceDataInfo;

            string json = globalUtils.getInstance().dataToJson<networkDeviceDataInfo>(deviceInfo);
            Debug.Log("networkDeviceDataInfo  json == "+ json);
            // communicationToWeb.getInstance().UnitySendMessageToWebFuntion("deviceInfo", json);
            EventCenterOptimizes.getInstance().EventTrigger<string, string>(gloab_EventCenter_Name.SEND_DATA_TO_COMMUNICATION_TO_WEB, "deviceInfo", json);
        });
    }
    private async Task<List<networkDeviceDataInfo>> getDataInfoOfIMEI(string imei){
        // 获取设备信息
        deviceInfoParams infoParams = new deviceInfoParams();
        infoParams.imei = imei;
        infoParams.pageSize = 0;
        infoParams.isLabeled = 1;
        List<networkDeviceDataInfo> data = await receiveDataFromNetworkController.getInstance().aysncGetDeviceList(infoParams);
        // Debug.Log("getDeviceList 拿到数据完成 =="+ System.DateTime.Now);
        Debug.Log("getDataInfoOfIMEI 拿到数据完成 =="+ data );
        return data;
    }

    
}
