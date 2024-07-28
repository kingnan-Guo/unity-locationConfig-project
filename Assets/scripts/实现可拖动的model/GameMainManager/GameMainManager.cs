using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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




#endregion

#region




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



    public AxisComponentType axisComponentType = AxisComponentType.moveAxes;

    #endregion


    void Awake(){
        
    }
    
    void Start()
    {

        //画面设置
        // QualitySettings.vSyncCount = 0;//垂直同步


        #region 使用单例管理的 函数

        /// 射线检测 分发数据        
        getGameObjectThroughMousePosition getGameObjectThroughMousePosition = new getGameObjectThroughMousePosition();
        MonoManager.getInstance().AddUpdateListener(getGameObjectThroughMousePosition.Update);


        // 鼠标事件 管理
        mouseInputMgr.getInstance();


        // 获取当前选中的物体
        getCurrentGameObject getCurrentGameObject = new getCurrentGameObject();


        // 给 模型 添加  xyz 轴
        addAxesToModel addAxesToModel = new addAxesToModel();


        addModelToScene addModelToScene = new addModelToScene();
        


        #endregion
    }

    void Update()
    {
        
    }
}
