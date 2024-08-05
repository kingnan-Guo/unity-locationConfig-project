using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseOperate : MonoBehaviour
{

    private new Camera camera;

    public float smoothSpeed = 20.0f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private Vector3 cameraTargetPosition;

    // private GameObject clickObject;
    // Start is called before the first frame update
    void Start()
    {
         camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Update()
    {
        Ctrl_Cam_Move();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;//   碰撞点 是 世界 坐标系
        // bool res = Physics.Raycast(ray,out hit);
        // if(res){
        //     Debug.Log(hit.collider.name);
        // }
        
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;//   碰撞点 是 世界 坐标系
            bool res = Physics.Raycast(ray,out hit);
            if(res){
                // 如果 物体上没有  collider 组件 则 无法 获取 碰撞点 ； 要求 都挂上 collider 组件
                // maxDistance 射线 最大 距离、
                // 除了 射线检测 还可以是 origin direction 检测
                // layerMask  忽略 一些
                Debug.Log(hit.collider.name);
                // Transform cam = this.transform.parent.Find("Main Camera").transform;
                // Debug.Log("cam ="+  this.transform.parent);
                targetPosition = hit.point;
                cameraTargetPosition = targetPosition - camera.transform.forward;
                isMoving = true;
            }
        }

        if(isMoving){
            // print("isMoving");
            // Debug.Log("camera.transform.forward ="+ camera.transform.forward);
            camera.transform.position = Vector3.Lerp(camera.transform.position,  cameraTargetPosition, Time.deltaTime * smoothSpeed);
            camera.transform.LookAt(targetPosition);
            if(camera.transform.position == cameraTargetPosition){
                isMoving = false;
            }
        }

        // camera.transform.position = hit.point + new Vector3(0, 10, -10);
        
        // if (Input.GetKey(KeyCode.Mouse2)) {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     //   碰撞点 是 世界 坐标系
        //     RaycastHit[] hits = Physics.RaycastAll(ray);
        //     for (int i = 0; i < hits.Length; i++)
        //     {
        //         Debug.Log(hits[i].collider.name);
        //     }
        // }

        // if(Input.GetMouseButtonDown(0)){
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     //   碰撞点 是 世界 坐标系
        //     RaycastHit[] hits = Physics.RaycastAll(ray);
        //     for (int i = 0; i < hits.Length; i++)
        //     {
        //         Debug.Log("index =="+i + " name == " +hits[i].collider.name);
        //     }
        // }




    }


    public void Ctrl_Cam_Move()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            isMoving = false;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            isMoving = false;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isMoving = false;
        }
    }



}
