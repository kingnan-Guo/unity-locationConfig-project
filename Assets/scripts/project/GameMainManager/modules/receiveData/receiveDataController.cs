using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 接收数据库相关的数据 进行处理
/// </summary>
public class receiveDataController : baseManager<receiveDataController>
{

    private string _databaseTabelName = "deviceInfoData";

    private class methodCallStatus{
        public static bool getAllIsLabeledDataListFromNetwork = false;
        public static bool getDataListFromeDB =  false;
    }


    public receiveDataController(){
        MonoManager.getInstance().AddUpdateListener(Update);

        receiveDataFromNetworkController.getInstance();
        listenReceiveData();

        SQLiteSigleManager.getInstance().CreateTable<deviceInfoData>();// 创建表
        // SQLiteSigleManager.getInstance().getDataList<deviceInfoData>();// 获取所有数据据

        // seletDeviceListFromNetwork("1000014");
 
        
    }


    public void changeDataBaseConfig(string databaseTabelName){
        _databaseTabelName = databaseTabelName;
    }


    // 监听接收数据
    public void listenReceiveData(){
        // 新增 更新 
        EventCenterOptimizes.getInstance().AddEventListener<GameObject,string>(gloab_EventCenter_Name.DONE_UPDATE_MODEL, (res, className) => {
            Transform[] transforms = new Transform[]{
                res.transform
            };
            preProcessing(transforms, className, "SelectAndUpDate");
        });

        // 监听删除
        EventCenterOptimizes.getInstance().AddEventListener<Transform[],string>(gloab_EventCenter_Name.DONE_DELETE_MODEL, (transforms, className) => {
            preProcessing(transforms, className, "DeleteData");
        });

        // DATABASE_OPERATE_END 数据库操作完成 监听
        EventCenterOptimize.getInstance().AddEventListener<MyEventArgs<string, object>>(gloab_EventCenter_Name.DATABASE_OPERATE_END, (data) => {
            // Debug.Log("DATABASE_OPERATE_END = " + data.METHODNAME);
            // 根据不同的 方法名 进行不同的操作

            // getAllIsLabeledDataListFromNetwork 获取完 所有 数据 ，然后 数据库 开始获取数据
            if(data.METHODNAME == "getAllIsLabeledDataListFromNetwork"){
                // getDataListFromeDB();
                methodCallStatus.getDataListFromeDB = true; // 修改 参数 切换到 主线程 渲染数据

                
            }
        });


        // 地图加载完成 开始获取 数据
        EventCenterOptimize.getInstance().AddEventListener<string>(gloab_EventCenter_Name.MAIN_MAP_LOAD_DONE, (res) =>{
            // 第一步获取 所有的 已经拖拽的 设备 入库 
            getAllIsLabeledDataListFromNetwork();

            // 第二步 将获取到的 设备 渲染到 地图上
            // getDataListFromeDB();

            

        });
    }


    public T assembleData<T>(Transform data){
        // Debug.Log("assembleData ======= ");
        if(typeof(T).Name == "deviceInfoData"){
           return (T)(object)assembleDataDeviceInfoData(data);
        }
        //  这里会在 调用的时候 会去  前端存储数据的 类中 获取数据
        return default(T);
    }

    public T assemblePositionData<T>(Transform data){
        Type classType = typeof(T);
        var listInstance = Activator.CreateInstance(classType);
        var deviceInfoDataListInstance = (listInstance as deviceInfoData);
        deviceInfoDataListInstance.positionX = data.localPosition.x;
        deviceInfoDataListInstance.positionY = data.localPosition.y;
        deviceInfoDataListInstance.positionZ = data.localPosition.z;
        deviceInfoDataListInstance.scaleX = data.localScale.x;
        deviceInfoDataListInstance.scaleY = data.localScale.x;
        deviceInfoDataListInstance.scaleZ = data.localScale.z;
        deviceInfoDataListInstance.rotateX = data.eulerAngles.x;
        deviceInfoDataListInstance.rotateY = data.eulerAngles.y;
        deviceInfoDataListInstance.rotateZ = data.eulerAngles.z;
        return (T)(object)listInstance;
    }





    /// <summary>
    /// 创建事件参数
    /// </summary>
    /// <param name="list"></param>
    /// <param name="operate"></param>
    /// <param name="className"></param>
    /// <returns></returns>
    // public MyEventArgs<string, List<BaseData>> createMyEventArgs(List<BaseData> list, string operate, string className, string keyName="imei", string MethodName = null){
    //     MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>(className, list, keyName, MethodName: MethodName);
    //     return args;
    // }

    /// <summary>
    /// 数据库操作
    /// </summary>
    /// <param name="list"></param>
    /// <param name="operate"></param>
    /// <param name="className"></param>
    public void databaseOperate(List<BaseData> list, string operate, string className, string extendData = null,string MethodName = null){
        MyEventArgs<string, List<BaseData>> args = globalUtils.getInstance().createMyEventArgs(list, operate, className, extendData: extendData,MethodName:MethodName);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, operate, args);
    }





    /// <summary>
    /// 数据预处理  基本归一化 处理
    /// </summary>
    /// <param name="TransformArr"></param>
    /// <param name="className"></param>
    /// <param name="operate"></param>
    public void preProcessing(Transform[] TransformArr, string className, string operate = "SelectAndUpDate")
    {
        // 数据库查询imei 有没有 有更新 没有添加
        // 查询数据库
        List<BaseData>  SelectListBaseData = new List<BaseData>();
        List<BaseData>  UpDateListBaseData = new List<BaseData>();

        // 通过名称 获取 类 className 
        Type classType = Type.GetType(className);

        var listInstance = Activator.CreateInstance(classType);

        MethodInfo method = typeof(receiveDataController).GetMethod(operate); // 获取方法信息
        MethodInfo generic = method.MakeGenericMethod(classType); // 创建一个具体的泛型方法
        // 搜索的条件赋值
        // Debug.Log("preProcessing listInstance == "+ listInstance);

        MethodInfo assembleDataMethod = typeof(receiveDataController).GetMethod("assembleData"); // 获取方法信息
        MethodInfo assembleDataGeneric = assembleDataMethod.MakeGenericMethod(classType);

        object[] parameters;

        foreach (Transform res in TransformArr)
        {
            if(className == "deviceInfoData"){
                // var deviceInfoDataListInstance = assemblePositionData<deviceInfoData>(res);
                var deviceInfoDataListInstance = (listInstance as deviceInfoData);
                deviceInfoDataListInstance.imei = res.name;

                SelectListBaseData.Add(deviceInfoDataListInstance);
                if(operate == "SelectAndUpDate"){
                    // UpDateListBaseData = (List<BaseData>)assembleDataGeneric.Invoke(this, new object[] {res});
                    UpDateListBaseData.Add(assembleDataGeneric.Invoke(this, new object[] {res}) as deviceInfoData);
                }
                
                if(operate == "DeleteData"){
                    UpDateListBaseData.Add(deviceInfoDataListInstance);
                }
                // Debug.Log("preProcessing UpDateListBaseData == "+ UpDateListBaseData);

            } else {
                // 其他函数 可能传值 不一样
                SelectListBaseData.Add(listInstance as BaseData);
                // ToAddList = assembleData<>(res);
                // assembleData<T>(res)
                parameters = new object[] {};
            }
        }
        parameters = new object[] {SelectListBaseData, UpDateListBaseData, className};
        generic.Invoke(this, parameters);
    }


    public void SelectAndUpDate<T>(List<BaseData> SelectListBaseData, List<BaseData> UpDateListBaseData,  string databaseTabelName){

        // SelectAndUpDate2(SelectListBaseData, UpDateListBaseData, databaseTabelName);
        // return;
        MyEventArgs<string, List<BaseData>> args = globalUtils.getInstance().createMyEventArgs(SelectListBaseData, "Select", databaseTabelName);
        IEnumerable<object> data = SQLiteSigleManager.getInstance().IOData("Select", args);
        Debug.Log("IEnumerable data == "+ data.Count());

        // if(data.Count() == 0){
        //     databaseOperate(UpDateListBaseData, "Insert", databaseTabelName);
        // }else{
        //     // 如果有数据 更新
        //     databaseOperate(UpDateListBaseData, "UpDateByIMEI", databaseTabelName);
        // }
        
        for (int i = 0; i < data.Count(); i++)
        {
            if((UpDateListBaseData[i] as deviceInfoData).imei == (data.ToList()[i] as deviceInfoData).imei){
                (UpDateListBaseData[i] as deviceInfoData).deviceName = (data.ToList()[i] as deviceInfoData).deviceName;
                (UpDateListBaseData[i] as deviceInfoData).deviceCategory = (data.ToList()[i] as deviceInfoData).deviceCategory;
                (UpDateListBaseData[i] as deviceInfoData).parentModelId = (data.ToList()[i] as deviceInfoData).parentModelId;
                (UpDateListBaseData[i] as deviceInfoData).parentModelName = (data.ToList()[i] as deviceInfoData).parentModelId;
                (UpDateListBaseData[i] as deviceInfoData).modelTypeName = (data.ToList()[i] as deviceInfoData).modelTypeName;
                (UpDateListBaseData[i] as deviceInfoData).modelType = (data.ToList()[i] as deviceInfoData).modelType;
            };
            // Debug.Log(" =="+ (data.ToList()[i] as deviceInfoData).imei);
            // Debug.Log("data =="+ data.ToList()[i]);
        }
        databaseOperate(UpDateListBaseData, "InsertOrUpdate", databaseTabelName, extendData: "imei",MethodName:"InsertOrUpdate");




        List<networkDeviceDataInfo> networkDeviceDataInfosList = new List<networkDeviceDataInfo>();
        foreach (deviceInfoData item in UpDateListBaseData.Cast<deviceInfoData>().ToList())
        {
            Debug.Log("JsonUtility.ToJson  item =="+ JsonUtility.ToJson(item));
            networkDeviceDataInfo networkDeviceDataInfo = new networkDeviceDataInfo();
            networkDeviceDataInfo.deviceName = item.deviceName;
            networkDeviceDataInfo.imei = item.imei;
            networkDeviceDataInfo.deviceCategory = (long)item.deviceCategory;
            networkDeviceDataInfo.modelType = item.modelType;
            networkDeviceDataInfo.modelTypeName = item.modelTypeName;
            networkDeviceDataInfo.parentModelId = item.parentModelId;
            networkDeviceDataInfo.isLabeled = 1;
            networkDeviceDataInfo.position = new position(){
                positionX = (float)item.positionX,
                positionY = (float)item.positionY,
                positionZ = (float)item.positionZ,
            };
            networkDeviceDataInfo.scale = new scale(){
                scaleX = (float)item.scaleX,
                scaleY = (float)item.scaleY,
                scaleZ = (float)item.scaleZ,
            };
            networkDeviceDataInfo.rotate = new rotate(){
                rotateX = (float)item.rotateX,
                rotateY = (float)item.rotateY,
                rotateZ = (float)item.rotateZ,
            };
            networkDeviceDataInfosList.Add(networkDeviceDataInfo);
        }
        
        string json = globalUtils.getInstance().dataToJson<List<networkDeviceDataInfo>>(networkDeviceDataInfosList);
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0,\"isLabeled\": 1}";
        // Debug.Log("jsonParams =="+ json);
        receiveDataFromNetworkController.getInstance().positionInfo(json.ToString(), () => {

            // databaseOperate(UpDateListBaseData, "InsertOrUpdate", databaseTabelName, MethodName:"InsertOrUpdate");


            EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.REFRESH_CANVAS_DEVICE_LIST, "string");
            
        });

    }

    //  start *************由于没办法 通过 imei 查到 数据所以 此函数 不生效
    public async void SelectAndUpDate2(List<BaseData> SelectListBaseData, List<BaseData> UpDateListBaseData,  string databaseTabelName){
        // seletDeviceListFromNetwork();
        List<networkDeviceDataInfo> networkDeviceDataInfosList = new List<networkDeviceDataInfo>();
        foreach (deviceInfoData deviceInfoData in SelectListBaseData)
        {
            networkDeviceDataInfo networkDeviceDataInfo = (await seletDeviceListFromNetwork(deviceInfoData.imei))[0];
            if(networkDeviceDataInfo != null){

                networkDeviceDataInfo.parentModelId = string.IsNullOrEmpty(networkDeviceDataInfo.parentModelId) ? GameMainManager.GetInstance().global_currentMainParent.name : networkDeviceDataInfo.parentModelId;
                networkDeviceDataInfo.isLabeled = 1;
                networkDeviceDataInfo.position = new position(){
                    positionX = (float)deviceInfoData.positionX,
                    positionY = (float)deviceInfoData.positionY,
                    positionZ = (float)deviceInfoData.positionZ,
                };
                networkDeviceDataInfo.scale = new scale(){
                    scaleX = (float)deviceInfoData.scaleX,
                    scaleY = (float)deviceInfoData.scaleY,
                    scaleZ = (float)deviceInfoData.scaleZ,
                };
                networkDeviceDataInfo.rotate = new rotate(){
                    rotateX = (float)deviceInfoData.rotateX,
                    rotateY = (float)deviceInfoData.rotateY,
                    rotateZ = (float)deviceInfoData.rotateZ,
                };
                networkDeviceDataInfosList.Add(networkDeviceDataInfo);


                deviceInfoData.deviceName = networkDeviceDataInfo.deviceName;
                deviceInfoData.deviceCategory = networkDeviceDataInfo.deviceCategory;
                deviceInfoData.modelType = networkDeviceDataInfo.modelType;
                deviceInfoData.modelTypeName = networkDeviceDataInfo.modelTypeName;
                deviceInfoData.deviceName = networkDeviceDataInfo.deviceName;
                deviceInfoData.parentModelId = networkDeviceDataInfo.parentModelId;
                deviceInfoData.parentModelName = networkDeviceDataInfo.parentModelId;
            }
        }

        string json = globalUtils.getInstance().dataToJson<List<networkDeviceDataInfo>>(networkDeviceDataInfosList);
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0,\"isLabeled\": 1}";
        Debug.Log("jsonParams =="+ json);
        receiveDataFromNetworkController.getInstance().positionInfo(json.ToString(), () => {
            databaseOperate(SelectListBaseData, "InsertOrUpdate", databaseTabelName);
            
        });

    }
    // *************由于没办法 通过 imei 查到 数据所以 此函数 不生效
    public async Task<List<networkDeviceDataInfo>> seletDeviceListFromNetwork(string imei){

        deviceInfoParams infoParams = new deviceInfoParams();
        infoParams.imei = imei;
        infoParams.deviceCategory = 1;
        List<networkDeviceDataInfo> data = await receiveDataFromNetworkController.getInstance().aysncGetDeviceList(infoParams);
        Debug.Log("seletFromNetwork data == "+ data.Count());

        return data;
    }

      // end *************由于没办法 通过 imei 查到 数据所以 此函数 不生效  



    // 吸顶
    // public void deviceCeiling(){
    //     // 获取当前点位的parent 是不是楼层 如果是 或者地下室 那么有吸顶 其他没有吸顶功能
    // }



    /// <summary>
    /// 删除设备
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="SelectListBaseData"></param>
    /// <param name="UpDateListBaseData"></param>
    /// <param name="databaseTabelName"></param>
    public void DeleteData<T>(List<BaseData> SelectListBaseData, List<BaseData> UpDateListBaseData,  string databaseTabelName){

        List<string> data = new List<string>();
        foreach (deviceInfoData item in UpDateListBaseData.Cast<deviceInfoData>().ToList())
        {
            data.Add(item.imei);
        }
        string json = globalUtils.getInstance().dataToJson<List<string>>(data);
        // Debug.Log("DeleteData  =="+ json);

        // 调取 编辑接口
        receiveDataFromNetworkController.getInstance().delete(json.ToString(), () => {
            databaseOperate(UpDateListBaseData, "DeleteByIMEI", databaseTabelName);
            receiveDataView.getInstance().DeleteModelOfSence<deviceInfoData>(UpDateListBaseData, "imei");
            EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.REFRESH_CANVAS_DEVICE_LIST, "string");
        });



    }







    // =============  获取所有的数据 并且  入库 ===========

    public async void getAllIsLabeledDataListFromNetwork(){
        deviceInfoParams infoParams = new deviceInfoParams();
        infoParams.pageSize = 0;
        infoParams.isLabeled = 1;
        List<networkDeviceDataInfo> data = await receiveDataFromNetworkController.getInstance().aysncGetDeviceList(infoParams);
        // Debug.Log("getAllIsLabeledDataListFromNetwork data == "+ data.Count());
        if(data.Count() > 0){
            // 入库
            reductionFunction(data, "getAllIsLabeledDataListFromNetwork");
            // sendToDatase(data, "getAllIsLabeledDataListFromNetwork");
        }
    }



    // 获取 所有的 数据 并渲染到界面上
    public void getDataListFromeDB(){
        IEnumerable<object> list = SelectData<deviceInfoData>();
        foreach (var item in list)
        {
            deviceInfoData  data = (item as deviceInfoData);
            data.localPosition = new Vector3((float)data.positionX, (float)data.positionY, (float)data.positionZ);
            data.localRotate = new Vector3((float)data.rotateX, (float)data.rotateY, (float)data.rotateZ);
            data.localScale = new Vector3((float)data.scaleX, (float)data.scaleY, (float)data.scaleZ);
        }
        // Debug.Log("getDataListFromeDB ==");
        receiveDataView.getInstance().AddModelToSecene<deviceInfoData>(list);
    }

    /// <summary>
    ///     获取所有的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<object> SelectData<T>() where T : BaseDataClass
    {
        IEnumerable<object> list = SQLiteSigleManager.getInstance().getALLDataList<T>();
        // Debug.Log("IEnumerable data == "+ list.Count());
        return list;
    }

    /// <summary>
    /// 根据条件查询数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public IEnumerable<object> SelectDataByData<T>(T data) where T : BaseDataClass{
        List<BaseData>  SelectListBaseData = new List<BaseData>();
        if(data != null && globalUtils.IsClassValue<T>(data)){
            SelectListBaseData.Add(data);
        }
        MyEventArgs<string, List<BaseData>> args = globalUtils.getInstance().createMyEventArgs(SelectListBaseData, "Select", typeof(T).Name);
        IEnumerable<object> dataList = SQLiteSigleManager.getInstance().IOData("Select", args);
        // Debug.Log("IEnumerable data == "+ dataList.Count());
        return dataList;
    }


    /// <summary>
    /// 根据条件查询数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="SelectListBaseData"></param>
    /// <returns></returns>
    public IEnumerable<object> SelectDataByDataBase<T>(List<BaseData> SelectListBaseData) where T : BaseDataClass{

        MyEventArgs<string, List<BaseData>> args = globalUtils.getInstance().createMyEventArgs(SelectListBaseData, "Select", typeof(T).Name);
        IEnumerable<object> dataList = SQLiteSigleManager.getInstance().IOData("Select", args);
        Debug.Log("IEnumerable data == "+ dataList.Count());
        return dataList;
    }


    //  ================== DeviceInfoData 定制 =============





    // DeviceInfoData 数据组装
    public deviceInfoData assembleDataDeviceInfoData(Transform data){

        networkDeviceDataInfo networkDeviceDataInfo = deviceListController.getInstance().getDeviceInfoByIMEI(data.name);
        // Debug.Log("networkDeviceDataInfo =="+ networkDeviceDataInfo);
        deviceInfoData deviceInfoData = new deviceInfoData(){
            imei = data.name, 
            parentModelId = GameMainManager.GetInstance().global_currentMainParent.name,
            parentModelName = GameMainManager.GetInstance().global_currentMainParent.name,
            positionX = data.position.x, 
            positionY = data.position.y, 
            positionZ = data.position.z,
            scaleX = data.localScale.x,
            scaleY = data.localScale.y,
            scaleZ = data.localScale.z,
            rotateX = data.eulerAngles.x,
            rotateY = data.eulerAngles.y,
            rotateZ = data.eulerAngles.z,

            deviceName = networkDeviceDataInfo.deviceName,
            deviceCategory = networkDeviceDataInfo.deviceCategory,
            modelType = networkDeviceDataInfo.modelType,
            modelTypeName = networkDeviceDataInfo.modelTypeName
        };

        return deviceInfoData;
    }



    // ===================== Update ========

    private void Update(){
        // if(methodCallStatus.getAllIsLabeledDataListFromNetwork){
        //     methodCallStatus.getAllIsLabeledDataListFromNetwork = false;
        // }
        if(methodCallStatus.getDataListFromeDB){
            getDataListFromeDB();
            methodCallStatus.getDataListFromeDB = false;
        }
    }






    // seletByIMEI
    public IEnumerable<object> seletByIMEI(string imei){
        return SQLiteSigleManager.getInstance().seletByIMEI<deviceInfoData>(imei);
    }






    // ============================================================
    // 多线程
    // 获取所有的 已经拖过 的点位
    // 存储到数据库





    // ======================== 


    private static readonly object _lockObject = new object();
    public void ThreadingTest(){
        Thread thread1 = new Thread(new ThreadStart(ExecuteQuery));
        thread1.Start();
    }
    public void ExecuteQuery(){
        lock (_lockObject)
        {
            // Debug.Log("=== ExecuteQuery ====");
            // SQLiteSigleManagerDemo.getInstance().InsertOrUpdate();
        }
    }

    public void reductionFunction(List<networkDeviceDataInfo> newWorkDeviceList, string MethodName= null){
        // 多线程 
        globalUtils.getInstance().creatThreadingPool(() =>{
            // getDeviceList();
            Debug.Log("多线程 个入库 =="+ System.DateTime.Now);
            sendToDatase(newWorkDeviceList, MethodName);
        });
    }

    public void sendToDatase(List<networkDeviceDataInfo> newWorkDeviceList, string MethodName = null){

        // 转成 deviceInfoData 然后 调用 InsertOrReplace
        List<BaseData>  UpDateListBaseData = new List<BaseData>();
        foreach (networkDeviceDataInfo item in newWorkDeviceList)
        {
            deviceInfoData deviceInfoData = new deviceInfoData();
            deviceInfoData.deviceName = item.deviceName;
            deviceInfoData.imei = item.imei;
            deviceInfoData.deviceCategory = item.deviceCategory;
            deviceInfoData.modelType = item.modelType;
            deviceInfoData.modelTypeName = item.modelTypeName;
            deviceInfoData.parentModelId = item.parentModelId;
            deviceInfoData.parentModelName = item.parentModelId;

            deviceInfoData.positionX = (float)item.position.positionX;
            deviceInfoData.positionY = (float)item.position.positionY;
            deviceInfoData.positionZ = (float)item.position.positionZ;

            deviceInfoData.scaleX = (float)item.scale.scaleX;
            deviceInfoData.scaleY = (float)item.scale.scaleY;
            deviceInfoData.scaleZ = (float)item.scale.scaleZ;

            deviceInfoData.rotateX = (float)item.rotate.rotateX;
            deviceInfoData.rotateY = (float)item.rotate.rotateY;
            deviceInfoData.rotateZ = (float)item.rotate.rotateZ;
            UpDateListBaseData.Add(deviceInfoData);
        }
        // Debug.Log("getDeviceList 拿到数据完成数据处理 =="+ System.DateTime.Now);


        // 发送数据给  receiveDataController 进行渲染数据


        // EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(
        //     gloab_EventCenter_Name.DATABASE_OPERATE, 
        //     "InsertOrReplace", 
        //     globalUtils.getInstance().createMyEventArgs(UpDateListBaseData, "InsertOrReplace", "deviceInfoData", MethodName: MethodName)
        // );

        databaseOperate(UpDateListBaseData, "InsertOrReplace", "deviceInfoData", MethodName: MethodName);
    }



    // 测试 10W设备 渲染， 10W 设备 均匀放在 100 * 100 的区域内
    public void test10WDevice(){
        
        List<deviceInfoData> newWorkDeviceList = new List<deviceInfoData>();
        for (int i = 0; i < 10000; i++)
        {
            deviceInfoData networkDeviceDataInfo = new deviceInfoData();
            networkDeviceDataInfo.deviceName = "deviceName"+i;
            networkDeviceDataInfo.localPosition = new Vector3(UnityEngine.Random.Range(0, 200) - 100, UnityEngine.Random.Range(0, 200), UnityEngine.Random.Range(0, 20) - 60);
            networkDeviceDataInfo.localRotate = Vector3.zero;
            networkDeviceDataInfo.localScale = Vector3.one;
            networkDeviceDataInfo.imei = "imei"+i;
            networkDeviceDataInfo.parentModelId = "mainMap(Clone)";
            newWorkDeviceList.Add(networkDeviceDataInfo);
        }
        Debug.Log("test10WDevice =="+ newWorkDeviceList.Count());

        receiveDataView.getInstance().AddModelToSecene<deviceInfoData>(newWorkDeviceList);


    }



    
}


// 当亲页面的主流程逻辑时
// 监听地图加载完成后回去 捞出 所有的 设备数据，存入到库里 完成后
// 从库里获取数据 展示在 界面上