using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.getInstance().AddEventListener("MonsterDead", testFunction);

        EventCenterOptimize.getInstance().AddEventListener<Monster>("MonsterDead", testFunctionOptimize);


        EventCenterOptimize.getInstance().AddEventListener("无参数事件", () => {
            Debug.Log("无参数事件 被触发 = ");
        });
    }

    public void testFunction(object info){
        Debug.Log("testFunction Monster name = "+  (info as Monster).Name);
    }

    public void testFunctionOptimize(Monster info){
        Debug.Log("testFunctionOptimize Monster name = "+  info.Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy() {
        EventCenter.getInstance().RemoveEventListener("MonsterDead", testFunction);

        EventCenterOptimize.getInstance().RemoveEventListener<Monster>("MonsterDead", testFunction);
    }
}
