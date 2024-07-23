using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class getGameObjectThroughMousePosition
{
    // Start is called before the first frame update
    public getGameObjectThroughMousePosition(){


        EventCenter.getInstance().AddEventListener("鼠标弹起左键", (res) => {
            getGameObject((Vector3)res);
        });


        EventCenter.getInstance().AddEventListener("鼠标位置", (res) => {
            MouseInGameObject((Vector3)res );
        });



    }


    private void getGameObject(Vector3 position){
        // Debug.Log("鼠标点击左键 11"+ res);



            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;//   碰撞点 是 世界 坐标系
            bool res = Physics.Raycast(ray,out hit);
            if(res){
                // 如果 物体上没有  collider 组件 则 无法 获取 碰撞点 ； 要求 都挂上 collider 组件
                // maxDistance 射线 最大 距离、
                // 除了 射线检测 还可以是 origin direction 检测
                // layerMask  忽略 一些
                Debug.Log("getGameObject =="+hit.collider.name);
                // Debug.Log(" ==="+ hit.transform.gameObject.tag);
                // Debug.Log(Vector3.forward );
                // Debug.Log(Time.deltaTime);
                //点击后 把事件 发给 事件中心 
                EventCenterOptimize.getInstance().EventTrigger<GameObject>("mousePositionPhysics", hit.transform.gameObject);
            }
    }


    private void MouseInGameObject(Vector3 position){
            // Debug.Log("鼠标点击左键 11"+ res);

            // Debug.Log("MouseInGameObject"+ position);
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;//   碰撞点 是 世界 坐标系
            bool res = Physics.Raycast(ray,out hit);
            if(res){
                // 如果 物体上没有  collider 组件 则 无法 获取 碰撞点 ； 要求 都挂上 collider 组件
                // maxDistance 射线 最大 距离、
                // 除了 射线检测 还可以是 origin direction 检测
                // layerMask  忽略 一些
                // Debug.Log("getGameObject =="+hit.collider.name);
                // Debug.Log(" ==="+ hit.transform.gameObject.tag);
                // if(hit.transform.gameObject.tag == "device"){
                //     // Debug.Log(" ==="+ hit.transform.gameObject.name);
                // }
                EventCenterOptimize.getInstance().EventTrigger<GameObject>("mouseMovePositionPhysics", hit.transform.gameObject);

            } else{
                // EventCenterOptimize.getInstance().EventTrigger<GameObject>("mouseMovePositionPhysics", null);

                // Debug.Log("没有点击到物体");
                EventCenterOptimize.getInstance().EventTrigger<GameObject>("mouseMovePositionPhysics", null);
            }
    }


    // Update is called once per frame
    public void Update()
    {
        
    }

}
