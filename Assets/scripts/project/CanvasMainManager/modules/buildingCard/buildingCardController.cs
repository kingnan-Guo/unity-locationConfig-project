using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


// [System.Serializable]
// public class floorList{
//     public string name;
//     public string type;
// }

// [System.Serializable]
// public class buildingInfo{
//     public string name;
//     public string type;
//     public Vector3 position;
//     public int direction;
//     public floorList[] floorList;
// }

// public class buildingList{
//     public buildingInfo[] data;
//     public int code;
//     public string mainModelName;
//     public string floorKeyWord;
// }



// 添加 楼层按钮 
public class buildingCardController : baseManager<buildingCardController>
{

    // private Dictionary<string, object> dataDic = new Dictionary<string, object>();
    public buildingCardController(){

        MonoManager.getInstance().AddUpdateListener(Update);

          // Debug.Log("buildingCardController init");
        // 初始化 加载json 获取当前的楼层信息
        // 先从本地获取  之后再从
        // globalUtils.getInstance().receiveJsonDateFormResources<buildingList>("json/buildingInfo", (res) =>{
        //     // EventCenterOptimize.getInstance().EventTrigger<Dictionary<string, object>>("DeviceInfoDictionary", res);
        //     // 通知页面 添加卡片


            
        //     res.data.ToList().ForEach((item) => {
        //         //存入 全局变量
        //         gloab_static_data.buildingDictionary.Add(item.name, item);
        //         item.position = Vector3.zero;
        //         dataDic.Add(item.name, item);
        //     });

        //     // Debug.Log("buildingCardController init ==" + dataDic);
        //     EventCenterOptimize.getInstance().EventTrigger<Dictionary<string, object>>(gloab_EventCenter_Name.BUILDING_INFO_DICTIONARY,  dataDic);
        // });


        // 地图加载完成
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.MAIN_MAP_LOAD_DONE, (res) =>{
            // Debug.Log(" gloab_EventCenter_NamemainMapLoadDone init ==" + res);


            Dictionary<string, object> dataDic = new Dictionary<string, object>();
            // 模型加载完成 开始 获取 模型的诗句 用于 补充 模型 json 的位置
            gloab_static_data.buildingDictionary.ToList().ForEach((item) => {

                buildingInfo buildingInfo = (item.Value as buildingInfo);
                find(buildingInfo);
                // Debug.Log("buildingCardController init ==" + buildingInfo.position);
                // // 从全局参数 获取 建筑物信息
                // buildingInfo otherbuildingInfo = new buildingInfo();
                // otherbuildingInfo = buildingInfo;
                // buildingInfo.position = Vector3.zero;
                dataDic.Add(buildingInfo.name, buildingInfo);

            });
            // 把数据 发送 给 view 用于展示卡片 
            EventCenterOptimize.getInstance().EventTrigger<Dictionary<string, object>>(gloab_EventCenter_Name.BUILDING_INFO_DICTIONARY,  dataDic);

        });
    }






    
    public void find(buildingInfo item){

        GameObject[] arr = GameObject.FindGameObjectsWithTag(gloab_TagName.BUILDING);

        // Debug.Log("find arr ==" + arr.Length);
        globalUtils.getInstance().filterSpecifiedList<GameObject, GameObject, GameObject[], string>(
            filterBuildingTag<GameObject, GameObject, object>, 
            arr,
            item.name ,
            (res) => {

                if(res.Count > 0){
                    // Debug.Log("find res ==" + res[0].transform.position);
                    item.position = res[0].transform.position;
                }
            }
        );




    }

    /// <summary>
    /// 根据 楼幢 称获取楼层
    /// </summary>
    /// <typeparam name="TR"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="data"></param>
    /// <param name="keyWorld"></param>
    /// <returns></returns>
    public TR filterBuildingTag<TR, T, K>(T data, K keyWorld)
    {
        // Debug.Log("filterBuildingTag data ==" + data);
        // Debug.Log("filterBuildingTag keyCode ==" + keyCode);
        if((data as GameObject).name.Contains(keyWorld + "_floor") && (data as GameObject).CompareTag(gloab_TagName.BUILDING)){
            return (TR)(object)(data as GameObject);
        }
        return default(TR);
    }



    private void Update(){

    }



   

}
