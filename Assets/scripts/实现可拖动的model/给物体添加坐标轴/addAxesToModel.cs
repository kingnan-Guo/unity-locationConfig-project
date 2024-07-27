using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addAxesToModel
{

    private Transform currentAxesParent; // 当前坐标轴的父节点
    public addAxesToModel(){
        EventCenterOptimize.getInstance().AddEventListener<GameObject>("mousePositionPhysics", (res) => {
            addAxesToModelMethod(res);
        });
    }

    // 过滤 给 选中的设备 添加 xyz 轴 ，之前选择的设备清除掉 xyz 轴
    // 从资源中获取 预制体

    public void addAxesToModelMethod(GameObject model){

        if(model.CompareTag("device")){
            if(isAxesAdded(model)){
                Debug.Log("已经添加了坐标轴");


                // 移除坐标轴
            } else {
                // Transform MoveAxesParent = isAxesInScene();
                if(currentAxesParent !=null && currentAxesParent != model){
                    // 清除
                    
                    // Transform MoveAxes = currentAxesParent.Find("MoveAxes(Clone)");

                    // Debug.Log("==="+ currentAxesParent.GetComponentInChildren<moveAxes>());
                    // Debug.Log("需要 清除 坐标轴"+ MoveAxes);
                    // GameObject.Destroy(MoveAxes.gameObject);
                    // currentAxesParent = null;
                    if(currentAxesParent.GetComponentInChildren<moveAxes>()){
                        GameObject.Destroy(currentAxesParent.GetComponentInChildren<moveAxes>().gameObject);
                    }
                    if(currentAxesParent.GetComponentInChildren<RotateAxes>()){
                        GameObject.Destroy(currentAxesParent.GetComponentInChildren<RotateAxes>().gameObject);
                    }
                    if(currentAxesParent.GetComponentInChildren<ScaleAxes>()){
                        GameObject.Destroy(currentAxesParent.GetComponentInChildren<ScaleAxes>().gameObject);
                    }

                    currentAxesParent = null;
                }
                GameObject gameObject = null;
                if(GameMainManager.GetInstance().axisComponentType == AxisComponentType.moveAxes){
                    gameObject = ResourcesMgr.getInstance().Load<GameObject>("Prefabs/MoveAxes");
                    gameObject.transform.parent = model.transform;
                    currentAxesParent = model.transform;
                    // 添加 坐标轴后 给 坐标轴的 moveAxes 脚本 中的 currentAxisParent 赋值
                    moveAxes moveAxes = currentAxesParent.GetComponentInChildren<moveAxes>();
                    moveAxes.currentAxisParent = model.transform;
                }
                if(GameMainManager.GetInstance().axisComponentType == AxisComponentType.RotateAxes){
                    gameObject = ResourcesMgr.getInstance().Load<GameObject>("Prefabs/RotateAxes");
                    gameObject.transform.parent = model.transform;
                    currentAxesParent = model.transform;
                    RotateAxes RotateAxes = currentAxesParent.GetComponentInChildren<RotateAxes>();
                    RotateAxes.currentAxisParent = model.transform;

                }
                if(GameMainManager.GetInstance().axisComponentType == AxisComponentType.ScaleAxes){
                    gameObject = ResourcesMgr.getInstance().Load<GameObject>("Prefabs/ScaleAxes");
                    gameObject.transform.parent = model.transform;
                    currentAxesParent = model.transform;
                    ScaleAxes ScaleAxes = currentAxesParent.GetComponentInChildren<ScaleAxes>();
                    ScaleAxes.currentAxisParent = model.transform;
                }
                gameObject.transform.localPosition = Vector3.zero;

            }
        }
    }


    // 查看当前 物体是否已经添加了坐标轴 
    private bool isAxesAdded(GameObject model){
        // 查找 model 下子集
        // 如果有 xyz 轴，则返回 true
        // Transform MoveAxes = model.transform.Find("MoveAxes(Clone)");
        if(model.transform.GetComponentInChildren<moveAxes>() || model.transform.GetComponentInChildren<RotateAxes>() || model.transform.GetComponentInChildren<ScaleAxes>()){
            return true;
        }
        // if(MoveAxes != null){
        //     return true;
        // }
        return false;
    }

    // 当前坐标轴 是否 已经添加到 场景中, 返回 坐标轴的 父级 gameObject
    private Transform isAxesInScene(){
        GameObject MoveAxes = GameObject.Find("MoveAxes");
        Debug.Log("MoveAxes: " + MoveAxes);
        Transform MoveAxesParent = null;
        if(MoveAxes != null){
            MoveAxesParent = MoveAxes.transform.parent;
        };
        return MoveAxesParent;
    }

}
