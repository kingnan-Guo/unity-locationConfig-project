using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class globalUtils : baseManager<globalUtils>
{


    public globalUtils(){
        Debug.Log("utils");
        //初始化
        // MonoManager.getInstance().AddUpdateListener(myUpdate);
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
