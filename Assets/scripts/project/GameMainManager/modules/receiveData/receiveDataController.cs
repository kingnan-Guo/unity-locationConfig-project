using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 接收数据库相关的数据 进行处理
/// </summary>
public class receiveDataController : baseManager<receiveDataController>
{

    private string _databaseTabelName = "deviceInfoData";


    public receiveDataController(){
        listenReceiveData();

        SQLiteSigleManager.getInstance().CreateTable<deviceInfoData>();// 创建表
        // SQLiteSigleManager.getInstance().getDataList<deviceInfoData>();// 获取所有数据据

        
        getDataList();


    }


    public void changeDataBaseConfig(string databaseTabelName){
        _databaseTabelName = databaseTabelName;
    }


    // 监听接收数据
    public void listenReceiveData(){
        EventCenterOptimizes.getInstance().AddEventListener<GameObject,string>(gloab_EventCenter_Name.DONE_UPDATE_MODEL, (res, className) => {
            preProcessing(res.transform, className);
        });
    }




    // public List<BaseData> assembleData(Transform data){
        
    //     return null;
    // }
    public List<BaseData> assembleData<T>(Transform data){
        Debug.Log("assembleData ======= ");

        // T instantiateClassData = instantiateClass<T>();
        List<BaseData> list = new List<BaseData>();

        if(typeof(T).Name == "deviceInfoData"){
           list = assembleDataDeviceInfoData(data);
        }
        //  这里会在 调用的时候 会去  前端存储数据的 类中 获取数据
        return list;
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






    public void preProcessing(Transform res, string className, string operate = "SelectAndUpDate")
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
        
        if(className == "deviceInfoData"){
            var deviceInfoDataListInstance = (listInstance as deviceInfoData);
            deviceInfoDataListInstance.imei = res.name;



            SelectListBaseData.Add(deviceInfoDataListInstance);
            if(operate == "SelectAndUpDate"){
                // UpDateListBaseData = assembleDataDeviceInfoData(res);
                UpDateListBaseData = (List<BaseData>)assembleDataGeneric.Invoke(this, new object[] {res});
            }
            
            if(operate == "DeleteData"){
                // UpDateListBaseData = assembleDataDeviceInfoData(res);
            }
            // Debug.Log("preProcessing UpDateListBaseData == "+ UpDateListBaseData);
            parameters = new object[] {SelectListBaseData, UpDateListBaseData, className};

        } else {
            // 其他函数 可能传值 不一样
            SelectListBaseData.Add(listInstance as BaseData);

            // ToAddList = assembleData<>(res);
            // assembleData<T>(res)
            parameters = new object[] {};
        }
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
            if((UpDateListBaseData[i] as deviceInfoData).imei== (data.ToList()[i] as deviceInfoData).imei){
                (UpDateListBaseData[i] as deviceInfoData).deviceName = (data.ToList()[i] as deviceInfoData).deviceName;
                (UpDateListBaseData[i] as deviceInfoData).deviceCategory = (data.ToList()[i] as deviceInfoData).deviceCategory;
                (UpDateListBaseData[i] as deviceInfoData).parentModelId = GameMainManager.GetInstance().global_currentMainParent.name;
                (UpDateListBaseData[i] as deviceInfoData).parentModelName = GameMainManager.GetInstance().global_currentMainParent.name;
                (UpDateListBaseData[i] as deviceInfoData).modelTypeName = (data.ToList()[i] as deviceInfoData).modelTypeName;
                (UpDateListBaseData[i] as deviceInfoData).modelType = (data.ToList()[i] as deviceInfoData).modelType;
            };
            // Debug.Log(" =="+ (data.ToList()[i] as deviceInfoData).imei);
            // Debug.Log("data =="+ data.ToList()[i]);
        }
        databaseOperate(UpDateListBaseData, "InsertOrUpdate", databaseTabelName);

        networkDeviceDataInfo networkDeviceDataInfo = new networkDeviceDataInfo();
        networkDeviceDataInfo.deviceName = "asd";
        networkDeviceDataInfo.position = new position(){
            positionX = 1,
            positionY = 2,
            positionZ = 2,
        };

        var serializer = new DataContractJsonSerializer(typeof(networkDeviceDataInfo));
        Debug.Log("serializer ==="+ serializer);
        var stream = new MemoryStream();
        Debug.Log("stream 1 ==="+ stream);
        serializer.WriteObject(stream, networkDeviceDataInfo);
          Debug.Log("stream 2 ==="+ stream);  
        string json = Encoding.UTF8.GetString(stream.ToArray());

        Debug.Log("json ==="+ json);

        Debug.Log("json ==="+ json.ToString());
        
        string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0,\"deviceCategory\": 0,\"isLabeled\": 1}";

        Debug.Log("jsonParams =="+ jsonParams);

    }




    public async void getDataList(){
        await Task.Delay(3000);
        IEnumerable<object> list = SQLiteSigleManager.getInstance().getDataList<deviceInfoData>();
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
        IEnumerable<object> list = SQLiteSigleManager.getInstance().getDataList<T>();
        return list;
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
            selectAndUpDateDeviceInfoData(SelectListBaseDatadeviceInfoData, assembleDataDeviceInfoData(res), className);
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
    public List<BaseData> assembleDataDeviceInfoData(Transform data){
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
            rotateX = data.localRotation.x,
            rotateY = data.localRotation.y,
            rotateZ = data.localRotation.z,
        };

        deviceInfoData = otherTempTest.getInstance().deviceInfoToDeviceInfoData(deviceInfoData);
        // Debug.Log("aacc == "+ aacc.deviceName + "  imei =="+ aacc.imei);
        List<BaseData> list = new List<BaseData>();
        list.Add(deviceInfoData);
        return list;
    }


    public void postInfo(){


        // receiveDataFromNetworkController.getInstance().positionInfo();
    }



    
}
