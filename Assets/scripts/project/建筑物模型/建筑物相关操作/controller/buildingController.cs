using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



/// <summary>
/// BuildingInfoClass 建筑物的 类信息
/// </summary>
public class BuildingInfoClass{
    public string BuildingTag;
    public string FloorName;
    public string FloorNumber;
    public int FloorRelativePositionMark;

    public string getBuildingTag(string data){
        return data.Split("_")[0];
    }
    public string getFloorName(string data){
        return data;
    }
    public string getFloorNumber(string data){
        return data.Split("_")[2];
    }
}


/// <summary>
/// 建筑物 数据 的操作 类
/// </summary>
public class buildingController
{


    private GameObject currentGameObject;



    private Material material_back = Resources.Load<Material>("Materials/buildingMaterial/floor_bl");

    Transform currentHoverFloor;

    private Transform currentMainFloor;

    public buildingController(){

        EventCenterOptimize.getInstance().AddEventListener<GameObject>("mouseMovePositionPhysics", (res) => {

            if(res != null){
                if(currentHoverFloor !=null && currentHoverFloor.tag == "building"){
                    buildingView.getInstance().backToBuildMaterial<GameObject>(currentHoverFloor.gameObject);
                }
                currentGameObject = res;
                currentHoverFloor = res.transform;

                if(currentHoverFloor != null) {
                    if(currentHoverFloor != null){
                
                        while (currentHoverFloor != null && !currentHoverFloor.gameObject.CompareTag("building"))
                        {
                            currentHoverFloor =  currentHoverFloor.parent;
                        }

                        // Debug.Log("currentHoverFloor =="+ currentHoverFloor.name + "== ");

                        if(currentHoverFloor != null &&currentHoverFloor.gameObject.CompareTag("building")){
                            
                            buildingView.getInstance().changeToBuildMaterialToSelect<GameObject>(currentHoverFloor.gameObject);
                        }
                        else {
                            // Debug.Log("没有 悬浮在 任何一个建筑物上");      
                            if(currentGameObject !=null && currentGameObject.tag == "building"){
                                buildingView.getInstance().backToBuildMaterial<GameObject>(currentGameObject);
                            }
                        }
                    } else{
                        Debug.Log("没有父级");
                    }
                }
            }

        });


        EventCenterOptimize.getInstance().AddEventListener<GameObject>("mousePositionPhysics", (res) => {
            Debug.Log("mousePositionPhysics == "+ res.name + "currentHoverFloor =="+  currentHoverFloor);

            if(currentHoverFloor != null){

                if(currentMainFloor == currentHoverFloor){
                    return;
                }
                currentMainFloor = currentHoverFloor;
                FindAllAboutBuilding(currentMainFloor.name);

                
            }
        });




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
                HiddenOrShowBuilding<BuildingInfoClass>(res);
                // ShowBuilding<BuildingInfoClass>(res);
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
    public void HiddenOrShowBuilding<T>(T data){
        BuildingInfoClass info = (data as BuildingInfoClass);
        string BuildingTag  = info.BuildingTag;
        string FloorNumber  = info.FloorNumber;
        filterSpecifiedFloor(info.BuildingTag, info.FloorName,info.FloorNumber);
    }


    /// <summary>
    /// 让 data 以下 的建筑物 都  展示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public void ShowBuilding<T>(T data){

    }


    // // 过滤 数据返回 数组
    public void filterSpecifiedFloor(string BuildingTag, string FloorName, string FloorNumber){
        


        // globalUtils.getInstance().filterSpecified<string, List<BuildingInfoClass> , string>
        // (
        //     globalUtils.getInstance().filterFunctionOfListClass< , string>, 
        //     floorName, 
        //     (res) => {
        //         // Debug.Log("filterSpecified floorName ==" + res);

        //         // Debug.Log("res BuildingInfoClass  ==="+ (res as BuildingInfoClass).BuildingTag);
        //         // // string[] arr = (string[])res;
        //         // // Debug.Log("arr[0] ==" + arr[0]);


        //         // HiddenBuilding<BuildingInfoClass>(res);
        //         // ShowBuilding<BuildingInfoClass>(res);
        //     }
        // );

   
        // 传入  
        
        // 先查找 这个楼 多少层


        if(currentMainFloor != null){
            Transform[] arr = currentMainFloor.parent.GetComponentsInChildren<Transform>();

            globalUtils.getInstance().filterSpecifiedList<Transform, string, Transform[], string>(
                // globalUtils.getInstance().filterFunctionOfListCustom<string, string>,
                filterBuildingTag<string, Transform, object>, 
                arr,
                BuildingTag,
                (res) => {
                    // Debug.Log("filterSpecifiedList res ==" + res);
                    globalUtils.getInstance().filterSpecifiedList<string, BuildingInfoClass, List<string>,  string>(
                        largefloorF<BuildingInfoClass, string, object>,
                        res,
                        FloorNumber,
                        (BuildingInfoClassList) => {
                            // Debug.Log("res22 res22 ==" + res22.Count);
                            // res22.ForEach((item) => {
                            //     Debug.Log("item.FloorRelativePositionMark ==" + (int)(item.FloorRelativePositionMark));
                            //     Debug.Log("item.BuildingTag ==" + (item.BuildingTag as string));
                            //     // item.SetActive(false);
                            // });

                            // 通知 view 显示隐藏 
                            buildingView.getInstance().HiddenOrShowBuilding<List<BuildingInfoClass>>(BuildingInfoClassList);


                        }
                    );

                }
            );

            // List<string> ss  = new List<string>();
            // for(int i = 0; i < arr.Length; i++){
            //     if(arr[i].name.Contains(BuildingTag + "_floor") && arr[i].CompareTag("building")){
            //         // Debug.Log("currentHoverFloor.parent.transform.childCount ==" + arr[i].name);
            //         ss.Add(arr[i].name);
            //         // ss[i] = arr[i].name;
            //     }
            // }
            // Debug.Log("ss ==" + ss);

            // string[] aaarrr = ss.ToArray();

            // globalUtils.getInstance().filterSpecifiedList<string, BuildingInfoClass, string[],  string>(
            //     largefloorF<BuildingInfoClass, string, object>,
            //     aaarrr,
            //     FloorNumber,
            //     (res) => {
            //         Debug.Log("res ==" + res);
            //     }
            // );


            // globalUtils.getInstance().filterSpecifiedList<string, BuildingInfoClass, List<string>,  string>(
            //     largefloorF<BuildingInfoClass, string, object>,
            //     ss,
            //     FloorNumber,
            //     (res) => {
            //         Debug.Log("ss res ==" + res);
            //     }
            // );




        }





        

        // GameObject floor = GameObject.Find(floorName);


        //  globalUtils.getInstance().filterSpecifiedList<string, BuildingInfoClass, string, List<string>,  List<BuildingInfoClass>>(
        //     globalUtils.getInstance().filterFunctionOfListCustom<BuildingInfoClass, string>, 
        //     floorName,
        //     new List<string>(),
        //     () => {
                
        //     }
        // );

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
        if((data as Transform).name.Contains(keyWorld + "_floor") && (data as Transform).CompareTag("building")){
            return (TR)(object)(data as Transform).name;
        }
        return default(TR);
    }


    /// <summary>
    /// 根据 楼层 称获取 以上 楼层
    /// </summary>
    /// <typeparam name="TR">输出类型 BuildingInfoClass</typeparam>
    /// <typeparam name="T">第一个参数类型</typeparam>
    /// <typeparam name="K">第二个参数类型</typeparam>
    /// <param name="data">第一个参数 得到的 是 楼层的名称</param>
    /// <param name="keyWorld">第二个参数 当前楼层</param>
    /// <returns></returns>

    public TR largefloorF1<TR, T, K>(T data, K keyWorld) where TR : new()
    {

        Debug.Log("largefloorF data ==" + data);
        TR res = new TR();
        System.Type TClass = typeof(TR);
        System.Reflection.FieldInfo[] FieldInfoArr = TClass.GetFields();
        foreach (var item in FieldInfoArr)
        {
            item.SetValue(res, TClass.GetMethod("get"+ item.Name)?.Invoke(res, new object[] { data}));

        }
        return (TR)(object)res;
    }
    public TR largefloorF_Default<TR, T, K>(T data, K keyWorld) where TR : new()
    {
        TR res = new TR();

        Debug.Log("int  == "+ int.Parse((data as string).Split("_")[2]) + "== " + int.Parse(keyWorld.ToString()));

        Debug.Log("bool  ==" + (int.Parse((data as string).Split("_")[2]) > int.Parse(keyWorld.ToString()))  );
        if(int.Parse((data as string).Split("_")[2]) <= int.Parse(keyWorld.ToString()) ){
            // Debug.Log("largefloorF default(TR) ==" + (TR)(object)null == null);
            // return (TR)(object)null;
            return default(TR);
        }

        System.Type TClass = typeof(TR);
        System.Reflection.FieldInfo[] FieldInfoArr = TClass.GetFields();
        foreach (var item in FieldInfoArr)
        {
            if(item.ToString() == "BuildingTag"){
                item.SetValue(res, (data as string).Split("_")[0]);
            }

            if(item.ToString() == "FloorName"){
                item.SetValue(res, data);
            }
            if(item.ToString() == "FloorNumber"){
                item.SetValue(res, (data as string).Split("_")[2]);
            }
        }
        // (res as  BuildingInfoClass).floorName = (string)TClass.GetMethod("getfloorName")?.Invoke(res, new object[] { data });
        return (TR)(object)res;
    }

    public TR largefloorF<TR, T, K>(T data, K keyWorld) where TR : new()
    {
        TR res = new TR();
        // Debug.Log("int  == "+ int.Parse((data as string).Split("_")[2]) + "== " + int.Parse(keyWorld.ToString()));
        // Debug.Log("bool  ==" + (int.Parse((data as string).Split("_")[2]) > int.Parse(keyWorld.ToString()))  );
        // int number = int.Parse((data as string).Split("_")[2]) - int.Parse(keyWorld.ToString());
        if(data == null){
             return default(TR);
        }
        System.Type TClass = typeof(TR);
        System.Reflection.FieldInfo[] FieldInfoArr = TClass.GetFields();
        foreach (var item in FieldInfoArr)
        {


            item.SetValue(res, TClass.GetMethod("get"+ item.Name)?.Invoke(res, new object[] { data}));
            // if(item.Name.ToString() == "BuildingTag"){
            //     item.SetValue(res, (data as string).Split("_")[0]);
            // }

            // if(item.Name.ToString() == "FloorName"){
            //     item.SetValue(res, data);
            // }
            // if(item.Name.ToString() == "FloorNumber"){
            //     item.SetValue(res, (data as string).Split("_")[2]);
            // }
            if(item.Name.ToString() == "FloorRelativePositionMark"){
                // Debug.Log("FloorRelativePositionMark =="+ (int.Parse((data as string).Split("_")[2]) - int.Parse(keyWorld.ToString())));
                item.SetValue(res, int.Parse((data as string).Split("_")[2]) - int.Parse(keyWorld.ToString()));
            }
        }
        
        
        return (TR)(object)res;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
