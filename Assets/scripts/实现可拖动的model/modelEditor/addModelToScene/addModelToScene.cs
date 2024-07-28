using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addModelToScene : baseManager<addModelToScene>
{

    private GameObject mousePositionGameObject = null;
    public addModelToScene()
    {
        MonoManager.getInstance().AddUpdateListener(Update);


        // EventCenterOptimize.getInstance().AddEventListener<string>("2dModelMoveTo3D", (name) => {
        //     Debug.Log("2dModelMoveTo3D ==" + name);
        // });
        EventCenter.getInstance().AddEventListener("2dModelTo3DModel", (name) => {
            testFunction((string)name);
        });

        //
        EventCenterOptimize.getInstance().AddEventListener<GameObject>("mouseMovePositionPhysics", (res) => {
            mousePositionGameObject = res;
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
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Cube.transform.position = pos;

        }

        Cube.transform.tag = "device";

    }

    GameObject cubeCube(string name, Color color){
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0, 0);
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.transform.rotation = Quaternion.Euler(0, 0, 0);
        cube.name = name;
        // 设置颜色
        cube.GetComponent<Renderer>().material.color = color;
        return cube;
    }




    void Update()
    {
        
    }
}
