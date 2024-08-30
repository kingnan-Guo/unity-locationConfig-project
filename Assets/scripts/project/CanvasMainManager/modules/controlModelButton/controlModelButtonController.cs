using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlModelButtonManager : baseManager<controlModelButtonManager>
{
    public controlModelButtonManager(){
        EventCenterOptimize.getInstance().AddEventListener<Transform>(gloab_EventCenter_Name.CANVAS_BUTTON, (transform) =>{
            Debug.Log("controlModelButtonManager" +  transform);
            if(transform.name == "Delete"){
                Delete();
            }
            else if(transform.name == "Ceiling"){
                Ceiling();
            }
            else if(transform.name == "MOVE"){
                GameMainManager.GetInstance().axisComponentType = AxisComponentType.moveAxes;
                changeAXES();
            } 
            else if(transform.name == "ROTATE"){
                GameMainManager.GetInstance().axisComponentType = AxisComponentType.RotateAxes;
                changeAXES();
            }
            else if(transform.name == "RotateInit"){
                RotateInit();
            }
        });
    }

    public void Delete(){
        Transform[] transforms = GameMainManager.GetInstance().currentAxisParentList.ToArray();
        EventCenterOptimizes.getInstance().EventTrigger<Transform[],string>(gloab_EventCenter_Name.DONE_DELETE_MODEL, transforms, "deviceInfoData");
    }

    public void Ceiling(){
        // 获取当前 主 建筑的 高度
        // Debug.Log("Ceiling ==" + GameMainManager.GetInstance().global_currentMainParent);
        // 获取 transform 的尺寸
        // Transform[] transforms = GameMainManager.GetInstance().currentAxisParentList.ToArray();
        // EventCenterOptimizes.getInstance().EventTrigger<Transform[],string>(gloab_EventCenter_Name.DONE_DELETE_MODEL, transforms, "deviceInfoData");
        foreach (Transform item in GameMainManager.GetInstance().currentAxisParentList)
        {
            Transform transformParent = item.parent;
            MeshCollider meshCollider;
            if(transformParent.TryGetComponent<MeshCollider>(out meshCollider)){
                // Debug.Log("Ceiling ==size == " + meshCollider.bounds.size);
                // Debug.Log("Ceiling == position == " + transformParent.position);
                // Debug.Log("Ceiling == 高度 == "+ transformParent.position.y);
                // Transform[] transforms = GameMainManager.GetInstance().currentAxisParentList.ToArray();
                item.position = new Vector3(item.position.x, transformParent.position.y + (meshCollider.bounds.size.y)/2, item.position.z);
                EventCenterOptimizes.getInstance().EventTrigger<GameObject, string>(gloab_EventCenter_Name.DONE_UPDATE_MODEL, item.gameObject, "deviceInfoData");
            
            }
        }
    }

    public void changeAXES(){
        Debug.Log("changeAXES");
        // EventCenterOptimizes.getInstance().EventTrigger<GameObject, string>(gloab_EventCenter_Name.CHANGE_AXES,  );

        // foreach (Transform item in GameMainManager.GetInstance().currentAxisParentList)
        // {
        //     Debug.Log("changeAXES" + item.gameObject);
        //     // EventCenterOptimize.getInstance().EventTrigger<GameObject>(gloab_EventCenter_Name.CHANGE_AXES, item.gameObject);
        // }
        GameMainManager.GetInstance().currentAxisParentList.ForEach((item) => {
            EventCenterOptimize.getInstance().EventTrigger<GameObject>(gloab_EventCenter_Name.CHANGE_AXES, item.gameObject);
        });
    }


    public void RotateInit(){
        GameMainManager.GetInstance().currentAxisParentList.ForEach((item) => {
            item.localRotation = Quaternion.Euler(0,0,0);
            item.GetChild(0).localRotation = Quaternion.Euler(0,0,0);
            EventCenterOptimizes.getInstance().EventTrigger<GameObject, string>(gloab_EventCenter_Name.DONE_UPDATE_MODEL, item.gameObject, "deviceInfoData");
        });
    }
}
