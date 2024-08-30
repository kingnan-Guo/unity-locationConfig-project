using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;


#region 全局 public  参数

    /// <summary>
    /// AxisComponentType 轴类型 移动 旋转 放大 
    /// </summary>
    public enum AxisComponentType {
        None = 0,
        moveAxes = 1,
        RotateAxes = 2,
        ScaleAxes = 3,
    }


    /// <summary>
    /// 全局 IP 端口
    /// </summary>
    public class gloabNetWorkConfig{
        public static string networkProtocol = "https://";
        // public static string ip = "124.160.108.62";
        public static string ip = "10.56.21.135";
        public static int? port = 443;
        public static string accessToken = "bearer  1:2lwZ4PeOrSZmG3gg0O3q0vc6U971pp0G";
        public static string timeStamp = "123456";
        public static string platform = "icc";
    }


    public class gloabCameraLookAtInfo{
        public Vector3 position;
        // public Quaternion rotation;
        public int distance;
        public int direction;
    }




#endregion





public class GameMainManager : SingletonAutoMono<GameMainManager>
{
    

    #region 全局变量 写在 单例 里

    /// <summary>
    /// 忽略射线检测的图层
    /// LayerMask.NameToLayer("名称") 根据 层名称 获取其层遮罩值
    /// </summary>
    public int IgnoreRaycastLayer => LayerMask.NameToLayer("Ignore Raycast");


    /// <summary>
    /// 忽略 canvas 上 射线检测的 标签
    /// </summary>
    public List<string> UIIgnoreRaycastTagList = new List<string>() { "2dBackground" }; //忽略射线检测的标签


    /// <summary>
    /// 忽略 2dDivice 上 点击事件 标签
    /// </summary>
    public List<string> UI2dDiviceTagList = new List<string>() { "2dDivice" }; //忽略射线检测的标签

    private AxisComponentType _axisComponentType = AxisComponentType.moveAxes;
    public AxisComponentType axisComponentType{
        get{
            return _axisComponentType;
        } set{
            _axisComponentType = value;
        }
    }


    /// <summary>
    /// 当前 的 要拖拽 设备放置点位的 父物体；全局 唯一信息
    /// </summary>
    private Transform _global_currentMainParent = null;
    public Transform global_currentMainParent{
        get{
            return _global_currentMainParent;
        } set{
            // _buildingDictionary = value;
        }
    }

    private buildingList _buildingListInfo = new buildingList();
    public buildingList buildingListInfo{
        get{
            return _buildingListInfo;
        } set{
        }
    }

    // public string mainModelTag = "mainMap"; // 主模型名称

    /// <summary>
    /// 当前的楼幢 楼层的信息 
    /// </summary>
    public Dictionary<string, object> _buildingDictionary = new Dictionary<string, object>();
    public Dictionary<string, object> buildingDictionary{
        get{
            return _buildingDictionary;
        } set{
            // _buildingDictionary = value;
        }
    }

    /// <summary>
    /// 全局记录 拖放的 点位信息
    /// </summary>
    public Dictionary<string, object> global_deviceInfo = new Dictionary<string, object>();



    private List<Transform> _currentAxisParentList = new List<Transform>();
    // public gloab_TagName gloab_TagName = new gloab_TagName();
    public List<Transform> currentAxisParentList{
        get{
            return _currentAxisParentList;
        } set{
            // _currentAxisParentList = value;
        }
    }


    #endregion


    void Awake(){
        EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.GLOBAL_CURRENT_MAIN_PARENT_TRANSFORM, (res) => {
            // Debug.Log("GameMainManager === GLOBAL_CURRENT_MAIN_PARENT_TRANSFORM");
            _global_currentMainParent = res;
        });
        
        // 切换了建筑 主建筑 那么 清理当前的 currentAxisParentList
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, (Resolution) =>{
            // Debug.Log("Resolution ==="+ Resolution);

            // new List<Transform>().RemoveAll(());
            _global_currentMainParent = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP).transform;
        });

        // 切换了可以拖拽的物体
        EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.HAVE_AXES_TRANSFORM, (transform) => {
            setCurrentAxisParentList(transform);
        });

        EventCenterOptimize.getInstance().AddEventListener<buildingList>(gloab_EventCenter_Name.BUILDING_INFO_OF_JSON, (buildingListInfo) => {
            setBuildingDictionary(buildingListInfo);
        });
        
    }
    
    void Start()
    {
        Debug.Log("GameMainManager === Start");
        
        //画面设置
        // QualitySettings.vSyncCount = 0;//垂直同步


        #region 使用单例管理的 函数

        /// 射线检测 分发数据        
        getGameObjectThroughMousePosition getGameObjectThroughMousePosition = new getGameObjectThroughMousePosition();
        MonoManager.getInstance().AddUpdateListener(getGameObjectThroughMousePosition.Update);


        // 鼠标事件 管理
        mouseInputMgr.getInstance();


        // 拖拽模型管理器
        draggableModelManager.getInstance();


        // networkManager 网络管理
        networkManager.getInstance();
        // networkManager.getInstance().Factory("https://124.160.108.62/evo-apigw/evo-brm/1.2.1/dictionary?itemType=field_unit", "GET", "", (webRequest) => {
        //     Debug.Log("Factory 请求成功" + webRequest.downloadHandler.text);
        // });


        //建筑物 管理器
        // buildingController buildingController = new buildingController();
        buildingController.getInstance();


        //数据库管理器
        SQLiteSigleManager.getInstance();




        // SQLiteSigleManagerDemo.getInstance().InsertOne();
        // SQLiteSigleManagerDemo.getInstance().InsertOrReplace();
        // SQLiteSigleManagerDemo.getInstance().upDate();
        // SQLiteSigleManagerDemo.getInstance().UpDateByKeyWord();
        // SQLiteSigleManagerDemo.getInstance().DeleteById();
        // SQLiteSigleManagerDemo.getInstance().upDate();

        // 接收数据
        receiveDataController.getInstance();


        // // 之后删除
        // otherTempTest.getInstance();



        
     

        

        #endregion



        #region 调用本地函数

        #endregion
    }

    void Update()
    {
    }


    void getIP(){
    }

    /// <summary>
    /// 设置当前的 可以拖拽的物体
    /// </summary>
    /// <param name="parent"></param>
    public void setCurrentAxisParentList(Transform parent){
        clearCurrentAxisParentList();
        _currentAxisParentList.Add(parent);
    }

    /// <summary>
    /// 清空当前的 可以拖拽的物体
    /// </summary>
    private void clearCurrentAxisParentList(){
        _currentAxisParentList.RemoveAll((itme) => true);
    }

    /// <summary>
    /// 给 _buildingDictionary 赋值 楼层的模型信息
    /// </summary>
    private void setBuildingDictionary(buildingList buildingListInfo){
        // 这里是网络请求 异步
        _buildingListInfo = buildingListInfo;
        buildingListInfo.data.ToList().ForEach((item) => {
            item.position = Vector3.zero;
            //存入 全局变量
            _buildingDictionary.Add(item.name, item);
        });
    }


}
