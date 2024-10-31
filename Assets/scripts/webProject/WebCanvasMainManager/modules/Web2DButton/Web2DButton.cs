using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web2DButton : baseManager<Web2DButton>
{
    public Web2DButton(){
        EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.CANVAS_BUTTON, (transform) =>{
            Debug.Log("controlModelButtonManager === " +  transform.name);
            // if(transform.name == "Delete"){
            // }



            // ========== TEMP ===================
            if(transform.name == "deviceAlarm" ){
                // if(Application.platform == RuntimePlatform.WebGLPlayer){
                //     communicationToWeb.getInstance().UnitySendMessageToWebFuntion("deviceInfo", "deviceData");
                // }
                communicationToWeb.getInstance().UnitySendMessageToWebFuntion("deviceInfo", "deviceData");
            }

            if(transform.name == "imitateAlarm0"){
                imitateAlarm.getInstance().imitateAlarmFunction0();
            } else if(transform.name == "unimitateAlarm0"){
                imitateAlarm.getInstance().unimitateAlarmFunction0();
            } else if(transform.name == "moveToImitateAlarm0"){
                imitateAlarm.getInstance().changeCameraPisition0();
            }

            // ========== TEMP ===================


        });
    }
}
