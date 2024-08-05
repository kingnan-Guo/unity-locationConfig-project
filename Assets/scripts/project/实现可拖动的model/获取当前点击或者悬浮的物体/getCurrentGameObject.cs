using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getCurrentGameObject
{

    Material selectMaterial = Resources.Load<Material>("Materials/selectMaterial/selectMaterial");

    Material defaultMaterial = Resources.Load<Material>("Materials/defaultMaterial/default");

    private Transform currentTransform;
    public getCurrentGameObject(){
        EventCenterOptimize.getInstance().AddEventListener<GameObject>("mouseMovePositionPhysics", (res) => {
            if(res != null){
                // Debug.Log("transformModelController == " + res.name);
                selectModel(res);
            }
            
        });
    }

    // 为 悬浮的  物体 添加 高亮
    // 如果 悬浮 的 物体 是设备 那么 给他  替换 shader ，shader 是 绘制 轮廓

    protected void selectModel(GameObject gameObject){

        if(gameObject != null){
            
            // 添加选中效果
            // if(currentGameObject.tag == "Axis"){
            //      Debug.Log("currentGameObject == " + currentGameObject.name);
            // }
            // if(gameObject.tag == "device" && currentGameObject){
                
            //      Debug.Log("currentGameObject == " + currentGameObject.name);
            //      currentGameObject.GetComponent<Renderer>().material = selectMaterial;
            //     //  currentGameObject.GetComponent
            // } else {
            //     currentGameObject.GetComponent<Renderer>().material = defaultMaterial;
            // }


            if(currentTransform !=null && currentTransform.gameObject.CompareTag("device")){
                currentTransform.gameObject.GetComponent<Renderer>().material = defaultMaterial;
            }

            currentTransform = gameObject.transform;


            if(gameObject.CompareTag("device")){
                
                 currentTransform.GetComponent<Renderer>().material = selectMaterial;
                //  currentGameObject.GetComponent
            } 
            
            // else {
            //     currentTransform.GetComponent<Renderer>().material = defaultMaterial;
            // }


        } else{
            if(currentTransform !=null && currentTransform.gameObject.CompareTag("device")){
                currentTransform.gameObject.GetComponent<Renderer>().material = defaultMaterial;
            }
            // // 清除选中效果
            // if(currentGameObject != null && currentGameObject = ;){
            //     currentGameObject.GetComponent<Renderer>().material = defaultMaterial;
            // }
        }
    }
    

}
