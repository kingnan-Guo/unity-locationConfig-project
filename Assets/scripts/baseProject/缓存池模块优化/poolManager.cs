using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolManager : baseManager<poolManager>
{
    public Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();

    public GameObject GetObj(string name){
        GameObject obj = null;

        // 有内容
        if(poolDic.ContainsKey(name) && poolDic[name].Count > 0) {
            obj = poolDic[name][0];
            poolDic[name].RemoveAt(0);
        }

        else {
            obj = GameObject.Instantiate( Resources.Load<GameObject>(name));
            // 把对象名字 改 的 和 池子 名字一样
            obj.name = name;
        }
        // 激活 让其 显示
        obj.SetActive(true);
        return obj;
    }


    public void pushObj(string name, GameObject obj){
        
        // 有抽屉
        if(poolDic.ContainsKey(name)){
            poolDic[name].Add(obj);
        } 
        // 没有 抽屉
        else {
            poolDic.Add(name, new List<GameObject>(){obj});
        }

        //放进缓存池的东西  就要把 obj 改为 失活状态
        obj.SetActive(false);
    }
}
