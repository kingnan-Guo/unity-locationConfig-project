using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addModelToScene : baseManager<addModelToScene>
{

    private GameObject mousePositionGameObject = null;

    private Transform currentMainParent = GameMainManager.GetInstance().global_currentMainParent;
    public addModelToScene()
    {
        MonoManager.getInstance().AddUpdateListener(Update);



        EventCenter.getInstance().AddEventListener(gloab_EventCenter_Name.TWO_D_MODEL_TO_THREE_D_MODEL, (name) => {
            testFunction((string)name);
        });

        //
        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.MOUSE_MOVE_POSITION_PHYSICS, (res) => {
            mousePositionGameObject = res;
        });


        // 创建一个 特殊的  模型 用于定位 当前的 模型位置
        EventCenter.getInstance().AddEventListener("2dModelIsIn3DScene", (statue) => {
            // Debug.Log("2dModelIsIn3DScene ==" + statue);
        });
    }

    void testFunction(string name){
        // Debug.Log("testFunction 2dModelTo3DModel ==" + name + " time"+ Time.deltaTime);
        createCube(name);


    }

    private void createCube(string name){
        GameObject Cube =  cubeCube(name, Color.red);

        if(mousePositionGameObject != null){
            // Cube.transform.position = new Vector3(0, 0, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;//   碰撞点 是 世界 坐标系
            bool res = Physics.Raycast(ray,out hit);
            if(res){
                Vector3 pos = hit.point;
                
                Cube.transform.position = pos;
            }
            
        } else {
            Vector3 screenZeroPosition = Camera.main.WorldToScreenPoint(GameObject.Find("LookAtCube").transform.position);
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenZeroPosition.z));
            // pos.z = 0;
            Cube.transform.position = pos;
        }
        Cube.transform.parent = GameMainManager.GetInstance().global_currentMainParent;
        Cube.transform.tag = gloab_TagName.DEVICE;

        EventCenterOptimizes.getInstance().EventTrigger<GameObject, string>(gloab_EventCenter_Name.DONE_UPDATE_MODEL, Cube, "deviceInfoData");
    }

    GameObject cubeCube(string name, Color color){
        // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = new Vector3(0, 0, 0);
        // cube.transform.localScale = new Vector3(1, 1, 1);
        // cube.transform.rotation = Quaternion.Euler(0, 0, 0);
        // cube.name = name;
        // // 设置颜色
        // cube.GetComponent<Renderer>().material.color = color;
        // return cube;

        GameObject device = GameObject.Instantiate(ResourcesMgr.getInstance().LoadPrefab<GameObject>("Models/device/40"), Vector3.zero, Quaternion.identity);
        device.transform.localScale =new Vector3(3, 3, 3);
        device.name = name;
        return device;
    }




    void Update()
    {
        
    }
}
