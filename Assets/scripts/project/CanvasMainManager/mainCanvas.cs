using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class mainCanvas : MonoBehaviour
{


    void Awake(){

        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.CHANGE_BUILDING_CARD_ACTIVE, (res) =>{
    
            
            // if(buildingCardView.getInstance().getBuildingButtonActiveTransform()?.name == res){
            //     return;
            // } else {

            //     // EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, buildingCardView.getInstance().getBuildingButtonActiveTransform()?.name);
            //     buildingCardView.getInstance().setActiveBuilding(res);
            // }
            // Debug.Log("getBuildingButtonActiveTransform =="+ buildingCardView.getInstance().getBuildingButtonActiveTransform()?.name);

            string currentActiveBuildingName = buildingCardView.getInstance().getBuildingButtonActiveTransform()?.name;

            // Debug.Log("currentActiveBuildingName + "+ currentActiveBuildingName +" ==res == "+  res);
 
            if(currentActiveBuildingName != null){
                if(res != currentActiveBuildingName){

                    // EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, currentActiveBuildingName);
                    EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, res);
                }
            }

            buildingCardView.getInstance().setActiveBuilding(res);
            // Debug.Log("currentActiveBuildingName 2 "+ currentActiveBuildingName +" ==res == "+  res);
            
        });

        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.CHANGE_FLOOR_CARD_ACTIVE, (res) => {
            buildingCardView.getInstance().setButtonActive<Transform>(buildingCardView.getInstance().FindTransform("F "+ res));
        });

    }

    // Start is called before the first frame update
    void Start()
    {
        // DragTo3D.GetInstance();

        buildingCardView.getInstance();
        buildingCardController.getInstance();

        controlModelButtonManager.getInstance();

        deviceListController.getInstance();

        

        // this.FindChildControl<>()


        // this.FindChildControl<Button>();

        // FindChildControl<Button>();

        // 当所有按钮都添加完成之后 会再次调用  FindChildControl<Button>();
        // this.callFindChildControl<Button>();




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
            deviceListController.getInstance().deviceHover(transform);
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
                // buildingCardView.getInstance().showFloorCardView(transform.name);
                // 
                buildingCardView.getInstance().setActiveBuilding(transform.name);
                
                EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, transform.name);
                break;
            case "2dFloorButton":
                  // Debug.Log("2dFloorButton" + transform.name.Split(" ")[1]);

                buildingCardView.getInstance().setButtonActive<Transform>(transform);

                EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.SHOW_APPOINT_FLOOR, transform.name.Split(" ")[1]);
                break;
            case "2dbackBtn":
                  // Debug.Log("backBtn");
                EventCenterOptimize.getInstance().EventTrigger<string>("back", "string");
                break;
            case "2dButton":
                  // Debug.Log("2dButton");
                EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.CANVAS_BUTTON, transform);
                break;
            case "2dUnderGroundButton":
                GameMainManager.GetInstance().underGroundStatus  = !GameMainManager.GetInstance().underGroundStatus;
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
