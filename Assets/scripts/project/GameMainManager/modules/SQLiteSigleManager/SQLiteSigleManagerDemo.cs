using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;


public class SQLiteSigleManagerDemo : baseManager<SQLiteSigleManagerDemo>
{
    public SQLiteSigleManagerDemo(){

    }


     //  select Demo 
    public void Select(){
        List<BaseData> listSingle = new List<BaseData>();
        deviceInfoData deviceInfoDataSingle = new deviceInfoData();
        // deviceInfoDataSingle.deviceName ="test_" + i;
        // deviceInfoDataSingle.deviceId = "0" + i;
        // deviceInfoDataSingle.deviceStatus = "5";
        deviceInfoDataSingle.modelType = "40";
        // deviceInfoDataSingle.imei = "123321123321123$1$1$" + i;
        // deviceInfoDataSingle.orgCode = "001001";
        // deviceInfoDataSingle.orgName = "E_floor_19";
        // deviceInfoDataSingle.position = new Vector3(85, 78, 55);
        // deviceInfoDataSingle.id = 3;
        listSingle.Add(deviceInfoDataSingle);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", listSingle);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "Select", args);
    }

    // SelectBySql
    public void SelectBySql(){
        List<BaseData> listss = new List<BaseData>();
        //  SelectBySql 可以
        customClass SelectBySqlCustomClass = new customClass();
        SelectBySqlCustomClass.sql = "id > 3";
        listss.Add(SelectBySqlCustomClass);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", listss);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "SelectBySql", args);
    }


    public void SelectById_customClass(){
        List<BaseData> listss = new List<BaseData>();
        //  SelectBySql 可以
        customClass SelectBySqlCustomClass = new customClass();
        SelectBySqlCustomClass.id = 3;
        listss.Add(SelectBySqlCustomClass);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", listss);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "SelectById", args);
    }



    public void SelectById_deviceInfoData(){
        List<BaseData> listss = new List<BaseData>();
        //  SelectBySql 可以
        deviceInfoData SelectBySqlCustomClass = new deviceInfoData();
        SelectBySqlCustomClass.id = 3;
        listss.Add(SelectBySqlCustomClass);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", listss);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "SelectById", args);
    }

    // 测试可以找到多个
    public void test(){

        List<BaseData> listss = new List<BaseData>();
        customClass SelectBySqlCustomClass = new customClass();
        SelectBySqlCustomClass.sql = "id > 3";
        listss.Add(SelectBySqlCustomClass);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("customClass", listss);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "test", args);

    }

    public void Insert(){
        List<BaseData> listss = new List<BaseData>();
        for (int i = 0; i < 20; i++)
        {
            deviceInfoData b = new deviceInfoData();
            b.deviceName ="test_" + i;
            b.deviceId = "0" + i;
            b.deviceStatus = "5";
            b.deviceCategory = 1;
            b.modelType = "40";
            b.modelTypeName = "烟感";
            b.imei = "123321123321123$1$1$" + i;
            b.parentModelId = "E_floor_19";
            b.parentModelName = "E_floor_19";
            b.position = new Vector3(85, 78, 55);
            b.positionX = i;
            b.positionY = 78.2f;
            b.positionZ = 55;

            listss.Add(b);
        }

        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", listss);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "Insert", args);


    }

    public void InsertOne(){
        Debug.Log("databaseOperateEnd 开始插入数据库 =="+ System.DateTime.Now);
        List<BaseData> listsB = new List<BaseData>();
        for (int i = 0; i < 10000; i++)
        {
            deviceInfoData b = new deviceInfoData();
            b.deviceName ="test_" + i;
            // b.deviceId = "0" + i;
            b.deviceCategory = 1;
            // b.deviceStatus = "5";
            b.modelType = "40";
            b.modelTypeName = "烟感";
            b.imei = "123321123321123$1$1$" + i;
            b.parentModelId = "E_floor_19";
            b.parentModelName = "E_floor_19";
            // b.position = new Vector3(85, 78, 55);
            b.positionX = i;
            b.positionY = 78;
            b.positionZ = 55;
            b.rotateX = 0;
            b.rotateY = 0;
            b.rotateZ = 0;
            b.scaleX = 1;
            b.scaleY = 1;
            b.scaleZ = 1;
            listsB.Add(b);
        }

        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", listsB);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "Insert", args);

        Debug.Log("databaseOperateEnd 数据处理完 =="+ System.DateTime.Now);
    }



    public void InsertOrUpdate(){
        List<BaseData> list = new List<BaseData>();
        deviceInfoData deviceInfoData = new deviceInfoData();
        deviceInfoData.deviceName = "name2001111";
        deviceInfoData.deviceId = "011221111";
        deviceInfoData.deviceCategory = 1;
        deviceInfoData.deviceStatus = "5";
        deviceInfoData.modelType = "401";
        deviceInfoData.modelTypeName = "烟感1";
        deviceInfoData.imei = "111111111111111$1$1$1";
        deviceInfoData.parentModelId = "E_floor_19";
        deviceInfoData.parentModelName = "E_floor_19";
        deviceInfoData.positionX = 200;
        deviceInfoData.positionY = 78;
        deviceInfoData.positionZ = 55;
        // scaleY = ,scaleX = ,scaleZ = ,rotateX = ,rotateY = ,rotateZ = 
        deviceInfoData.scaleY = 1;
        deviceInfoData.scaleX = 1;
        deviceInfoData.scaleZ = 1;
        deviceInfoData.rotateX = 0;
        deviceInfoData.rotateY = 0;
        deviceInfoData.rotateZ = 0;

        
        list.Add(deviceInfoData);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list, "imei");
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "InsertOrUpdate", args);


    }



    public void InsertOrReplace(){
        List<BaseData> list = new List<BaseData>();
        deviceInfoData deviceInfoData = new deviceInfoData();
        deviceInfoData.deviceName = "name1";
        deviceInfoData.deviceId = "011221111";
        deviceInfoData.deviceCategory = 1;
        deviceInfoData.deviceStatus = "5";
        deviceInfoData.modelType = "401";
        deviceInfoData.modelTypeName = "烟感1";
        deviceInfoData.imei = "111111111111111$1$1$1";
        deviceInfoData.parentModelId = "E_floor_19";
        deviceInfoData.parentModelName = "E_floor_19";
        deviceInfoData.positionX = 200;
        deviceInfoData.positionY = 78;
        deviceInfoData.positionZ = 55;
        // scaleY = ,scaleX = ,scaleZ = ,rotateX = ,rotateY = ,rotateZ = 
        deviceInfoData.scaleY = 1;
        deviceInfoData.scaleX = 1;
        deviceInfoData.scaleZ = 1;
        deviceInfoData.rotateX = 0;
        deviceInfoData.rotateY = 0;
        deviceInfoData.rotateZ = 0;
        list.Add(deviceInfoData);

        deviceInfoData deviceInfoData2 = new deviceInfoData();
        deviceInfoData2.deviceName = "name2";
        deviceInfoData2.deviceId = "011221111";
        deviceInfoData2.deviceCategory = 1;
        deviceInfoData2.deviceStatus = "5";
        deviceInfoData2.modelType = "401";
        deviceInfoData2.modelTypeName = "烟感1";
        deviceInfoData2.imei = "111111111111111$1$1$2";
        deviceInfoData2.parentModelId = "E_floor_19";
        deviceInfoData2.parentModelName = "E_floor_19";
        deviceInfoData2.positionX = 200;
        deviceInfoData2.positionY = 78;
        deviceInfoData2.positionZ = 55;
        // scaleY = ,scaleX = ,scaleZ = ,rotateX = ,rotateY = ,rotateZ = 
        deviceInfoData2.scaleY = 1;
        deviceInfoData2.scaleX = 1;
        deviceInfoData2.scaleZ = 1;
        deviceInfoData2.rotateX = 0;
        deviceInfoData2.rotateY = 0;
        deviceInfoData2.rotateZ = 0;
        list.Add(deviceInfoData2);

        deviceInfoData deviceInfoData3 = new deviceInfoData();
        deviceInfoData3.deviceName = "name3";
        deviceInfoData3.deviceId = "011221111";
        deviceInfoData3.deviceCategory = 1;
        deviceInfoData3.deviceStatus = "5";
        deviceInfoData3.modelType = "401";
        deviceInfoData3.modelTypeName = "烟感1";
        deviceInfoData3.imei = "111111111111111$1$1$3";
        deviceInfoData3.parentModelId = "E_floor_19";
        deviceInfoData3.parentModelName = "E_floor_19";
        deviceInfoData3.positionX = 200;
        deviceInfoData3.positionY = 78;
        deviceInfoData3.positionZ = 55;
        // scaleY = ,scaleX = ,scaleZ = ,rotateX = ,rotateY = ,rotateZ = 
        deviceInfoData3.scaleY = 1;
        deviceInfoData3.scaleX = 1;
        deviceInfoData3.scaleZ = 1;
        deviceInfoData3.rotateX = 0;
        deviceInfoData3.rotateY = 0;
        deviceInfoData3.rotateZ = 0;


        list.Add(deviceInfoData3);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "InsertOrReplace", args);


    }




    public void DeleteById(){
        

        List<BaseData> list = new List<BaseData>();
        customClass SelectBySqlCustomClass = new customClass();
        SelectBySqlCustomClass.id = 3;
        list.Add(SelectBySqlCustomClass);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "DeleteById", args);


    }


    public void DeleteByIds(){
        
        List<BaseData> list = new List<BaseData>();

        for (int i = 2; i < 8; i++){
            //  SelectBySql 可以
            deviceInfoData SelectBySqlCustomClass = new deviceInfoData();
            SelectBySqlCustomClass.id = i;
            list.Add(SelectBySqlCustomClass);
        }
        // customClass SelectBySqlCustomClass = new customClass();
        // SelectBySqlCustomClass.id = 5;
        // list.Add(SelectBySqlCustomClass);
        // SelectBySqlCustomClass.id = 6;
        // list.Add(SelectBySqlCustomClass);
        Debug.Log("DeleteByIds" + list.Count);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "DeleteByIds", args);


    }


    public void DeleteBySql(){
        
        List<BaseData> list = new List<BaseData>();
        customClass SelectBySqlCustomClass = new customClass();
        SelectBySqlCustomClass.sql = "id > 15";
        list.Add(SelectBySqlCustomClass);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "DeleteBySql", args);
    }



    public void upDate(){
        List<BaseData> list = new List<BaseData>();
        deviceInfoData deviceInfoData = new deviceInfoData();
        deviceInfoData.id = 8;
        deviceInfoData.deviceName = "name200";
        deviceInfoData.deviceId = "01122";
        deviceInfoData.deviceCategory = 1;
        deviceInfoData.deviceStatus = "5";
        deviceInfoData.modelType = "40";
        deviceInfoData.modelTypeName = "烟感";
        deviceInfoData.imei = "123321123321123$1$1$7";
        deviceInfoData.parentModelId = "E_floor_19";
        deviceInfoData.parentModelName = "E_floor_19";
        deviceInfoData.position = new Vector3(85, 78, 55);
        deviceInfoData.positionX = 200;
        deviceInfoData.positionY = 78;
        deviceInfoData.positionZ = 55;
        
        list.Add(deviceInfoData);


        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list,  "imei='123321123321123$1$1$7'");
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "UpDate", args);
    }


    public void UpDateByKeyWord(){
        List<BaseData> list = new List<BaseData>();
        deviceInfoData deviceInfoData = new deviceInfoData();
        deviceInfoData.id = 8;
        deviceInfoData.deviceName = "name2001";
        deviceInfoData.deviceId = "011221";
        deviceInfoData.deviceCategory = 1;
        deviceInfoData.deviceStatus = "5";
        deviceInfoData.modelType = "40";
        deviceInfoData.modelTypeName = "烟感";
        deviceInfoData.imei = "123321123321123$1$1$7";
        deviceInfoData.parentModelId = "E_floor_19";
        deviceInfoData.parentModelName = "E_floor_19";
        deviceInfoData.position = new Vector3(85, 78, 55);
        deviceInfoData.positionX = 200;
        deviceInfoData.positionY = 78;
        deviceInfoData.positionZ = 55;
        
        list.Add(deviceInfoData);

        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list,  "imei");
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "UpDateByKeyWord", args);
    }



    public void UpDateById(){
        List<BaseData> list = new List<BaseData>();
        deviceInfoData deviceInfoData = new deviceInfoData();
        deviceInfoData.id = 9;
        deviceInfoData.deviceName = "name200";
        deviceInfoData.deviceId = "01122";
        deviceInfoData.deviceCategory = 1;
        deviceInfoData.deviceStatus = "5";
        deviceInfoData.modelType = "40";
        deviceInfoData.modelTypeName = "烟感";
        deviceInfoData.imei = "123321123321123$1$1$7";
        deviceInfoData.parentModelId = "E_floor_19";
        deviceInfoData.parentModelName = "E_floor_19";
        deviceInfoData.position = new Vector3(85, 78, 55);
        deviceInfoData.positionX = 200;
        deviceInfoData.positionY = 78;
        deviceInfoData.positionZ = 55;
        list.Add(deviceInfoData);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "UpDateById", args);
    }
    

    public void UpDateByIMEI(){
        List<BaseData> list = new List<BaseData>();
        deviceInfoData deviceInfoData = new deviceInfoData();
        deviceInfoData.id = 9;
        deviceInfoData.deviceName = "name200";
        deviceInfoData.deviceId = "01122";
        deviceInfoData.deviceCategory = 1;
        deviceInfoData.deviceStatus = "5";
        deviceInfoData.modelType = "40";
        deviceInfoData.modelTypeName = "烟感";
        deviceInfoData.imei = "123321123321123$1$1$7";
        deviceInfoData.parentModelId = "E_floor_19";
        deviceInfoData.parentModelName = "E_floor_19";
        deviceInfoData.position = new Vector3(85, 78, 55);
        deviceInfoData.positionX = 200;
        deviceInfoData.positionY = 78;
        deviceInfoData.positionZ = 55;
        list.Add(deviceInfoData);
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>("deviceInfoData", list);
        EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, "UpDateByIMEI", args);
    }





    // ===================================================

    public void otherBackUp(){


        // ==============================
        // EventCenterOptimizes.getInstance().EventTrigger<string, string>(gloab_EventCenter_Name.DATABASE_CONFIG, "SelectBySql", "id < 3");
        // MyEventArgs<string, string> args = new MyEventArgs<string, string>("deviceInfoData", "id > 3");
        // EventCenterOptimizes.getInstance().EventTrigger<string, MyEventArgs<string, string>>(gloab_EventCenter_Name.DATABASE_OPERATE, "SelectBySql", args);


        // ==============================
        // EventCenterOptimizes.getInstance().AddEventListener<string, MyEventArgs<string, string>>(gloab_EventCenter_Name.DATABASE_OPERATE, (params1, params2) => {
        //     Debug.Log("gloab_EventCenter_Name.DATABASE_CONNECT_SUCCESS ==111="+ params1 + "===" +params2);
            
        //     // MyEventArgs<string, string> aa = params2;

        //     // Debug.Log("aa.KEYWORD =="+ aa.KEYWORD + "====aa.MESSAGE =="+ aa.MESSAGE);

        //     Type classType = Type.GetType(params2.KEYWORD);

        //     MethodInfo methodInfo = typeof(SQLiteSigleCommand).GetMethod(params1);
        //     methodInfo = methodInfo.MakeGenericMethod(classType); // 指定泛型方法的类型参数

        //     Type genericListType = typeof(List<>); // 获取List<>的泛型类型定义
        //     Type specificListType = genericListType.MakeGenericType(classType); // 创建List<int>的具体类型
    
        //     // 创建List<int>实例
        //     var listInstance = Activator.CreateInstance(specificListType);
        //     // Debug.Log("listInstance =="+ listInstance);

        //     if (methodInfo != null)
        //     {
        //         object[] parameters = new object[] {params2.MESSAGE};

        //         if(params2.KEYWORD.Contains("Select")){
        //             listInstance = methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
        //             (listInstance as IEnumerable<object>).ToList().ForEach((item) => {
        //                 Debug.Log("item =="+ item);
        //             });
        //             Debug.Log("listInstance =="+ listInstance);
        //         } else {
        //             methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
        //         }


        //         // Debug.Log("listInstance =="+ listInstance.Count);

        //         //methodInfo.Invoke(SQLiteSigleCommand.getInstance(), null); // 调用泛型方法
                
        //     }
        // });


    }
}
