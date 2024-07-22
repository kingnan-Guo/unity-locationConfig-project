

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;


public class poolDataAsync{
    public GameObject fatherObj;
    public List<GameObject> poolList;
    public poolDataAsync(GameObject obj, GameObject poolObj){
        // 给我们的 抽屉 创建一个 父对象 并且 把他 作为 我们 pool（衣柜） 对象的 子物体
        fatherObj = new GameObject(obj.name);

        fatherObj.transform.parent = poolObj.transform;

        // 放到 list 中
        poolList =  new List<GameObject>(){};

        pushObj(obj);
    }


    // 放置到 对象中

    public void pushObj(GameObject obj){

        obj.SetActive(false);
        // 存起来
        poolList.Add(obj);
        // 修改 父对象
        obj.transform.parent = fatherObj.transform;
        
    }



    // 从 抽屉里 取东西
    public GameObject GetObject(){

        GameObject obj = null;
        // 取出第一个
        obj = poolList[0];
        poolList.RemoveAt(0);

       
        // 激活 让其 显示
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
}



public class poolManagerOptimizeAsync : baseManager<poolManagerOptimizeAsync>
{
    public Dictionary<string, poolDataAsync> poolDic = new Dictionary<string, poolDataAsync>();

    private GameObject poolObj;// 根节点

    public void GetObj(string name, UnityAction<GameObject> callback){
        Debug.Log("GameObject GetObj");
        // GameObject obj = null;

        // 有内容
        if(poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0) {
            // obj = poolDic[name].GetObject();
            callback(poolDic[name].GetObject());
        }

        else {
            // obj = GameObject.Instantiate( Resources.Load<GameObject>(name));
            // // 把对象名字 改 的 和 池子 名字一样
            // obj.name = name;


            // 异步加载
            ResourcesMgr.getInstance().LoadAsync<GameObject>(name, (objTemp) => {
                // obj = objTemp;
                objTemp.name = name;

                callback(objTemp);

            });
        }
        // return obj;
    }


    public void pushObj(string name, GameObject obj){
        

        if(poolObj == null){
            Debug.Log("new GameObject =");
            poolObj = new GameObject("pool");
        }

        // 有抽屉
        if(poolDic.ContainsKey(name)){
            poolDic[name].pushObj(obj);
        } 
        // 没有 抽屉
        else {
            poolDic.Add(name, new poolDataAsync(obj, poolObj));
        }

        //放进缓存池的东西  就要把 obj 改为 失活状态
        // obj.SetActive(false);
    }


    public void Clear(){
        poolDic.Clear();
        poolObj = null;
    }

}



