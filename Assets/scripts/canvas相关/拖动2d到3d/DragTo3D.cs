using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragTo3D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // private Vector3 screenPoint;
    private Vector3 offset;
    private RaycastHit hit;
 
    public LayerMask dragLayer;

    private GameObject gameObject;
 
    public void OnBeginDrag(PointerEventData eventData)
    {


        // gameObject = eventData.pointerDrag;
        // Debug.Log("OnBeginDrag" + eventData);
        // Debug.Log("OnBeginDrag gameObject ==" + gameObject);
        // gameObject = eventData.pointerDrag;
        // var canvas = FindInParent<Canvas>(gameObject);
        // if (canvas != null)
        // {
        //     screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        //     offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, screenPoint.z));
        // }

        // PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        // pointerEventData.position = Input.mousePosition;


        // Debug.Log("transform.Find(background) ===" + transform.Find("background").name);

        // GraphicRaycaster raycaster = transform.Find("background").GetComponent<GraphicRaycaster>();

        // raycaster.enabled = false; // 启用按钮的射线检测
        // if (raycaster != null)
        // {
        //     raycaster.enabled = false; // 禁用按钮的射线检测
        // }
        // 创建列表来存储结果
        List<RaycastResult> results = new List<RaycastResult>();

        // 进行射线投射，检查物体
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var items in results)
        {
            if(items.gameObject.CompareTag("2dDevice")){
                gameObject =  items.gameObject;
                break;
            }
            // Debug.Log("OnDrag gameObject =="+items.gameObject.name);
        }
        // Debug.Log("OnDrag gameObject =="+items.gameObject.name);
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag eventData ==" + eventData);
        if(gameObject != null){
            gameObject.transform.position = eventData.position;
        }
        if(globalUtils.getInstance().UIElementsBlockRaycast(GameMainManager.GetInstance().UIIgnoreRaycastTagList)){
            // Debug.Log("UIIgnoreRaycastTagList");
            if(gameObject != null){
                gameObject.SetActive(true);
            }
            
            return;
        } else{
            if(gameObject != null){
                gameObject.SetActive(false);
            }
        }
        // Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        // if (Physics.Raycast(ray, out hit, 100f))
        // {
        //     // transform.position = hit.point + offset;
        //     Debug.Log("UI hit ==" + hit.transform.name);
        // }



    }
 
    public void OnEndDrag(PointerEventData eventData)
    {

        
        // 如果 当前鼠标点  在 背景  之外 那么 从背景中 删除 此 拖动的2d 物体
        if(globalUtils.getInstance().UIElementsBlockRaycast(GameMainManager.GetInstance().UIIgnoreRaycastTagList)){
            gameObject = null;
            return;
        }

        Debug.Log("OnEndDrag eventData ==" + eventData );
        // 通知 addModel 模块 添加 模型

        if(gameObject != null){

            EventCenter.getInstance().EventTrigger("2dModelTo3DModel", gameObject.name);
            // EventCenterOptimize.getInstance().EventTrigger<string>("2dModelMoveTo3D", gameObject.name);
            Debug.Log("OnEndDrag eventData == gameObject.name ==" + gameObject.name );


            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
        gameObject = null;
    }
 
    private T FindInParent<T>(GameObject go) where T : Component
    {
        Debug.Log("FindInParent");
        if (go == null) return null;
        var comp = go.GetComponent<T>();
 
        if (comp != null)
            return comp;
        else
            return FindInParent<T>(go.transform.parent.gameObject);
    }

    public void SetRaycastTarget(bool value)
    {
        // canvas.raycastTarget = value;
    }


}