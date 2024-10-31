using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// canvas的射线检测
/// </summary>
public class canvasRaycast : baseManager<canvasRaycast>
{
    public canvasRaycast(){

    }

    public Transform RaycastAll()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        // gr.Raycast(eventData, list);
        EventSystem.current.RaycastAll(eventData, list);
        Transform ts = null;
        foreach (RaycastResult item in list) {
            // Debug.Log("UIElementsBlockRaycast ==" + item.gameObject.transform.name + "tem.gameObject.tag=" + item.gameObject.tag);
            // if(item.gameObject.transform.GetComponent<Text>() == null){
            //     ts = item.gameObject.transform;
            //     break;
            // }
            // Debug.Log("UIElementsBlockRaycast ==" + item.gameObject.name);
            if(item.gameObject.tag != "Untagged" ){
                ts = item.gameObject.transform;
                break;
            }
        }
        return ts;
    }


    public void Raycast()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        // gr.Raycast(eventData, list);
        EventSystem.current.RaycastAll(eventData, list);
        // Debug.Log("UIElementsBlockRaycast ==" + list.Count);
    }



    public void update(){

    }




}
