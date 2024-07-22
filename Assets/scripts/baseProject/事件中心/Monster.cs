using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public string Name = "Monster";
    public int type = 1;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Dead", 2f);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void Dead(){
        // 触发事件
        EventCenter.getInstance().EventTrigger("MonsterDead", this);

        EventCenterOptimize.getInstance().EventTrigger<Monster>("MonsterDead", this);


        EventCenterOptimize.getInstance().EventTrigger("无参数事件");
    }
}
