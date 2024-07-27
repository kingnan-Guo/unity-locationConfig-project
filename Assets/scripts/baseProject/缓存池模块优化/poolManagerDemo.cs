using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolManagerDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // if(Input.GetMouseButton(0)){
        //     poolManager.getInstance().GetObj("baseProject/cube");
        // }
        // if(Input.GetMouseButton(1)){
        //     poolManager.getInstance().GetObj("baseProject/Capsule");
        // }

        // if(Input.GetMouseButton(0)){
        //     poolManagerOptimize.getInstance().GetObj("baseProject/cube");
        // }
        // if(Input.GetMouseButton(1)){
        //     poolManagerOptimize.getInstance().GetObj("baseProject/Capsule");
        // }

        
        if(Input.GetMouseButton(0)){
            // poolManagerOptimize.getInstance().GetObj("baseProject/cube");
            poolManagerOptimizeAsync.getInstance().GetObj("baseProject/cube", (res) => {
                Debug.Log("异步回调 cube");
            });
        }
        if(Input.GetMouseButton(1)){
            // poolManagerOptimize.getInstance().GetObj("baseProject/Capsule");
            poolManagerOptimizeAsync.getInstance().GetObj("baseProject/Capsule", (res) => {
                Debug.Log("异步回调 Capsule");
            });
        }

    }
}
