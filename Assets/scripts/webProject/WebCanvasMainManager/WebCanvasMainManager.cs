using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class WebCanvasMainManager : MonoBehaviour
{
    void Awake(){
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.CHANGE_BUILDING_CARD_ACTIVE, (res) =>{
           
            string currentActiveBuildingName = buildingCardView.getInstance().getBuildingButtonActiveTransform()?.name;
             
            if(currentActiveBuildingName != null){
                if(res != currentActiveBuildingName){
                    // EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, currentActiveBuildingName);
                    EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, res);
                }
            }
            buildingCardView.getInstance().setActiveBuilding(res);
        });
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.CHANGE_FLOOR_CARD_ACTIVE, (res) => {
            buildingCardView.getInstance().setButtonActive<Transform>(buildingCardView.getInstance().FindTransform("F "+ res));
        });
    }
    void Start()
    {
        buildingCardView.getInstance();
        buildingCardController.getInstance();
        Web2DButton.getInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0)){
           Transform transform = canvasRaycast.getInstance().RaycastAll();
           canvasClick(transform);
        }

        //鼠标悬浮 按钮
        if(true){
            Transform transform = canvasRaycast.getInstance().RaycastAll();
            buildingCardView.getInstance().buttonMaterialFunction<Transform>(transform);
        }
    }


    public void canvasClick(Transform transform){
          Debug.Log("canvasClick transform = "+ transform?.name);
        if(transform == null || transform?.tag == null){
            return;
        }
        switch (transform.tag)
        {
            case "2dBuildingButton":
                // 获取当前 active 的按钮
                if(buildingCardView.getInstance().getBuildingButtonActiveTransform()?.name == transform.name){
                    return;
                }
                // 点击了 2d 楼宇的 卡片
                // 1、切换视角 
                changeCameraPosition(transform);
                // 2、 显示楼宇的楼层卡片
                buildingCardView.getInstance().setActiveBuilding(transform.name);
                EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, transform.name);
                break;
            case "2dFloorButton":
                buildingCardView.getInstance().setButtonActive<Transform>(transform);
                EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.SHOW_APPOINT_FLOOR, transform.name.Split(" ")[1]);
                break;
            case "2dbackBtn":
                // EventCenterOptimize.getInstance().EventTrigger<string>("back", "string");
                break;
            case "2dButton":
                EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.CANVAS_BUTTON, transform);
                break;
            case "2dUnderGroundButton":
                Invoke("SWITCHUNDERGROUND", 0.5f);
                if(buildingCardView.getInstance().getBuildingButtonActiveTransform()?.name == transform.name){
                    return;
                }
                // 1、切换视角
                changeCameraPosition(Vector3.zero, 200, 0);
                buildingCardView.getInstance().setActiveBuilding("全景");
                EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, "全景");
                break;
            default:
                break;
        }
    }
    private void changeCameraPosition(Transform transform){
        gloabCameraLookAtInfo gloabCameraLookAtInfo = new gloabCameraLookAtInfo();
        buildingInfo buildingInfo = ((buildingInfo)gloab_static_data.buildingDictionary[transform.name]);
        gloabCameraLookAtInfo.position = buildingInfo.position;
        gloabCameraLookAtInfo.distance = 200;
        gloabCameraLookAtInfo.direction = buildingInfo.direction;
        EventCenterOptimize.getInstance().EventTrigger<gloabCameraLookAtInfo>(gloab_EventCenter_Name.CAMERA_POSITION, gloabCameraLookAtInfo);
    }
    private void changeCameraPosition(Vector3 position, int distance = 200, int direction = 0){
        gloabCameraLookAtInfo gloabCameraLookAtInfo = new gloabCameraLookAtInfo();
        gloabCameraLookAtInfo.position = position;
        gloabCameraLookAtInfo.distance = distance;
        gloabCameraLookAtInfo.direction = direction;
        EventCenterOptimize.getInstance().EventTrigger<gloabCameraLookAtInfo>(gloab_EventCenter_Name.CAMERA_POSITION, gloabCameraLookAtInfo);
    }
    private void SWITCHUNDERGROUND(){
        EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.SWITCH_UNDER_GROUND, "string");
    }

}
