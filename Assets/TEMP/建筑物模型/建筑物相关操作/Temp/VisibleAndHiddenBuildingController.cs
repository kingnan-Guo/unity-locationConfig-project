// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// /// <summary>
// /// BuildingInfoClass 建筑物的 类信息
// /// </summary>
// public class BuildingInfoClass{
//     public string BuildingTag;
//     public string FloorName;
//     public string FloorNumber;

//     public string getBuildingTag(string data){
//         return data.Split("_")[0];
//     }
//     public string getFloorName(string data){
//         return data;
//     }
//     public string getFloorNumber(string data){
//         return data.Split("_")[2];
//     }
// }
public class VisibleAndHiddenBuildingController
{

    Transform currentFloor;
    private GameObject currentGameObject;


    // 记录那些建筑物被拖出
    private Dictionary<string, Transform> buildingMap = new Dictionary<string, Transform>();


    public VisibleAndHiddenBuildingController(){
        EventCenterOptimize.getInstance().AddEventListener<GameObject>("mousePositionPhysics", (res) => {
            // Debug.Log("mousePositionPhysics == "+ res.name);
            // getFloor(res);
        });
    }


    private void getFloor(GameObject res){

        currentGameObject = res;

        // if(currentFloor !=null && currentFloor.gameObject.CompareTag("building")){
        //     Transform newGameObject = currentGameObject.transform;
        //     while (newGameObject != null && !newGameObject.gameObject.CompareTag("building"))
        //     {
        //         newGameObject =  newGameObject.parent;
        //     }
        //     if(newGameObject.name != currentFloor.name){
        //         Debug.Log(" 点击的  建筑物 显示当前  === "+ currentFloor.name);
        //         buildingMap.Remove(currentFloor.name);
        //     }
        // }

        // if(currentFloor == null || (currentFloor !=null && currentGameObject.name != currentFloor.name)) {
        //     currentFloor = res.transform;
        //     if(currentFloor != null){
        //         while (currentFloor != null && !currentFloor.gameObject.CompareTag("building"))
        //         {
        //             currentFloor =  currentFloor.parent;
        //         }
        //         if(currentFloor != null && currentFloor.gameObject.CompareTag("building")){
        //             // 节流
        //             if(!buildingMap.ContainsKey(currentFloor.name)){
        //                 buildingMap.Add(currentFloor.name, currentFloor);
        //                 Debug.Log("点击 currentFloor =="+ currentFloor.name + "拖出");
        //                 return;
        //             }
        //         } else {
        //             // Debug.Log("没有 点击在 任何一个建筑物上");
        //         }
        //     } else{
        //         Debug.Log("没有父级");
        //     }
        // }


            currentFloor = res.transform;
            if(currentFloor != null){
                while (currentFloor != null && !currentFloor.gameObject.CompareTag("building"))
                {
                    currentFloor =  currentFloor.parent;
                }
                if(currentFloor != null && currentFloor.gameObject.CompareTag("building")){
                    // 节流
                    // if(!buildingMap.ContainsKey(currentFloor.name)){
                    //     buildingMap.Add(currentFloor.name, currentFloor);
                    //     Debug.Log("点击 currentFloor =="+ currentFloor.name + "拖出");
                    //     return;
                    // }
                    Debug.Log("点击 currentFloor =="+ currentFloor.name + " ====");
                    FindAllAboutBuilding(currentFloor.name);
                    return;
                } else {
                    // Debug.Log("没有 点击在 任何一个建筑物上");
                }
            } else{
                Debug.Log("没有父级");
            }




    }



    public T filterFunction<T, K>(K name) where T : new ()
    {
        T  res = new T();
        return res;
    }

    

    /// <summary>
    /// 查到当前 楼层相关的 所有 楼层；
    /// 1、查找当前 楼层 是 哪一栋；
    /// 2、当前楼层 以上 的楼层；
    /// 3、当前楼层 以下 的楼层；
    /// </summary>
    /// <param name="floorName">传入 当前 楼层 </param>
    private void FindAllAboutBuilding(string floorName){
        // Debug.Log("floorName ==" + floorName);

        // globalUtils.getInstance().filterFunction<BuildingInfoClass>(floorName);

        // BuildingInfoClass  functionToPass(string x) {
        //     return new BuildingInfoClass();
        // }

        // globalUtils.getInstance().filterSpecified<string , BuildingInfoClass>(
        //     globalUtils.getInstance().filterFunction<BuildingInfoClass>, floorName, (res) => {
        //     Debug.Log("filterSpecified floorName ==" + res);
        //     // string[] arr = (string[])res;
        //     // Debug.Log("arr[0] ==" + arr[0]);
        // });

        // 规则匹配函数
        globalUtils.getInstance().filterSpecified<string , BuildingInfoClass, string>
        (
            globalUtils.getInstance().filterFunctionOfClass<BuildingInfoClass, string>, 
            floorName, 
            (res) => {
                // Debug.Log("filterSpecified floorName ==" + res);

                // Debug.Log("res BuildingInfoClass  ==="+ (res as BuildingInfoClass).BuildingTag);
                // string[] arr = (string[])res;
                // Debug.Log("arr[0] ==" + arr[0]);


                HiddenBuilding<BuildingInfoClass>(res);
                ShowBuilding<BuildingInfoClass>(res);
            }
        );

    }

    


    /// <summary>
    /// 隐藏 data 以上的所有 模型
    /// 1、找到 具体的楼栋
    /// 2、找到 楼层
    /// 3、找到 楼层以上的 模型
    /// 4、隐藏 模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public void HiddenBuilding<T>(T data){
        BuildingInfoClass info = (data as BuildingInfoClass);
        string BuildingTag  = info.BuildingTag;
        string FloorNumber  = info.FloorNumber;
        
        
        
    }


    /// <summary>
    /// 让 data 以下 的建筑物 都  展示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public void ShowBuilding<T>(T data){

    }

}
