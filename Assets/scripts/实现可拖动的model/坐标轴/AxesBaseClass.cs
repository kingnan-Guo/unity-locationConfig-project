using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

#endregion




// 此处主要实现 坐标轴的基础功能 ，之后也可以使用 此父类 管理 其他 子类
public class AxesBaseClass : MonoBehaviour
{

    // 当前要移动的 物体 
    public Transform currentMoveAxisParent;
    protected AxisType axisType = AxisType.None;

    public int DragState{
        get;
        set;
    }

    void Start()
    {
        // 悬浮到哪个轴 上 那个轴 高亮

    }
    // Update is called once per frame
    void Update()
    {
        Raycast();
        OnDrag();
    }

    protected virtual void OnDrag(){
        Debug.Log("OnDrag");
    }


    protected virtual void OnDragEnd() { }


    // 高亮 某一个 轴
    protected virtual void Highlight() { }


    /// 射线检测 点击了哪一个轴
    /// 
    protected virtual void Raycast() { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;//   碰撞点 是 世界 坐标系
        bool res = Physics.Raycast(ray,out hit);
        if(res){

        } else{

        }
    }


}
