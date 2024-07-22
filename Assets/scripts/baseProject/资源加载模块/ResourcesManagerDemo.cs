using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManagerDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            // poolManagerOptimize.getInstance().GetObj("baseProject/cube");
            GameObject obj = ResourcesMgr.getInstance().Load<GameObject>("baseProject/cube");
            obj.transform.position = new Vector3(0,0,0);
            obj.SetActive(true);
            obj.GetComponent<Renderer>().material.color = Color.red;
        }
        if(Input.GetMouseButton(1)){
            ResourcesMgr.getInstance().LoadAsync<GameObject>("baseProject/Capsule", (obj) => {
                Debug.Log("异步加载完成");
            });

        }
    }
}
