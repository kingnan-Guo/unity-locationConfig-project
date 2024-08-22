using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenCover.Framework.Model;
using UnityEngine;

public class SQLiteSigleManager : baseManager<SQLiteSigleManager>
{

    public SQLiteSigleManager(){
        Init();
    }
    //初始化数据库
    public void Init(){
        // 创建数据库
        SQLiteSigleCommand.getInstance().Init(gloabNetWorkConfig.ip);

        // 创建数据表
        



        EventCenterOptimizes.getInstance().AddEventListener<string, MyEventArgs<string, List<BaseData>>>(gloab_EventCenter_Name.DATABASE_OPERATE, (params1, params2) => {
            // Debug.Log("gloab_EventCenter_Name.DATABASE_CONNECT_SUCCESS ==222="+ params1 + "===" +params2);
            // Debug.Log("params2.KEYWORD =="+ params2.KEYWORD + "====params2.MESSAGE =="+ params2.MESSAGE);
            IOData(params1, params2);
        });


        // // 获取当前的表中 的 数据 列表
        // getDataList<deviceInfoData>();
    }


    public void CreateTable<T>() where T : BaseDataClass
    {
        SQLiteSigleCommand.getInstance().CreateTable<T>();
    }


    // <string, MyEventArgs<string, List<BaseData>>>
    public IEnumerable<object> IOData(string params1, MyEventArgs<string, List<BaseData>> params2){
        Type classType = Type.GetType(params2.KEYWORD);
        Type genericListType = typeof(List<>); // 获取List<>的泛型类型定义
        Type specificListType = genericListType.MakeGenericType(classType); // 创建List<int>的具体类型

        // 多个同名的 函数，用反射调用 方法
        MethodInfo[] methodInfoArr = typeof(SQLiteSigleCommand).GetMethods();
        MethodInfo methodInfo = null;

        foreach (MethodInfo methodItem in methodInfoArr)
        {
            if (methodItem.Name == params1){
                var ParameterType = methodItem.GetParameters()[0].ParameterType;
                // Debug.Log("methodInfo.Name =="+ ParameterType +"=="+ (new List<BaseData>()).GetType());

                // Debug.Log("methodInfo.Name bool =="+ (new List<BaseData>()).GetType().Name == (new List<BaseData>()).GetType().Name);
                // Debug.Log(" ==(new List<BaseData>()).GetType() =="+ (new List<BaseData>()).GetType().Name);
                // Debug.Log("String ParameterType ==="+ String.Equals(ParameterType.Name, (new List<BaseData>()).GetType().Name, StringComparison.OrdinalIgnoreCase));
                if(String.Equals(ParameterType.Name, (new List<BaseData>()).GetType().Name, StringComparison.OrdinalIgnoreCase)) {
                    // Debug.Log(" 传入数组的 methodInfo =="+ methodInfo);
                    methodInfo = methodItem;
                    methodInfo = methodInfo.MakeGenericMethod(classType); // 指定泛型方法的类型参数
                    // Debug.Log("methodInfo =="+ methodInfo);
                } else {
                    // Debug.Log("传入其他类型的 methodInfo =="+ methodInfo);
                    methodInfo = methodItem;
                    methodInfo = methodInfo.MakeGenericMethod(classType); 
                }
            }
        }
        // methodInfo = methodInfo.MakeGenericMethod(classType); 

        // // 创建List<int>实例
        var listInstance = Activator.CreateInstance(specificListType);
        // Debug.Log("listInstance =="+ listInstance);

        if (methodInfo != null)
        {
            // Select
            if(params1.Contains("Select")){
                listInstance = Select(params1, params2, methodInfo, classType);
            }
            // upDate
            else if(params1.Contains("UpDate")){
                Update(params1, params2, methodInfo, classType);
            }
            // Insert
            else if(params1.Contains("Insert")){
                Insert(params1, params2, methodInfo, classType);
            } 
            // Delete
            else if( params1.Contains("Delete")){
                Delete(params1, params2, methodInfo, classType);
            }
            // other
            else {
                Debug.Log("ohter ==" );
                otherFunction(params1, params2, methodInfo, classType);

            }

            // Debug.Log("listInstance =="+ listInstance.Count);
        }

        return listInstance as IEnumerable<object>;

    }



    public IEnumerable<object> Select(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
        Type genericListType = typeof(List<>); // 获取List<>的泛型类型定义
        Type specificListType = genericListType.MakeGenericType(classType); // 创建List<int>的具体类型
        var listInstance = Activator.CreateInstance(specificListType);
        
        object[] parameters;
        if(params1.Contains("Sql")){
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in params2.MESSAGE)
            {
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    if(property.GetValue(item) != null){
                        stringBuilder.Append(property.GetValue(item));
                        stringBuilder.Append(" AND ");
                    }

                }
            }
            stringBuilder.Remove(stringBuilder.Length - 5, 5); // 移除最后一个AND
            Debug.Log("Select sql = " + stringBuilder.ToString());
            parameters = new object[] {stringBuilder.ToString()};
        } 
        else if(params1.Contains("ById")) {
            // parameters = new object[] {stringBuilder.ToString()};
            int? id = null;
            foreach (var item in params2.MESSAGE)
            {
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    // stringBuilder.Append(property.GetValue(item));
                    // if(property.GetValue(item) != null){
                    //     id = property.GetValue(item)
                    // }
                    // Debug.Log("property.Name =="+ property.Name);
                    if(property.Name == "id" && property.GetValue(item) != null){
                        // Debug.Log("property.GetValue == "+ property.GetValue(item));
                        // id = (int?)property.GetValue(item);
                        id = int.Parse(property.GetValue(item).ToString());
                    }
                }
            }
            // Debug.Log("id =="+ id);
            parameters = new object[] {id};
            
        } 
        else {// Select
            //  传入类
            parameters = new object[] { };
            foreach (var item in params2.MESSAGE)
            {
                // item BaseData 要转换成   params2.KEYWORD 
                parameters = new object[] {item};
                // decimal
                // Debug.Log("Select item =="+ item);
            }
        }


        listInstance = methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
        (listInstance as IEnumerable<object>).ToList().ForEach((item) => {
            Debug.Log("IEnumerable item =="+ item);
        });
        Debug.Log("listInstance =="+ listInstance);
        return (listInstance as IEnumerable<object>);
    }



    public void Delete(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){

        Debug.Log("== Delete ==" );
        object[] parameters;
        if(params1.Contains("ByIds")) {
            List<int> ids = new List<int>();
            foreach (var item in params2.MESSAGE)
            {
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    if(property.Name == "id" && property.GetValue(item) != null){
                        ids.Add(int.Parse(property.GetValue(item).ToString()));
                    }
                }
            }
            Debug.Log("ids = " + ids.Count);
            parameters = new object[] { ids };
        } 
        else if(params1.Contains("ById")){
            int? id = null;
            foreach (var item in params2.MESSAGE)
            {
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    if(property.Name == "id" && property.GetValue(item) != null){
                        id = int.Parse(property.GetValue(item).ToString());
                    }
                }
            }
            parameters = new object[] {id};
        }
        else if(params1.Contains("BySql")){
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in params2.MESSAGE)
            {
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    if(property.GetValue(item) != null){
                        stringBuilder.Append(property.GetValue(item));
                        stringBuilder.Append(" AND ");
                    }
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 5, 5); // 移除最后一个AND
            Debug.Log("Select sql = " + stringBuilder.ToString());
            parameters = new object[] {stringBuilder.ToString()};
        } 
        else {
            parameters = new object[] {  };
        }

        methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);

    }






    public void Insert(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
        if(params1.Contains("InsertOrUpdate")){
            InsertOrUpdate(params1, params2, methodInfo, classType);
            return;
        }

        Debug.Log("== Insert ==");
        // 如果传入  的是 T classType
        var ParameterType = methodInfo.GetParameters()[0].ParameterType;
        // 这个是  传入的 数据  是  List<classType>
        IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(classType));
        foreach (var item in params2.MESSAGE)
        {
            object obj = Activator.CreateInstance(classType);
            var publicProperties = item.GetType().GetProperties();
            foreach (var property in publicProperties)
            {
                property.SetValue(obj, property.GetValue(item));
            }
            list.Add(obj);
        }

        // Debug.Log("Insert ParameterType.Name == "+ ParameterType.Name);

        if(String.Equals(ParameterType.Name, classType.Name, StringComparison.OrdinalIgnoreCase) ) {
            Debug.Log(" == Insert T ==");
            for(int i = 0; i < list.Count; i++){
                object[] parameters = new object[] {list[i]};
                methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
                // Debug.Log("list[i] =="+ list[i]);
            }
        } 
        else {
            Debug.Log(" == Insert list<T> ==");
            object[] parameters = new object[] { list };
            // Debug.Log("list =="+ list);
            methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
        }

    }




    public void Update(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
                    
        if(params1.Contains("UpDateById") || params1.Contains("UpDateByIMEI")){
            // UpDateById
            // var ParameterType = methodInfo.GetParameters()[0].ParameterType;
            // 这个是  传入的 数据  是  List<classType>
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(classType));
            foreach (var item in params2.MESSAGE)
            {
                object obj = Activator.CreateInstance(classType);
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    property.SetValue(obj, property.GetValue(item));
                }
                list.Add(obj);
            }

            for(int i = 0; i < list.Count; i++){
                object[] parameters = new object[] {list[i]};
                methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
                // Debug.Log("list[i] =="+ list[i]);
            }

        } 
        else if(params1.Contains("InsertOrUpdate")){
            InsertOrUpdate(params1, params2, methodInfo, classType);
        }
        else if(params1.Contains("InsertOrReplace")){
            InsertOrReplace(params1, params2, methodInfo, classType);
        }
        else if(params1.Contains("UpDates")){

        } 
        else if(params1.Contains("UpDates")){

        } 
        else {
            Debug.Log("== upDate ==");

            // 这个是  传入的 数据  是  List<classType>
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(classType));
            foreach (var item in params2.MESSAGE)
            {
                object obj = Activator.CreateInstance(classType);
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    property.SetValue(obj, property.GetValue(item));
                }
                list.Add(obj);
            }

            for(int i = 0; i < list.Count; i++){
                string sql = (string)params2.EXTENDDATA;
                object[] parameters = new object[] {list[i], sql};
                methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
                // Debug.Log("list[i] =="+ list[i]);
            }

            // string sql = "";
            // parameters = new object[] { dd, sql };
            // methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
        }

    }


    public void otherFunction(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){

        var ParameterType = methodInfo.GetParameters()[0].ParameterType;
        if(String.Equals(ParameterType.Name, classType.Name, StringComparison.OrdinalIgnoreCase) ) {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in params2.MESSAGE)
            {
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    stringBuilder.Append(property.GetValue(item));
                    stringBuilder.Append(" AND ");
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 5, 5); // 移除最后一个AND
            Debug.Log("Select sql = " + stringBuilder.ToString());
            object[] parameters = new object[] {stringBuilder.ToString()};
            // methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
        }
        else {

            // (specificListType)params2.MESSAGE;
            // params2.MESSAG list<BaseDate> 转换  list<T> ; 动态 泛型

            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(classType));
            foreach (var item in params2.MESSAGE)
            {
                object obj = Activator.CreateInstance(classType);
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    property.SetValue(obj, property.GetValue(item));
                }
                list.Add(obj);
            }
            object[] parameterss = new object[] {
                list
            };
            methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameterss);
        }


    }










    /// <summary>
    ///     插入或者更新 
    ///     params2.EXTENDDATA 是指定的对比字段
    /// </summary>
    /// <param name="params1"></param>
    /// <param name="params2"></param>
    /// <param name="methodInfo"></param>
    /// <param name="classType"></param>
    public void InsertOrUpdate(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){

        IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(classType));
        foreach (var item in params2.MESSAGE)
        {
            object obj = Activator.CreateInstance(classType);
            var publicProperties = item.GetType().GetProperties();
            foreach (var property in publicProperties)
            {
                property.SetValue(obj, property.GetValue(item));
            }
            list.Add(obj);
        }

        for(int i = 0; i < list.Count; i++){
            string name = (string)params2.EXTENDDATA;
            object[] parameters = new object[] {list[i], name};
            methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
        }

    }


    public void  InsertOrReplace(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
        Insert(params1, params2, methodInfo, classType);
    }




    // =========================

    public IEnumerable<object> getDataList<T>() where T : BaseDataClass
    {

        // List<BaseData> list = new List<BaseData>();
        // list.Add(new deviceInfoData());
        // MyEventArgs<string, List<BaseData>> args = createMyEventArgs(list, "Select", "deviceInfoData");
        // IEnumerable<object> listArr =  IOData("Select", args);
        // Debug.Log("getDataList listArr === "+ listArr.Count());

        //  ===============
        IEnumerable<object> listArr = SQLiteSigleCommand.getInstance().SelectAll<T>();
        Debug.Log("getDataList listArr === "+ listArr.Count());
        // SQLiteSigleView.getInstance().AddModelToSecene<T>(listArr);

        return listArr;
        
    }













    public MyEventArgs<string, List<BaseData>> createMyEventArgs(List<BaseData> list, string operate, string className){

        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>(className, list);
        return args;
    }


    // ===================================
    // private void testInsert(){
    //     // 批量新增
    //     List<deviceInfoData> list = new List<deviceInfoData>();
    //     for (int i = 0; i < 5; i++)
    //     {
    //         deviceInfoData b = new deviceInfoData();
    //         b.deviceName ="test_" + i;
    //         b.deviceId = "0" + i;
    //         b.deviceStatus = "5";
    //         b.deviceType = "deviceType83_40";
    //         b.imei = "123321123321123$1$1$" + i;
    //         b.orgCode = "001001";
    //         b.orgName = "E_floor_19";
    //         b.position = new Vector3(85, 78, 55);
    //         b.x = i;
    //         b.y = 78;
    //         b.z = 55;

    //         list.Add(b);
    //     }
    //     SQLiteSigleCommand.getInstance().Insert(list);
    // }

    // private void testSelect(){
    //     deviceInfoData deviceInfoData = new deviceInfoData();
    //     // deviceInfoData.id = 11;
    //     deviceInfoData.deviceType = "deviceType83_40";
    //     List<deviceInfoData> aasc=  SQLiteSigleCommand.getInstance().Select<deviceInfoData>(deviceInfoData);
    //     Debug.Log("查询结果" + aasc.Count);
    //     aasc.ToList().ForEach((res) => {
    //         Debug.Log("查询结果" + res.deviceName);
    //     });
    // }

}
