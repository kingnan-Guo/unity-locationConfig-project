using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AssetBundleMainMapInfo {
    public static string platform = "StandaloneWindows"; /// web 
    public static string AssetBundleName = "StandaloneWindows";
    // ip + /scf3DStatic/uploads/StreamingAssets/StandaloneWindows/StandaloneWindows
    public static string pathUrl = "/scf3DStatic/uploads/StreamingAssets/";

    public static string buildingInfoJsonName = "buildingInfo.json";
    // public static string[] nameArr = new string[]{"mainmap", "treemap"};
}

public class getMainModel : baseManager<getMainModel>
{
    private AssetBundle _mianAB = null;
    private AssetBundleManifest _manifest = null;


    public getMainModel(){

        EventList();
    }



    public async void EventList(){


        await getBuidingInfoFromNetwork();

        await getMainMap();

        await getOtherMap();
    }


    private async Task<buildingList> getBuidingInfoFromNetwork(){
        string url = AssetBundleMainMapInfo.pathUrl +"/" + AssetBundleMainMapInfo.buildingInfoJsonName;
        url = networkLoadABManager.getInstance().assembleUrl(url);
        // Debug.Log("url== "+ url);
        UnityWebRequest webRequest = await networkManager.getInstance().aysncFactory(url, "GET", null);
        // Debug.Log("webRequest.downloadHandler.text =="+ webRequest.downloadHandler.text);
        buildingList buildingListData = JsonUtility.FromJson<buildingList>(webRequest.downloadHandler.text);
        // Debug.Log("buildingListData==== "+ buildingListData);
        if(buildingListData != null){
            EventCenterOptimize.getInstance().EventTrigger<buildingList>(gloab_EventCenter_Name.BUILDING_INFO_OF_JSON, buildingListData);
        }
        return buildingListData;
    }


    public async Task<int> getMainMap(){
        GameObject go = await handleData("mainmap", "Assets/Prefabs/mainMap.prefab");
        
        go.transform.position = new Vector3(0,0,0);
        go.transform.tag = gloab_TagName.MAIN_MAP;
        // Debug.Log("obj.transform.childCount =="+ obj.transform.childCount);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if(go.transform.GetChild(i).name.Contains("floor")){
                go.transform.GetChild(i).tag = gloab_TagName.BUILDING;
            }
        }
        // 将主场景 传给 GameMainManager 
        EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.GLOBAL_CURRENT_MAIN_PARENT_TRANSFORM, go.transform);
        EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.MAIN_MAP_LOAD_DONE, "true");
        return 0;
    }


    public async Task<int> getOtherMap(){
        GameObject got = await handleData("treemap", "Assets/Prefabs/tree.prefab");
        got.transform.position = new Vector3(-7.26f, -4.2f, -4.22f);
        return 0;
    }



    public async Task<GameObject> handleData(string name, string source){
        string url = AssetBundleMainMapInfo.pathUrl + AssetBundleMainMapInfo.platform +  "/" +AssetBundleMainMapInfo.AssetBundleName;
        GameObject go = await networkLoadABManager.getInstance().LoadResAsync(url, name, source) as GameObject;
        return  go;
    }
}
