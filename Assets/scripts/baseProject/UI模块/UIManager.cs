using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Bottom,
    Middle,
    Top,
    System
}


public class UIManager : baseManager<UIManager>
{
    public Dictionary<string, BasePanel> PanelDict = new Dictionary<string, BasePanel>();
    
    // 记录 UI 的canvas 对象 方便以后 外部应用
    public RectTransform canvas;

    private Transform bottom;
    private Transform middle;
    private Transform top;
    private Transform system;
    public UIManager(){
       
        // 找到 canvas 对象  
        GameObject gameObject = ResourcesMgr.getInstance().Load<GameObject>("baseProject/UI/Canvas");
        canvas = gameObject.transform as RectTransform;
        // 过场景不移除
        GameObject.DontDestroyOnLoad(gameObject);

        bottom = canvas.Find("Bottom");
        middle = canvas.Find("Middle");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        
            
        // 找到 EventSystem 对象
        gameObject =ResourcesMgr.getInstance().Load<GameObject>("baseProject/UI/EventSystem");
            // 过场景不移除
        GameObject.DontDestroyOnLoad(gameObject);
    }


    //// <summary>
    /// 显示 面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="PlaneName">面板名称</param>
    /// <param name="layer">图层</param>
    /// <param name="callback">当面板预设体 创建 成功后 ，获取到脚本 回调</param>
    /// <returns></returns>
    public void ShowPanel<T>(string PlaneName, E_UI_Layer layer = E_UI_Layer.Middle, UnityAction<T> callback = null) where T: BasePanel
    {

        if(PanelDict.ContainsKey(PlaneName)){
            
            PanelDict[PlaneName].Show();
            if (callback != null)
            {
                callback(PanelDict[PlaneName] as T);
            }
            return;
        }
        ResourcesMgr.getInstance().LoadAsync<GameObject>(PlaneName, (obj) => {
            //做为 canvas 的子对象
            //并且  要设置它的相对位置
            Transform father = bottom;
            switch (layer)
            {
                case E_UI_Layer.Bottom:
                    father = bottom;
                    break;
                case E_UI_Layer.Middle:
                    father = middle;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
                default:
                    father = middle;
                    break;
            }

            // 设置 父对象
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;


            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;


            // 得到预设体上的 面板 脚本
            T panel = obj.GetComponent<T>();
            // 处理 面板 调用后的逻辑
            // 回调  
            if (callback != null)
            {
                callback(panel);
            }


            panel.Show();
            // 存入 脚本 ；方便 调用
            PanelDict.Add(PlaneName, panel);

        });
            
    }


    /// <summary>
    /// 隐藏 面板
    /// </summary>
    /// <param name="PlaneName">面板名称</param>
    public void HidePanel(string PlaneName){
        if (PanelDict.ContainsKey(PlaneName))
        {
            PanelDict[PlaneName].Hide();
            GameObject.Destroy(PanelDict[PlaneName].gameObject);
            PanelDict.Remove(PlaneName);
            return;
        }
    }

    public void DestroyPanel(string PlaneName){

    }



    ///<summary>
    /// 获取 面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="PlaneName">面板名称</param>
    /// <returns></returns>
    public T GetPanel<T>(string PlaneName) where T: BasePanel{

        if(PanelDict.ContainsKey(PlaneName)){
            return PanelDict[PlaneName] as T;
        }
        return null;
        
    }



    /// <summary>
    /// 获取 所有 面板
    /// </summary>
    /// <returns></returns>
    public List<BasePanel> GetAllPanel(){
        // return PanelDict.Values.ToList();
        return null;
    }



    /// <summary>
    /// 获取 图层
    /// </summary>
    /// <param name="layer">图层</param>
    /// <returns></returns>
    public Transform GetLayer(E_UI_Layer layer){
        switch (layer)
        {
            case E_UI_Layer.Bottom:
                return bottom;
                // break;
            case E_UI_Layer.Middle:
                return middle;
                // break;
            case E_UI_Layer.Top:
                return top;
            case E_UI_Layer.System:
                return system;
            default:
                return middle;
        }
    }


    /// <summary>
    /// 给 控件 添加 自定义 事件 监听
    /// </summary>
    /// <param name="control">控件</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件 相应 回调</param>
    /// </summary>
    /// 
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack){
        EventTrigger eventTrigger = control.GetComponent<EventTrigger>();
        if(eventTrigger == null){
            // control.AddComponent<EventTrigger>()
            eventTrigger = control.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;

        entry.callback.AddListener(callBack);

        // 把 new 出来 的 事件 相应对象 entry 加到  eventTrigger 中
        // 所有的 事件响应 都在 triggers 上
        eventTrigger.triggers.Add(entry);


    }



}
