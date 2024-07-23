using System.Collections;
using System.Collections.Generic;
using UnityEngine;







public class GameMainManager : SingletonAutoMono<GameMainManager>
{
    

    #region 全局变量 写在 单例 里

    /// <summary>
    /// 忽略射线检测的图层
    /// LayerMask.NameToLayer("名称") 根据 层名称 获取其层遮罩值
    /// </summary>
    public int IgnoreRaycastLayer => LayerMask.NameToLayer("Ignore Raycast");




    #endregion
    
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
        

        #endregion
    }

    void Update()
    {
        
    }
}
