using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addAxesToModel
{

    private Transform currentAxesParent; // 当前坐标轴的父节点
    public addAxesToModel(){
        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.MOUSE_POSITION_PHYSICS, (res) => {
            if(res.CompareTag(gloab_TagName.DEVICE)){
                addAxesToModelMethod(res);
            } else{
                // if(res.transform.parent.CompareTag(gloab_TagName.DEVICE) || (res.transform.parent.parent != null && res.transform.parent.parent.CompareTag(gloab_TagName.DEVICE))){
                // } else {
                //     // RemoveAxes();
                // }
            }
        });

        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.CHANGE_AXES, (res) => {
            Debug.Log("gloab_EventCenter_Name.CHANGE_AXES");
            if(res.CompareTag(gloab_TagName.DEVICE)){
                addAxesToModelMethod(res);

                // int AxisComponentTypeNumber =  isAxesAdded(res);
                // if(AxisComponentTypeNumber > 0){
                //     Debug.Log("已经添加了坐标轴");
                //     if((int)GameMainManager.GetInstance().axisComponentType != AxisComponentTypeNumber){
                //         Debug.Log("切换 坐标轴");
                //         RemoveAxes();
                //         AddAxes(res);
                //     }
                // }


                // // if(res.name != currentAxesParent.name){
                // //     // 通知 GameMainManager 修改 CurrentAxisParentList
                // //     EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.HAVE_AXES_TRANSFORM, res.transform);
                // // }
                // EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.HAVE_AXES_TRANSFORM, res.transform);


            }


        });

    }

    // 过滤 给 选中的设备 添加 xyz 轴 ，之前选择的设备清除掉 xyz 轴
    // 从资源中获取 预制体

    public void addAxesToModelMethod(GameObject model){

        if(model.CompareTag(gloab_TagName.DEVICE)){
            int AxisComponentTypeNumber =  isAxesAdded(model);
            if(AxisComponentTypeNumber > 0){
                Debug.Log("已经添加了坐标轴");

                // Debug.Log("坐标轴类型"+ AxisComponentTypeNumber);
                // Debug.Log("axisComponentType = "+ GameMainManager.GetInstance().axisComponentType);

                if((int)GameMainManager.GetInstance().axisComponentType != AxisComponentTypeNumber){
                    Debug.Log("切换 坐标轴");
                    RemoveAxes();
                    AddAxes(model);
                }
                
                // 移除坐标轴
            } 
            else {
                // Transform MoveAxesParent = isAxesInScene();
                if(currentAxesParent !=null && currentAxesParent != model){
                    // 清除
                    
                    // Transform MoveAxes = currentAxesParent.Find("MoveAxes(Clone)");

                    // Debug.Log("==="+ currentAxesParent.GetComponentInChildren<moveAxes>());
                    // Debug.Log("需要 清除 坐标轴"+ MoveAxes);
                    // GameObject.Destroy(MoveAxes.gameObject);
                    // currentAxesParent = null;
                    // if(currentAxesParent.GetComponentInChildren<moveAxes>()){
                    //     GameObject.Destroy(currentAxesParent.GetComponentInChildren<moveAxes>().gameObject);
                    // }
                    // if(currentAxesParent.GetComponentInChildren<RotateAxes>()){
                    //     GameObject.Destroy(currentAxesParent.GetComponentInChildren<RotateAxes>().gameObject);
                    // }
                    // if(currentAxesParent.GetComponentInChildren<ScaleAxes>()){
                    //     GameObject.Destroy(currentAxesParent.GetComponentInChildren<ScaleAxes>().gameObject);
                    // }

                    // currentAxesParent = null;
                    RemoveAxes();
                }

                AddAxes(model);
                

            }

            // if(model.name != currentAxesParent.name){
            //     // 通知 GameMainManager 修改 CurrentAxisParentList
            //     EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.HAVE_AXES_TRANSFORM, model.transform);
            // }
            if(!GameMainManager.GetInstance().currentAxisParentList.Contains(model.transform)){
                EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.HAVE_AXES_TRANSFORM, model.transform);
            }

            // Debug.Log("model =="+ model + "=currentAxesParent=="+ currentAxesParent);

            // EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.HAVE_AXES_TRANSFORM, model.transform);

           
            
        }
    }


    // 添加坐标轴
    public void AddAxes(GameObject model){
        GameObject gameObject = null;
        IObservable IObservable = null;
        if(GameMainManager.GetInstance().axisComponentType == AxisComponentType.moveAxes){
            gameObject = ResourcesMgr.getInstance().Load<GameObject>("Prefabs/Axes/MoveAxes");
            gameObject.transform.parent = model.transform;
            currentAxesParent = model.transform;
            // 添加 坐标轴后 给 坐标轴的 moveAxes 脚本 中的 currentAxisParent 赋值
            moveAxes moveAxes = currentAxesParent.GetComponentInChildren<moveAxes>();
            moveAxes.currentAxisParent = model.transform;
            IObservable = moveAxes;
        }
        if(GameMainManager.GetInstance().axisComponentType == AxisComponentType.RotateAxes){
            gameObject = ResourcesMgr.getInstance().Load<GameObject>("Prefabs/Axes/RotateAxes");
            gameObject.transform.parent = model.transform;
            currentAxesParent = model.transform;
            RotateAxes RotateAxes = currentAxesParent.GetComponentInChildren<RotateAxes>();
            RotateAxes.currentAxisParent = model.transform;
            IObservable = RotateAxes;
        }
        if(GameMainManager.GetInstance().axisComponentType == AxisComponentType.ScaleAxes){
            gameObject = ResourcesMgr.getInstance().Load<GameObject>("Prefabs/Axes/ScaleAxes");
            gameObject.transform.parent = model.transform;
            currentAxesParent = model.transform;
            ScaleAxes ScaleAxes = currentAxesParent.GetComponentInChildren<ScaleAxes>();
            ScaleAxes.currentAxisParent = model.transform;
            IObservable = ScaleAxes;
        }
        gameObject.transform.localPosition = Vector3.zero;


        getComponent(IObservable);

    }


    // 查看当前 物体是否已经添加了坐标轴 
    private int isAxesAdded(GameObject model){
        // 查找 model 下子集
        // 如果有 xyz 轴，则返回 true
        // Transform MoveAxes = model.transform.Find("MoveAxes(Clone)");
        if(model.transform.GetComponentInChildren<moveAxes>()){
            return 1;
        }else if(model.transform.GetComponentInChildren<RotateAxes>()){
            return 2;
        } else if(model.transform.GetComponentInChildren<ScaleAxes>()){
            return 3;
        }
        // if(MoveAxes != null){
        //     return true;
        // }
        return -1;
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

    public void RemoveAxes(){
        if(currentAxesParent != null){
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




    }

    /// <summary>
    /// 给 AxesBaseClass 添加订阅
    /// AxesBaseClass.Done 函数 会 触发 
    /// 
    /// </summary>
    /// <param name="iObservable"></param>
    private void getComponent(IObservable iObservable){
        iObservable.Subscribe(() =>{
            // Debug.Log("Subscribe aa ==="+ currentAxesParent);
            NotifyModelTransform(currentAxesParent.gameObject);
        });
    }

    // 通知更新设备的位置信息
    private void NotifyModelTransform(GameObject model){
        EventCenterOptimizes.getInstance().EventTrigger<GameObject, string>(gloab_EventCenter_Name.DONE_UPDATE_MODEL, model, "deviceInfoData");
    }
}
