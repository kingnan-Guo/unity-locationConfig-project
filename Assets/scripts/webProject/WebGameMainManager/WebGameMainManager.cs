using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WebGameMainManager : SingletonAutoMono<WebGameMainManager>
{

    public static Dictionary<string, deviceInfoData> deviceInfoDataDictionary = new Dictionary<string, deviceInfoData>();

    void Awake(){
        EventCenterOptimize.getInstance().AddEventListener<buildingList>(gloab_EventCenter_Name.BUILDING_INFO_OF_JSON, (buildingListInfo) => {
            setBuildingDictionary(buildingListInfo);
        });

        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.MAIN_MAP_LOAD_DONE, (res) =>{
            Debug.Log(" gloab_EventCenter_Name MAIN_MAP_LOAD_DONE ==" + res);
        });
    }

    void Start()
    {
        // 与web端的 通信管理器

        
        // // 获取当前点击的物体
        getCurrentGameObject getCurrentGameObject = new getCurrentGameObject();

        /// 射线检测 分发数据        
        getGameObjectThroughMousePosition getGameObjectThroughMousePosition = new getGameObjectThroughMousePosition();
        MonoManager.getInstance().AddUpdateListener(getGameObjectThroughMousePosition.Update);
        // 鼠标事件 管理
        mouseInputMgr.getInstance();
        // networkManager 网络管理
        networkManager.getInstance();
        // 建筑物 管理器
        buildingController.getInstance();
        // 接收数据控制器
        WebReceiveDataController.getInstance();


        // ----------- temp -----------
        clickDevice.getInstance();
        upDateEquipmentInfo.getInstance();
        buildingAnimationController.getInstance();
        modelStatusController.getInstance();
        // -----------------
        

    }

    void Update()
    {
        
    }

    /// <summary>
    /// 给 _buildingDictionary 赋值 楼层的模型信息
    /// </summary>
    private void setBuildingDictionary(buildingList buildingListInfo){
        // 这里是网络请求 异步
        gloab_static_data.buildingListInfo = buildingListInfo;
        buildingListInfo.data.ToList().ForEach((item) => {
            item.position = Vector3.zero;
            //存入 全局变量
           gloab_static_data.buildingDictionary.Add(item.name, item);
        });
    }

}
