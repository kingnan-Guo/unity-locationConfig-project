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

    public string parentName;

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
public class buildingController : baseManager<buildingController>
{
    private class methodCallStatus{
        public static bool subThreadToManThreadOfBuildingInfo = false;
        public static List<BuildingInfoClass> BuildingInfoClassList = new List<BuildingInfoClass>();
    }

    private GameObject currentGameObject;



    private Material material_back = Resources.Load<Material>("Materials/buildingMaterial/floor_bl");

    Transform currentHoverFloor;

    private Transform currentMainFloor;

    

    public buildingController(){
        MonoManager.getInstance().AddUpdateListener(Update);

        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.MOUSE_MOVE_POSITION_PHYSICS, (res) => {

            // // 如果 鼠标 在 拖动 设备 上  不进行 射线检测
            // if(globalUtils.getInstance().UIElementsBlockRaycast(GameMainManager.GetInstance().UI2dDiviceTagList)){
            //     return;
            // }
            bool flag = false;
            // flag = true;
            // 如果是 web
            if(Application.platform == RuntimePlatform.WebGLPlayer){
                flag = true;
            }
            
            if(res != null && flag){
                if(currentHoverFloor !=null && currentHoverFloor.tag == gloab_TagName.BUILDING){
                    buildingView.getInstance().backToBuildMaterial<GameObject>(currentHoverFloor.gameObject);
                }
                currentGameObject = res;
                currentHoverFloor = res.transform;

                if(currentHoverFloor != null) {
                    if(currentHoverFloor != null){
                
                        while (currentHoverFloor != null && !currentHoverFloor.gameObject.CompareTag(gloab_TagName.BUILDING))
                        {
                            currentHoverFloor =  currentHoverFloor.parent;
                        }

                        // Debug.Log("currentHoverFloor =="+ currentHoverFloor.name + "== ");

                        if(currentHoverFloor != null &&currentHoverFloor.gameObject.CompareTag(gloab_TagName.BUILDING)){
                            
                            buildingView.getInstance().changeToBuildMaterialToSelect<GameObject>(currentHoverFloor.gameObject);
                        }
                        else {
                            // Debug.Log("没有 悬浮在 任何一个建筑物上");      
                            if(currentGameObject != null && currentGameObject.tag == gloab_TagName.BUILDING){
                                buildingView.getInstance().backToBuildMaterial<GameObject>(currentGameObject);
                            }
                        }
                    } else{
                        Debug.Log("没有父级");
                    }
                }
            }

        });


        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.MOUSE_POSITION_PHYSICS, (res) => {
            // Debug.Log("mousePositionPhysics == "+ res.name + "currentHoverFloor =="+  currentHoverFloor);

            if(currentHoverFloor != null){


                if(currentMainFloor == currentHoverFloor){
                    return;
                }


                // Debug.Log("CHANGE_BUILDING_CARD_ACTIVE 顺序不能修改");
                // 顺序不能修改  CHANGE_BUILDING_CARD_ACTIVE 先修改按钮的 样式 然后再去 跳转到到指定的楼层
                EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_BUILDING_CARD_ACTIVE, currentHoverFloor.transform.name.Split("_")[0]);
                EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.CHANGE_FLOOR_CARD_ACTIVE, currentHoverFloor.transform.name);


                // currentMainFloor = currentHoverFloor;
                // FindAllAboutBuilding(currentMainFloor.name);
                // /// 将主场景 设置为 点击的楼层
                // EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.GLOBAL_CURRENT_MAIN_PARENT_TRANSFORM, currentMainFloor.transform);
                EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.SHOW_APPOINT_FLOOR, currentHoverFloor.name);
            }
        });



        /// 展示指定楼层呢
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.SHOW_APPOINT_FLOOR, (res) => {
            Debug.Log("mousePositionPhysics == "+ res + "currentHoverFloor =="+  currentHoverFloor);
            if(true){
                
                currentMainFloor = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP)?.transform.Find(res).transform;
                // Debug.Log("SHOW_APPOINT_FLOOR currentMainFloor =="+ currentMainFloor);
                FindAllAboutBuilding(currentMainFloor.name);
                /// 将主场景 设置为 点击的楼层
                EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.GLOBAL_CURRENT_MAIN_PARENT_TRANSFORM, currentMainFloor.transform);
            
                // 切换到 楼层 后 将卡片 转换到 该 楼层并且 设置 楼幢 和 楼层的 active;


     
            
            }
        });


        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.CHANGE_MAIN_BUILDING, (Resolution) =>{
            if(currentMainFloor != null){
                if(Resolution == currentMainFloor.name.Split("_")[0]){
                    return;
                }
            }
            // Debug.Log("Resolution ==="+ Resolution);
            // Debug.Log("gloab_EventCenter_Name.CHANGE_MAIN_BUILDING =="+ currentMainFloor);
            reductionFunction(Resolution);
        });

        // 显示地下室
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.SWITCH_UNDER_GROUND, (res) =>{
            showUnderGroundBuilding(res);
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
    public void FindAllAboutBuilding(string floorName){
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
            // Debug.Log("currentMainFloor.name ==" + currentMainFloor.name);
            // Transform[] arr = currentMainFloor.parent.GetComponentsInChildren<Transform>();

            // GameObject[] arrGameObject = ;
            // Debug.Log("arrGameObject =="+ arrGameObject.Count());
            Transform[] arr =  new Transform[currentMainFloor.parent.childCount];
            // Debug.Log("currentMainFloor.parent.childCount =="+ currentMainFloor.parent.childCount);
            for (int i = 0; i < currentMainFloor.parent.childCount; i++)
            {
                arr[i] = currentMainFloor.parent.GetChild(i).transform;
            }
            // Transform[] arr = GameObject.FindObjectsOfType<Transform>();

            // GameObject[] GameObjectss = currentMainFloor.gameObject.;

            // Debug.Log("currentMainFloor.GameObjectss ==" + GameObjectss.Count());

            // // GameObject[] GameObjectss = UnityEngine.Object.FindObjectsOfType<GameObject>() as GameObject[];

            // Debug.Log("currentMainFloor.parent ==" + currentMainFloor.parent.name);

            // Debug.Log("currentMainFloor.name == arr" + arr.Count());
            // Debug.Log("currentMainFloor GameObjectss =="+ GameObjectss.Count());
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
                            // Debug.Log("BuildingInfoClassList ==" + BuildingInfoClassList.Count);
                            BuildingInfoClass targetBuildingInfo = null;
                            BuildingInfoClassList.ForEach((item) => {
                                if(item.FloorRelativePositionMark == 0){
                                    targetBuildingInfo = item;
                                }
                            });
        

                            // 通知 view 显示隐藏 
                            buildingView.getInstance().HiddenOrShowBuilding<List<BuildingInfoClass>>(BuildingInfoClassList);

                            if(targetBuildingInfo != null){
                                buildingView.getInstance().changeCameraPosition<BuildingInfoClass>(targetBuildingInfo);
                            }



                        }
                    );

                }
            );

            // List<string> ss  = new List<string>();
            // for(int i = 0; i < arr.Length; i++){
            //     if(arr[i].name.Contains(BuildingTag + "_floor") && arr[i].CompareTag(gloab_TagName.BUILDING)){
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
        if((data as Transform).name.Contains(keyWorld + "_floor") && (data as Transform).CompareTag(gloab_TagName.BUILDING)){
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

        // Debug.Log("largefloorF data ==" + data);
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

        // Debug.Log("int  == "+ int.Parse((data as string).Split("_")[2]) + "== " + int.Parse(keyWorld.ToString()));

        // Debug.Log("bool  ==" + (int.Parse((data as string).Split("_")[2]) > int.Parse(keyWorld.ToString()))  );
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
    private void Update()
    {
        if(methodCallStatus.subThreadToManThreadOfBuildingInfo){
            methodCallStatus.subThreadToManThreadOfBuildingInfo = false;
            subThreadToManThreadOfBuildingInfo();
        }
    }



    // 过滤所有的楼层数据
    



    // / 多线程 ======================
    /// 更换 楼幢 后  前一个 楼幢 的 漏测还原  多线程================================== 不知是否支持 web 端

    /// <summary>
    /// 首先判断 哪些 楼层要还原;
    /// 例如 点击 的A栋 后 点击B 栋 那么 所有的 楼层 全部显示
    /// </summary>
    public void reductionFunction(string name){

        // Loom.RunAsync(delegate {
        //     Debug.Log("reductionFloor asdasd");
        // });

        Debug.Log("reductionFloor currentMainFloor =="+ currentMainFloor);
        // Transform transform = currentMainFloor;
        string currentMainFloorName = null;
        if(currentMainFloor != null){
            currentMainFloorName  = currentMainFloor.name;
        }

        currentMainFloor = null;
        
        // // 主线程函数 web 端 使用
        if(Application.platform == RuntimePlatform.WebGLPlayer){
            filterReductionFloor(currentMainFloorName);
            return;
        }


        // 多线程 
        globalUtils.getInstance().creatThreadingPool(() =>{
            Debug.Log("reductionFloor  = ="+ currentMainFloorName);
            // Debug.Log("transform =123="+ transform);
            filterReductionFloor(currentMainFloorName);
            // Transform target = GameObject.Find(currentMainFloorName);
            // Debug.Log("target target ==="+ GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP));
        });
    }


    public void filterReductionFloor(string name){

        if(name != null){
            // Debug.Log("filterReductionFloor  name =="+ name);

            // 解析字符串 用于 过滤 当前的 楼层 的

            string[] arr = name.Split("_");

            string buildingTag = arr[0];

            floorList[] floorList = (floorList[])(((buildingInfo)gloab_static_data.buildingDictionary[buildingTag]).floorList);

            globalUtils.getInstance().filterSpecifiedList<floorList, BuildingInfoClass, floorList[],  string>(
                floorListToBuildingInfoClassList<BuildingInfoClass, floorList, object>,
                floorList,
                "100",
                (BuildingInfoClassList) => {
                    // Debug.Log("assas BuildingInfoClassList =="+ BuildingInfoClassList.Count());
                    // 通知 view 显示隐藏 
                    // Debug.Log("assas BuildingInfoClassList =="+ BuildingInfoClassList.Count());
                    // BuildingInfoClassList.ForEach((item) => {
                    //     Debug.Log("assas item =="+ item.FloorName);
                    // });


                    // SUB_THREAD_TO_MAN_THREAD_OF_BUILDING_INFO
                    // 通过事件委托 将子线程的数据 传递给 主线程 但是不能用
                    // EventCenterOptimize.getInstance().EventTrigger<List<BuildingInfoClass>>(gloab_EventCenter_Name.SUB_THREAD_TO_MAN_THREAD_OF_BUILDING_INFO, BuildingInfoClassList);
                   
                   
                   
                    methodCallStatus.BuildingInfoClassList = BuildingInfoClassList;
                    methodCallStatus.subThreadToManThreadOfBuildingInfo = true;


                    // buildingView.getInstance().HiddenOrShowBuilding<List<BuildingInfoClass>>(BuildingInfoClassList);

                    
                }
            );
        }


    }
    /// <summary>
    /// 过滤 数据的方法
    /// </summary>
    /// <typeparam name="TR"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="data"></param>
    /// <param name="keyWorld"></param>
    /// <returns></returns>
    public TR floorListToBuildingInfoClassList<TR, T, K>(T data, K keyWorld) where TR : new()
    {
        floorList datafloorList =  data as floorList;
        TR res = new TR();
        if(datafloorList.name == null){
             return default(TR);
        }
        System.Type TClass = typeof(TR);
        System.Reflection.FieldInfo[] FieldInfoArr = TClass.GetFields();
        foreach (var item in FieldInfoArr)
        {

            item.SetValue(res, TClass.GetMethod("get"+ item.Name)?.Invoke(res, new object[] { datafloorList.name}));
            if(item.Name.ToString() == "FloorRelativePositionMark"){
                // Debug.Log("FloorRelativePositionMark =="+ (int.Parse((data as string).Split("_")[2]) - int.Parse(keyWorld.ToString())));
                item.SetValue(res, int.Parse((datafloorList.name as string).Split("_")[2]) - int.Parse(keyWorld.ToString()));
            }
        }
        
        return (TR)(object)res;
    }

    /// <summary>
    /// subThreadToManThreadOfBuildingInfo 子线程 切回 主线程 后 调用 函数 把数据传给 页面
    /// </summary>
    private void subThreadToManThreadOfBuildingInfo(){
        List<BuildingInfoClass> BuildingInfoClassList = methodCallStatus.BuildingInfoClassList;
        // EventCenterOptimize.getInstance().EventTrigger<List<BuildingInfoClass>>(gloab_EventCenter_Name.SUB_THREAD_TO_MAN_THREAD_OF_BUILDING_INFO, BuildingInfoClassList);
        buildingView.getInstance().HiddenOrShowBuilding<List<BuildingInfoClass>>(BuildingInfoClassList);

        // currentMainFloor = null;
    }



    /// 协程 =============
    /// 


    // 地下室 定制
    public void showUnderGroundBuilding(string flag){
        // 获取所有 的GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP) 的子节点
        Transform mainMapTransform = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP).transform;
        for (int i = 0; i < mainMapTransform.childCount; i++)
        {
            if(mainMapTransform.GetChild(i).tag == gloab_TagName.BUILDING){
                if(mainMapTransform.GetChild(i).gameObject.activeInHierarchy == true){
                    mainMapTransform.GetChild(i).gameObject.SetActive(false);
                } else{
                    mainMapTransform.GetChild(i).gameObject.SetActive(true);
                }
                // mainMapTransform.GetChild(i).gameObject.SetActive(!GameMainManager.GetInstance().underGroundStatus);
                
            }
        }
        if(mainMapTransform.Find("dimian").gameObject.activeInHierarchy == true){
            mainMapTransform.Find("dimian").gameObject.SetActive(false);
        } else {
            mainMapTransform.Find("dimian").gameObject.SetActive(true);
        }
        // mainMapTransform.Find("dimian").gameObject.SetActive(!GameMainManager.GetInstance().underGroundStatus);
        
    }


}
