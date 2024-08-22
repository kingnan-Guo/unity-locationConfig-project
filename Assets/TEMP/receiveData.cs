using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenCover.Framework.Model;
using UnityEngine;

/// <summary>
/// 接收数据库相关的数据 进行处理
/// </summary>
public class receiveData : baseManager<receiveData>
{

    private string _databaseTabelName = "deviceInfoData";
    // private Class deviceInfoData = new Class();


    public receiveData(){

        listenReceiveData();
        
    }


    public void changeDataBaseConfig(string databaseTabelName){
        _databaseTabelName = databaseTabelName;
    }


    // 监听接收数据
    public void listenReceiveData(){
        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.DONE_ADD_MODEL_TO_SECENE, (res) => {
            // Debug.Log("gloab_EventCenter_Name.DONE_ADD_MODEL_TO_SECENE =="+ res);

            preProcessing(res.transform);
        });
        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.DONE_UPDATE_MODEL, (res) => {
            // Debug.Log("gloab_EventCenter_Name.DONE_ADD_MODEL_TO_SECENE =="+ res);

            preProcessing(res.transform);
        });
    }


    public void preProcessing(Transform res){
        // 数据库查询imei 有没有 有更新 没有添加
        // 查询数据库
        List<BaseData> listSingle = new List<BaseData>();
        deviceInfoData deviceInfoDataSingle = new deviceInfoData();
        deviceInfoDataSingle.imei = res.name;
        listSingle.Add(deviceInfoDataSingle);

        //databaseOperate(listSingle, "SelectBySql", _databaseTabelName);
        // databaseOperate(listSingle, "Select", _databaseTabelName);
        // SQLiteSigleManager.getInstance().OnSelectData += (res) => {};

        MyEventArgs<string, List<BaseData>> args = createMyEventArgs(listSingle, "Select", _databaseTabelName);
        IEnumerable<object> data = SQLiteSigleManager.getInstance().IOData("Select", args);
        Debug.Log("IEnumerable data == "+ data.Count());
        

        if(data.Count() == 0){
            databaseOperate(assembleData(res), "Insert", _databaseTabelName);
        }else{
            databaseOperate(assembleData(res), "UpDateByIMEI", _databaseTabelName);
        }

    }


    public List<BaseData> assembleData(Transform data){
        deviceInfoData deviceInfoData = new deviceInfoData(){
            imei = data.name, 
            // x = data.position.x, 
            // y = data.position.y, 
            // z = data.position.z
        };

        deviceInfoData = otherTempTest.getInstance().deviceInfoToDeviceInfoData(deviceInfoData);
        // Debug.Log("aacc == "+ aacc.deviceName + "  imei =="+ aacc.imei);
        List<BaseData> list = new List<BaseData>();
        list.Add(deviceInfoData);
        return list;
    }

    public MyEventArgs<string, List<BaseData>> createMyEventArgs(List<BaseData> list, string operate, string className){

        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>(className, list);
        return args;
    }

    public void databaseOperate(List<BaseData> list, string operate, string className){
        MyEventArgs<string, List<BaseData>> args = createMyEventArgs(list, operate, className);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, operate, args);
    }




    
}
