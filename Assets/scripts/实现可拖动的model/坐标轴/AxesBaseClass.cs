using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



#region 全局 public  参数

/// <summary>
/// AxisType 枚举 轴类型 
/// </summary>
public enum AxisType {
    None = 0,
    X = 1,
    Y = 2,
    Z = 3,
    XY = 4,
    XZ = 5,
    YZ = 6,
}


/// <summary>
/// 当前 拖拽的 状态
/// </summary>
public enum DragStateType {
    normal = 0,
    hover = 1,
    mouseButtonDown = 2,
    isDraging = 3,
}
#endregion




// 此处主要实现 坐标轴的基础功能 ，之后也可以使用 此父类 管理 其他 子类
public class AxesBaseClass : MonoBehaviour
{
    /// <summary>
    /// 要忽略的图层
    /// </summary>
    public int IgnoreRaycastLayer => LayerMask.NameToLayer("Ignore Raycast");

    /// <summary>
    /// 获取 arrow 图层的  值
    /// </summary>
    public int arrowLayer => LayerMask.NameToLayer("arrow");


    /// <summary>
    /// 当前要 移动 旋转 缩放 的物体 
    /// </summary>
    public Transform currentAxisParent;
    /// <summary>
    /// 记录当前 模型 初始化 的 旋转角度
    /// </summary>
    private Vector3 currentAxisParentInitialRotate;// 记录当前 模型 初始化 的 旋转角度

    /// <summary>
    /// 坐标轴 类型
    /// </summary>
    protected AxisType axisType = AxisType.None;

    /// <summary>
    /// DragState 当前操作 状态 机 ： 0 默认 、1 悬入箭头 或 悬入 箭头形成的 平面 2、鼠标选中 坐标轴  3、正在拖动 
    /// </summary>
    public DragStateType DragState{
        get;
        protected set;
    }

    public string currentActiveAxis;

    /// <summary>
    /// 忽略 canvas 上 射线检测的 标签; 与 utils 中 的 UIIgnoreRaycastTagList 保持一致； 
    /// 写两遍 是为了 保证 AxesBaseClass 的独立性
    /// </summary>
    public List<string> UIIgnoreRaycastTagList = new List<string>() { "2dBackground" }; //忽略射线检测的标签

    void Start()
    {
        if(currentAxisParent != null){
            currentAxisParentInitialRotate = currentAxisParent.transform.localEulerAngles;
        }
        DragState = DragStateType.normal;
        // 悬浮到哪个轴 上 那个轴 高亮

    }
    // Update is called once per frame
    void Update()
    {
        if(DragState == DragStateType.normal || DragState == DragStateType.hover){
            // Debug.Log("Raycast check ============= ==");
            // 如果 鼠标 在 canvas 上  不进行 射线检测
            if(UIElementsBlockRaycast(UIIgnoreRaycastTagList)){
                return;
            }
            Raycast();
        }
        OnDrag();
    }

    protected virtual void OnDrag(){
        Debug.Log("OnDrag");
    }


    protected virtual void OnDragEnd() { }


    // 高亮 某一个 轴
    protected virtual void Highlight() { }


    /// 射线检测 点击了哪一个轴
    protected virtual void Raycast() { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;//   碰撞点 是 世界 坐标系
        bool res = Physics.Raycast(ray,out hit, 10000, 1<<arrowLayer);

        if(res){

            // Debug.Log("hit =="+ hit.transform.name);
            bool isXYZ = hit.transform.parent == transform;
            if(isXYZ){
                
                // Debug.Log("Axis  === "+ hit.transform.gameObject.GetComponent<Axis>());

                Axis axis = hit.transform.gameObject.GetComponent<Axis>();
                DragState = DragStateType.hover;
                axisType = axis.axisType;
                // axis.isActive = true;
                axis.setActive(true);
            }
        } else{
            axisType = AxisType.None;
            DragState = DragStateType.normal;
            
            // axis = hit.transform.gameObject.GetComponent<Axis>();
            // if(axis != null){
            //     axis.setActive(true);
            // }

            // Debug.Log("Axis  === "+ axisType);
        }


    }

    /// <summary>
    /// 全局 全局 function 阻挡 射线
    /// 不传参数 忽略 所有   canvas 图层
    /// </summary>
    /// <param name="IgnoreRaycastTagList">阻挡射线的 标签 集合名称</param>
    /// <returns></returns>
    public bool UIElementsBlockRaycast(List<string> IgnoreRaycastTagList)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        // gr.Raycast(eventData, list);
        EventSystem.current.RaycastAll(eventData, list);
        if(IgnoreRaycastTagList.Count > 0){
            foreach (RaycastResult item in list){
                if (IgnoreRaycastTagList.Contains(item.gameObject.tag)){
                    return true;
                }
            }
            return false;
        } else {
            return list.Count > 0;
        }
        
    }
}
