using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class getLocalModel : SingletonAutoMono<getLocalModel>
{
    // Start is called before the first frame update
    void Start()
    {


        EventList();



    }

    public  void EventList(){
        getBuidingInfoFromNetwork();

        getMainMap();

        getOtherMap();
    }

    public void getBuidingInfoFromNetwork(){
        globalUtils.getInstance().receiveJsonDateFormResources<buildingList>("json/buildingInfo", (res) =>{
            buildingList buildingListData = res;

            // 设置全局的 所有信息
            // _buildingListInfo = res;
            // res.data.ToList().ForEach((item) => {
            //     item.position = Vector3.zero;
            //     //存入 全局变量
            //     // _buildingDictionary.Add(item.name, item);
            // });
            EventCenterOptimize.getInstance().EventTrigger<buildingList>(gloab_EventCenter_Name.BUILDING_INFO_OF_JSON, buildingListData);
        });

        // return default(buildingList);

    }


    private void getMainMap(){

        string name = "Assets/Prefabs/mainMap.prefab";
        GameObject obj = ABManager.GetInstance().LoadRes("mainmap", name) as GameObject;
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.tag = gloab_TagName.MAIN_MAP;

        // Debug.Log("obj.transform.childCount =="+ obj.transform.childCount);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if(obj.transform.GetChild(i).name.Contains("floor")){
                obj.transform.GetChild(i).tag ="building";
            }
        }
        
        // 将主场景 传给 GameMainManager 
        EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.GLOBAL_CURRENT_MAIN_PARENT_TRANSFORM, obj.transform);

        EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.MAIN_MAP_LOAD_DONE, "true");


    }

    private void getOtherMap(){
        string tree = "Assets/Prefabs/tree.prefab";
        GameObject treeObj = ABManager.GetInstance().LoadRes("treemap", tree) as GameObject;
        treeObj.transform.position = new Vector3(-7.26f, -4.2f, -4.22f);
    }

}
