using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
            // Debug.Log("Select sql = " + stringBuilder.ToString());
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
        // (listInstance as IEnumerable<object>).ToList().ForEach((item) => {
        //     Debug.Log("IEnumerable item =="+ item);
        // });
        // Debug.Log("listInstance =="+ listInstance);
        databaseOperateEnd(params1,  classType, MethodName:params2.METHODNAME, null);
        return (listInstance as IEnumerable<object>);
    }



    public int Delete(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
        object InsertStatus = new int();
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
            // Debug.Log("ids = " + ids.Count);
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
        else if(params1.Contains("DeleteByIMEI")){
            // string? imei = null;
            List<string> imeis = new List<string>();
            foreach (var item in params2.MESSAGE)
            {
                var publicProperties = item.GetType().GetProperties();
                foreach (var property in publicProperties)
                {
                    if(property.Name == "imei" && property.GetValue(item) != null){
                        imeis.Add(property.GetValue(item).ToString());
                    }
                }
            }
            Debug.Log("imeis = " + imeis.Count);
            parameters = new object[] { imeis };

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
            // Debug.Log("Select sql = " + stringBuilder.ToString());
            parameters = new object[] {stringBuilder.ToString()};
        } 
        else {
            parameters = new object[] {  };
        }

        InsertStatus = methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);


        // Debug.Log("InsertStatus =="+ InsertStatus);
        if(((int)InsertStatus) > 0){
            // Debug.Log("删除完成");
            databaseOperateEnd(params1,  classType, MethodName:params2.METHODNAME, InsertStatus);
        }

        return (int)InsertStatus;

    }






    public int Insert(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
        // Debug.Log("== Insert ==");
        object InsertStatus = new int();
        if(params1.Contains("InsertOrUpdate")){
            InsertStatus = InsertOrUpdate(params1, params2, methodInfo, classType);
        
        } else {

            
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
                // Debug.Log(" == Insert T ==");
                for(int i = 0; i < list.Count; i++){
                    object[] parameters = new object[] {list[i]};
                    InsertStatus = methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
                    // Debug.Log("list[i] =="+ list[i]);
                }
            }
            else {
                // Debug.Log(" == Insert list<T> ==");
                object[] parameters = new object[] { list };
                // Debug.Log("list =="+ list);
                InsertStatus = methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
            }

        }

        // Debug.Log("InsertStatus ====="+ InsertStatus);
        if(((int)InsertStatus) > 0){
            // Debug.Log("完成入库");
            databaseOperateEnd(params1,  classType, MethodName:params2.METHODNAME, InsertStatus);
        }

        return (int)InsertStatus;


        

    }




    public void Update(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
         object InsertStatus = new int();
         int Count = 0;
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
                InsertStatus = methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
                // Debug.Log("list[i] =="+ list[i]);
                Count +=  (int)InsertStatus;
            }

        } 
        else if(params1.Contains("InsertOrUpdate")){
            InsertStatus = InsertOrUpdate(params1, params2, methodInfo, classType);
        }
        else if(params1.Contains("InsertOrReplace")){
            InsertStatus = InsertOrReplace(params1, params2, methodInfo, classType);
        }
        else if(params1.Contains("UpDates")){

        } 
        else {
            // Debug.Log("== upDate ==");

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
                InsertStatus = methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
                // Debug.Log("list[i] =="+ list[i]);
                Count +=  (int)InsertStatus;
            }

            // string sql = "";
            // parameters = new object[] { dd, sql };
            // methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);
        }

      
        // Debug.Log("InsertStatus ====="+ InsertStatus + "Count =="+ Count);
        if(((int)InsertStatus) > 0){
            // Debug.Log("完成 upDate");
            databaseOperateEnd(params1,  classType, MethodName:params2.METHODNAME, InsertStatus);
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
            // Debug.Log("Select sql = " + stringBuilder.ToString());
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





    // ================== 扩展 ==========================
    /// <summary>
    ///     插入或者更新 
    ///     params2.EXTENDDATA 是指定的对比字段
    /// </summary>
    /// <param name="params1"></param>
    /// <param name="params2"></param>
    /// <param name="methodInfo"></param>
    /// <param name="classType"></param>
    public int InsertOrUpdate(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
        object InsertStatus = new int();
        int Count = 0;
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
            InsertStatus = methodInfo.Invoke(SQLiteSigleCommand.getInstance(), parameters);

            Count += (int)InsertStatus;
        }
        return Count;

    }


    public int InsertOrReplace(string params1, MyEventArgs<string, List<BaseData>> params2, MethodInfo methodInfo, Type classType){
        return Insert(params1, params2, methodInfo, classType);
    }






    /// <summary>
    /// 数据库操作完成  发送事件 
    /// </summary>
    /// <param name="operate">对数据库的操作</param>
    /// <param name="className">类名</param>
    /// <param name="MethodName">调用 数据库操作 的 函数 名</param>
    /// <param name="data">数据库操作返回的数据</param>
    public void databaseOperateEnd(string operate, Type className, string MethodName = null,object data = null){
        Debug.Log("databaseOperateEnd == 数据 "+ className.Name +" == 操作 =="+ operate +" == 完成 == 结果 ="+ data +"MethodName =="+MethodName);
        // Debug.Log("databaseOperateEnd 数据库 end=="+ DateTime.Now);
        // 数据库操作完成 输出数据
        EventCenterOptimize.getInstance().EventTrigger<MyEventArgs<string, object>>(gloab_EventCenter_Name.DATABASE_OPERATE_END, new MyEventArgs<string, object>(operate, className, data, MethodName));
    }





    // seletByIMEI
    public IEnumerable<object> seletByIMEI<T>(string IMEI) where T : BaseDataClass
    {
        return SQLiteSigleCommand.getInstance().SelectBySql<T>("IMEI = '"+ IMEI +"'");
    }




    // 删除数据库 
    public void DeleteDatabase(){
        // Debug.Log("DeleteDatabase == "+ databaseName);
        string databaseName = gloabNetWorkConfig.ip;

        SQLiteSigleCommand.getInstance().DeleteTable<deviceInfoData>();
    }












    // ========================= 待删除 ==

    public IEnumerable<object> getALLDataList<T>() where T : BaseDataClass
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
        //  ===============
        // IEnumerable<object> listArr = SQLiteSigleCommand.getInstance().SelectBySql<T>("deviceCategory = 1");

        // IEnumerable<object> listArr = SQLiteSigleCommand.getInstance().SelectBySql<T>("ORDER BY id DESC LIMIT 50");
        // // IEnumerable<object> listArr = SQLiteSigleCommand.getInstance().SelectBySql<T>("id > 105011");
        // return listArr;
        
    }

    // public IEnumerable<object> getALLDataList<T>() where T : BaseDataClass
    // {
    //     return listArr;
    // }


















}
