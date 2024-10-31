using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;




// /opt/evoWpms/scf3DStatic/uploads/StreamingAssets/WebGL
public class WebAssetBundleMainMapInfo {
    public static string platform = "WebGL"; /// web 
    public static string AssetBundleName = "WebGL";
    // ip + /scf3DStatic/uploads/StreamingAssets/StandaloneWindows/StandaloneWindows
    public static string pathUrl = "/scf3DStatic/uploads/StreamingAssets/";
    public static string buildingInfoJsonName = "buildingInfo.json";

}


public class WebMainMap : baseManager<WebMainMap>
{
    private AssetBundle _mianAB = null;
    private AssetBundleManifest _manifest = null;
    private buildingList _buildingListData;


    public WebMainMap(){
        AssetBundleMainMapInfo.platform = "WebGL";

        if(false){
            WebAssetBundleMainMapInfo.AssetBundleName = "WebGL_night";
        }
        EventList();
    }

    public async void EventList(){
        Debug.Log("WebMainMap EventList");


        await getBuidingInfoFromNetwork();

        await getMainMap();

        // await getOtherMap();
    }

    private async Task<buildingList> getBuidingInfoFromNetwork(){
        string url = WebAssetBundleMainMapInfo.pathUrl +"/" + WebAssetBundleMainMapInfo.buildingInfoJsonName;
        url = networkLoadABManager.getInstance().assembleUrl(url);
        // Debug.Log("url== "+ url);
        UnityWebRequest webRequest = await networkManager.getInstance().aysncFactory(url, "GET", null);
        // Debug.Log("webRequest.downloadHandler.text =="+ webRequest.downloadHandler.text);
        buildingList buildingListData = JsonUtility.FromJson<buildingList>(webRequest.downloadHandler.text);
        _buildingListData = buildingListData;
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
            string keyWord = _buildingListData.floorKeyWord;
            // string keyWord = GameMainManager.GetInstance().buildingListInfo.floorKeyWord;
            // Debug.Log("keyWord =="+ keyWord);
            if(go.transform.GetChild(i).name.Contains(keyWord)){
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
        string url = WebAssetBundleMainMapInfo.pathUrl + WebAssetBundleMainMapInfo.platform +  "/" +WebAssetBundleMainMapInfo.AssetBundleName;
        GameObject go = await networkLoadABManager.getInstance().LoadResAsync(url, name, source) as GameObject;
        return  go;
    }
}
