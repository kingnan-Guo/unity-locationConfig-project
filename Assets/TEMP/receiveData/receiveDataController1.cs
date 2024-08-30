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

/// <summary>
/// 接收数据库相关的数据 进行处理
/// </summary>
public class receiveDataController1 : baseManager<receiveDataController>
{

    private string _databaseTabelName = "deviceInfoData";


    public receiveDataController1(){
        listenReceiveData();

        SQLiteSigleManager.getInstance().CreateTable<deviceInfoData>();// 创建表
        // SQLiteSigleManager.getInstance().getDataList<deviceInfoData>();// 获取所有数据据

        
        // getDataList();

        // ThreadingTest();
    }


    public void changeDataBaseConfig(string databaseTabelName){
        _databaseTabelName = databaseTabelName;
    }


    // 监听接收数据
    public void listenReceiveData(){
        EventCenterOptimizes.getInstance().AddEventListener<GameObject,string>(gloab_EventCenter_Name.DONE_UPDATE_MODEL, (res, className) => {
            Transform[] transforms = new Transform[]{
                res.transform
            };
            preProcessing(transforms, className, "SelectAndUpDate");
        });
        EventCenterOptimizes.getInstance().AddEventListener<Transform[],string>(gloab_EventCenter_Name.DONE_DELETE_MODEL, (transforms, className) => {
            preProcessing(transforms, className, "DeleteData");
        });


        // DATABASE_OPERATE_END
        EventCenterOptimizes.getInstance().AddEventListener<string, string>(gloab_EventCenter_Name.DATABASE_OPERATE_END, (operate, tabelName) => {
            Debug.Log("DATABASE_OPERATE_END = " + operate);
    
        });
        
    }




    // public List<BaseData> assembleData(Transform data){
        
    //     return null;
    // }
    public T assembleData<T>(Transform data){
        Debug.Log("assembleData ======= ");

        // T instantiateClassData = instantiateClass<T>();
        // List<BaseData> list = new List<BaseData>();

        if(typeof(T).Name == "deviceInfoData"){
           return (T)(object)assembleDataDeviceInfoData(data);
        }
        //  这里会在 调用的时候 会去  前端存储数据的 类中 获取数据
        return default(T);
    }



    private T instantiateClass<T>(){
        return (T)System.Activator.CreateInstance(typeof(T));
    }


    /// <summary>
    /// 创建事件参数
    /// </summary>
    /// <param name="list"></param>
    /// <param name="operate"></param>
    /// <param name="className"></param>
    /// <returns></returns>
    public MyEventArgs<string, List<BaseData>> createMyEventArgs(List<BaseData> list, string operate, string className, string keyName="imei"){
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>(className, list, keyName);
        return args;
    }

    /// <summary>
    /// 数据库操作
    /// </summary>
    /// <param name="list"></param>
    /// <param name="operate"></param>
    /// <param name="className"></param>
    public void databaseOperate(List<BaseData> list, string operate, string className){
        MyEventArgs<string, List<BaseData>> args = createMyEventArgs(list, operate, className);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, operate, args);
    }






    public void preProcessing(Transform[] TransformArr, string className, string operate = "SelectAndUpDate")
    {
        // 数据库查询imei 有没有 有更新 没有添加
        // 查询数据库
        List<BaseData>  SelectListBaseData = new List<BaseData>();
        List<BaseData>  UpDateListBaseData = new List<BaseData>();

        // 通过名称 获取 类 className 
        Type classType = Type.GetType(className);
        // Debug.Log("preProcessing classType == "+ classType);

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
                var deviceInfoDataListInstance = (listInstance as deviceInfoData);
                deviceInfoDataListInstance.imei = res.name;



                SelectListBaseData.Add(deviceInfoDataListInstance);
                if(operate == "SelectAndUpDate"){
                    // UpDateListBaseData = assembleDataDeviceInfoData(res);
                    // UpDateListBaseData = (List<BaseData>)assembleDataGeneric.Invoke(this, new object[] {res});
                    UpDateListBaseData.Add(assembleDataGeneric.Invoke(this, new object[] {res}) as deviceInfoData);
                }
                
                if(operate == "DeleteData"){
                    // UpDateListBaseData = assembleDataDeviceInfoData(res);
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
        MyEventArgs<string, List<BaseData>> args = createMyEventArgs(SelectListBaseData, "Select", databaseTabelName);
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
        databaseOperate(UpDateListBaseData, "InsertOrUpdate", databaseTabelName);

        

        
        // foreach (deviceInfoData item in UpDateListBaseData)
        // {
            
        // }

        // for (int i = 0; i < UpDateListBaseData.Count(); i++)
        // {
        //     Debug.Log("UpDateListBaseData ==" +UpDateListBaseData[i]);
        // }

        Debug.Log("==="+ UpDateListBaseData.Count());


        List<networkDeviceDataInfo> networkDeviceDataInfosList = new List<networkDeviceDataInfo>();
        foreach (deviceInfoData item in UpDateListBaseData.Cast<deviceInfoData>().ToList())
        {
            // Debug.Log("networkDeviceDataInfo =="+ item.imei + "== =="+ item.position);
            // Debug.Log("networkDeviceDataInfo =="+ item.imei + "== =="+ item.scale);
            // Debug.Log("networkDeviceDataInfo =="+ item.imei + "== =="+ item.rotate);
            // Debug.Log("networkDeviceDataInfo =="+ item.imei + "== =="+ item.modelType);
            // Debug.Log("networkDeviceDataInfo =="+ item.imei + "== =="+ item.modelTypeName);

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
        Debug.Log("jsonParams =="+ json);
        receiveDataFromNetworkController.getInstance().positionInfo(json.ToString(), () => {

        });
        

    }




    public async void getDataList(){
        await Task.Delay(3000);
        IEnumerable<object> list = SQLiteSigleManager.getInstance().getALLDataList<deviceInfoData>();
        Debug.Log("IEnumerable list == "+ list.Count());
        foreach (var item in list)
        {
            deviceInfoData  data = (item as deviceInfoData);
            data.localPosition = new Vector3((float)data.positionX, (float)data.positionY, (float)data.positionZ);
            data.localRotate = new Vector3((float)data.rotateX, (float)data.rotateY, (float)data.rotateZ);
            data.localScale = new Vector3((float)data.scaleX, (float)data.scaleY, (float)data.scaleZ);
        }

        receiveDataView.getInstance().AddModelToSecene<deviceInfoData>(list);

    }
        

    public IEnumerable<object> SelectData<T>() where T : BaseDataClass
    {
        IEnumerable<object> list = SQLiteSigleManager.getInstance().getALLDataList<T>();
        return list;
    }

    // 吸顶
    public void deviceCeiling(){
        // 获取当前点位的parent 是不是楼层 如果是 或者地下室 那么有吸顶 其他没有吸顶功能
        

    }


    public void DeleteData<T>(List<BaseData> SelectListBaseData, List<BaseData> UpDateListBaseData,  string databaseTabelName){
        // string databaseTabelName 
        // // 获取当前 编辑的点位 
        // List<BaseData> DeleteDataInfoData = new List<BaseData>();
        // foreach (Transform item in GameMainManager.GetInstance().currentAxisParentList)
        // {
        //     deviceInfoData deviceInfoDataSingle = new deviceInfoData();
        //     deviceInfoDataSingle.imei = item.name;
        //     DeleteDataInfoData.Add(deviceInfoDataSingle);
        // }

        // MyEventArgs<string, List<BaseData>> args = createMyEventArgs(SelectListBaseData, "Select", databaseTabelName);
        // IEnumerable<object> data = SQLiteSigleManager.getInstance().IOData("DeleteByIMEI", args);
        // Debug.Log("IEnumerable data == "+ data.Count());


        // string json = globalUtils.getInstance().dataToJson<>();
        // UpDateListBaseData
        // string[] = 
        List<string> data = new List<string>();
        foreach (deviceInfoData item in UpDateListBaseData.Cast<deviceInfoData>().ToList())
        {
            data.Add(item.imei);
        }
        string json = globalUtils.getInstance().dataToJson<List<string>>(data);
        Debug.Log("DeleteData  =="+ json);


        // IEnumerable<object> list = SQLiteSigleManager.getInstance().getDataList<deviceInfoData>();
        // Debug.Log("IEnumerable list == "+ list.Count());
        // foreach (var item in list)
        // {
        //     deviceInfoData  data = (item as deviceInfoData);
        //     data.localPosition = new Vector3((float)data.positionX, (float)data.positionY, (float)data.positionZ);
        //     data.localRotate = new Vector3((float)data.rotateX, (float)data.rotateY, (float)data.rotateZ);
        //     data.localScale = new Vector3((float)data.scaleX, (float)data.scaleY, (float)data.scaleZ);
        // }
        // receiveDataView.getInstance().AddModelToSecene<deviceInfoData>(list);
        // 调取 编辑接口
        receiveDataFromNetworkController.getInstance().delete(json.ToString(), () => {
            databaseOperate(UpDateListBaseData, "DeleteByIMEI", databaseTabelName);
            receiveDataView.getInstance().DeleteModelOfSence<deviceInfoData>(UpDateListBaseData, "imei");
        });



    }

















    //  ================== DeviceInfoData 定制 =============

    public void preProcessingDeviceInfoData(Transform res, string className, string operate = "selectAndUpDateDeviceInfoData")
    {

        operate = "selectAndUpDateDeviceInfoData";
        if(className == "deviceInfoData" && operate == "selectAndUpDateDeviceInfoData"){
            //databaseOperate(listSingle, "SelectBySql", _databaseTabelName);
            // databaseOperate(listSingle, "Select", _databaseTabelName);
            // SQLiteSigleManager.getInstance().OnSelectData += (res) => {};
            List<BaseData> SelectListBaseDatadeviceInfoData = new List<BaseData>();
            deviceInfoData deviceInfoDataSingle = new deviceInfoData();
            deviceInfoDataSingle.imei = res.name;
            SelectListBaseDatadeviceInfoData.Add(deviceInfoDataSingle);
            // selectAndUpDateDeviceInfoData(SelectListBaseDatadeviceInfoData, assembleDataDeviceInfoData(res), className);
        }
    }


    // DeviceInfoData selectAndUpDate
    public void selectAndUpDateDeviceInfoData(List<BaseData> SelectListBaseData, List<BaseData> UpDateListBaseData, string databaseTabelName){
        MyEventArgs<string, List<BaseData>> args = createMyEventArgs(SelectListBaseData, "Select", databaseTabelName);
        IEnumerable<object> data = SQLiteSigleManager.getInstance().IOData("Select", args);
        // Debug.Log("IEnumerable data == "+ data.Count());
        if(data.Count() == 0){
            databaseOperate(UpDateListBaseData, "Insert", databaseTabelName);
        }else{
            databaseOperate(UpDateListBaseData, "UpDateByIMEI", databaseTabelName);
        }
    }


    // DeviceInfoData 数据组装
    public deviceInfoData assembleDataDeviceInfoData(Transform data){

        // Vector3 rotate = new Vector3(data.rotation.x, data.rotation.y, data.rotation.z);
        // Quaternion q = Quaternion.Euler(rotate);
        // Debug.Log("assembleDataDeviceInfoData rotate == "+ data.rotation);

        networkDeviceDataInfo networkDeviceDataInfo = deviceListController.getInstance().getDeviceInfoByIMEI(data.name);
        Debug.Log("networkDeviceDataInfo =="+ networkDeviceDataInfo);
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

        // deviceInfoData = otherTempTest.getInstance().deviceInfoToDeviceInfoData(deviceInfoData);

        
        // Debug.Log("aacc == "+ aacc.deviceName + "  imei =="+ aacc.imei);
        // List<BaseData> list = new List<BaseData>();
        // list.Add(deviceInfoData);
        return deviceInfoData;
    }


    public void postInfo(){


        // receiveDataFromNetworkController.getInstance().positionInfo();
    }




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

            SQLiteSigleManagerDemo.getInstance().InsertOrUpdate();
        }
    }


    
}
