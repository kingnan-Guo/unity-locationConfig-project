using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragTo3D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // private Vector3 screenPoint;
    private Vector3 offset;
    private RaycastHit hit;
 
    public LayerMask dragLayer;

    private GameObject gameObject;

    void Start(){
        



    }
    // 鼠标双击获取 当前射线检测到的物体
    // 单击事件回调函数

    // 上一次点击的时间
    private float lastClickTime = 0;

    // 两次点击之间的最大时间间隔
    private float doubleClickInterval = 0.3f;
    public void OnPointerClick(PointerEventData eventData)
    {

        // 创建列表来存储结果
        List<RaycastResult> results = new List<RaycastResult>();

        // 进行射线投射，检查物体
        EventSystem.current.RaycastAll(eventData, results);


        if (Time.time - lastClickTime < doubleClickInterval)
        {
            // 双击事件
            Debug.Log("DoubleClick!");

            foreach (var items in results)
            {
                if(items.gameObject.CompareTag(gloab_TagName.CANVAS_DEVICE_DISABLE)){
                    Debug.Log("OnBeginDrag gameObject CANVAS_DEVICE_DISABLE =="+items.gameObject.name);

                    Transform mainMap =  GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP).transform;
                    // GameObject aa =  mainMap.FindGameObjectWithTag(gloab_TagName.CANVAS_DEVICE);

                    // FindGameObjectRecursive(Transform parent, string name)
                    Transform aa = globalUtils.getInstance().FindGameObjectRecursive(mainMap, items.gameObject.name).transform;
                    
                    // GameObject.FindWithTag(gloab_TagName.CANVAS_DEVICE);
                    
                    Debug.Log("GameObject aa =="+aa);
                    Debug.Log("GameObject =="+aa.parent.name+" =="+aa.parent.gameObject.activeInHierarchy);
                
                    
                    if(aa.parent.gameObject.activeInHierarchy){
                        // if(string.Equals(aa.parent.name, "CanvasDevice")){

                        // }
                        globalUtils.getInstance().changeCameraPosition(aa, 20);
                    }
                    // Debug.Log("OnBeginDrag gameObject CANVAS_DEVICE_DISABLE =="+aa.name);
                }
                // Debug.Log("OnDrag gameObject =="+items.gameObject.name);
            }
        }
        else
        {
            // 单击事件
            Debug.Log("SingleClick!");

            foreach (var items in results)
            {
                if(items.gameObject.CompareTag(gloab_TagName.CANVAS_DEVICE)){
                    gameObject =  items.gameObject;
                    break;
                }
                // Debug.Log("OnDrag gameObject =="+items.gameObject.name);
            }

        }

        // 更新上一次点击的时间
        lastClickTime = Time.time;
    }

 
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
            if(items.gameObject.CompareTag(gloab_TagName.CANVAS_DEVICE)){
                gameObject =  items.gameObject;
                break;
            }
            if(items.gameObject.CompareTag(gloab_TagName.CANVAS_DEVICE_DISABLE)){
                Debug.Log("OnBeginDrag gameObject CANVAS_DEVICE_DISABLE =="+items.gameObject.name);
            }
            
            // Debug.Log("OnDrag gameObject =="+items.gameObject.name);
        }
        // Debug.Log("OnDrag gameObject =="+ gameObject.name);
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag eventData ==" + eventData);
        if(gameObject != null){
            // gameObject.transform.position = eventData.position;
        }
        if(globalUtils.getInstance().UIElementsBlockRaycast(GameMainManager.GetInstance().UIIgnoreRaycastTagList)){
            // Debug.Log("UIIgnoreRaycastTagList");
            if(gameObject != null){
                // gameObject.SetActive(true);
                EventCenter.getInstance().EventTrigger(gloab_EventCenter_Name.TWO_D_MODEL_IS_IN_THREE_D_SCENE, "false");
            }
            
            return;
        } else{
            if(gameObject != null){
                // gameObject.SetActive(false);
                EventCenter.getInstance().EventTrigger(gloab_EventCenter_Name.TWO_D_MODEL_IS_IN_THREE_D_SCENE, "true");
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

            EventCenter.getInstance().EventTrigger(gloab_EventCenter_Name.TWO_D_MODEL_TO_THREE_D_MODEL, gameObject.name);
            // EventCenterOptimize.getInstance().EventTrigger<string>("2dModelMoveTo3D", gameObject.name);
            Debug.Log("OnEndDrag eventData == gameObject.name ==" + gameObject.name );


            // gameObject.SetActive(false);
            // GameObject.Destroy(gameObject);
            gameObject.gameObject.tag = gloab_TagName.CANVAS_DEVICE_DISABLE; // 设置为不可拖动
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