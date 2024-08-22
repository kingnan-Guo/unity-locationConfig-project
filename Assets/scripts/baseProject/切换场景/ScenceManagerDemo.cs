using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceManagerDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScenceMgr.getInstance().LoadScence("MainScene", () => {
            Debug.Log("场景切换完成");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
