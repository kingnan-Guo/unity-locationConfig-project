using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using OpenCover.Framework.Model;
using UnityEngine;

public class SQLiteSigleCommand : baseManager<SQLiteSigleCommand>
{
    SQLiteDBCommand SQLiteDBCommand;
    public SQLiteSigleCommand(){
        // Init("test.db");
    }
    //初始化数据库
    public void Init(string DatabaseName){

        string filePath = Path.Combine(Application.streamingAssetsPath, $"SQLiteDatabase/{DatabaseName}.db");

        SQLiteDBCommand = new SQLiteDBCommand(filePath);

    }

    // 创建表
    public void CreateTable<T>() where T : BaseDataClass
    {
        if(IsTabelCreated<T>()){
            Debug.Log("表存在");
        }else{
            // Debug.Log("表不存在");
            SQLiteDBCommand.CreateTable<T>();// 创建 class 表
        }
    }

    /// <summary>
    /// 删除表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void DeleteTable<T>() where T : BaseDataClass
    {
        SQLiteDBCommand.DeleteTable<T>();
    }


    /// <summary>
    /// 插入数据
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="data">数据 可以是类实例 也可以时 List</param>
    public int Insert<T>(T data) where T : BaseDataClass
    {
        // 插入数据
        if(IsTabelCreated<T>()){
            return SQLiteDBCommand.Insert<T>(data);
        } else {
            // CreateTable<T>();
            // SQLiteDBCommand.Insert<T>(data);
            Debug.LogError($"表不存在,请先创建 {typeof(T).Name} 表");
            return -1;
        }
    }

    /// <summary>
    /// 批量插入数据
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="tList"></param>
    public int Insert<T>(List<T> tList) where T : class
    {
        // 检查参数
        if(tList == null || tList.Count == 0){
            Debug.LogError("Insert Error : t is null 参数错误");
            return -1;
        }

        // 插入数据
        if(IsTabelCreated<T>()){
            return SQLiteDBCommand.Insert<T>(tList);
        } else {
            Debug.LogError($"表不存在,请先创建 {typeof(T).Name} 表");
            return -1;
        }
    }



    // InsertOrUpdate
    /// <summary>
    ///  插入或更新数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="name"></param>
    public int InsertOrUpdate<T>(T data, string name = "imei") where T : BaseDataClass
    {
        // 插入数据
        if(IsTabelCreated<T>()){
            return SQLiteDBCommand.InsertOrUpdate<T>(data, name);
        } else {
            // CreateTable<T>();
            // SQLiteDBCommand.Insert<T>(data);
            Debug.LogError($"表不存在,请先创建 {typeof(T).Name} 表");
            return -1;
        }
    }


    // InsertOrReplace
    /// <summary>
    ///  插入或替换数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tList"></param>
    public int InsertOrReplace<T>(List<T> tList) where T : class
    {
        // 检查参数
        if(tList == null || tList.Count == 0){
            Debug.LogError("Insert Error : t is null 参数错误");
            return -1;
        }

        // 插入数据
        if(IsTabelCreated<T>()){
            return SQLiteDBCommand.InsertOrReplace<T>(tList);
        } else {
            Debug.LogError($"表不存在,请先创建 {typeof(T).Name} 表");
            return -1;
        }
    }



    /// <summary>
    /// 更新数据
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="data">要修改的数据 , 传入的数据要传 id</param>
    public int UpDateById<T>(T data) where T : BaseDataClass{
        return SQLiteDBCommand.UpDateById<T>(data);
    }

    /// <summary>
    /// 单条 更新数据
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="data">要更新的数据</param>
    /// <param name="sqlFliter">sql 过滤条件</param>
    public int UpDate<T>(T data, string sqlFliter) where T : BaseDataClass{
        return SQLiteDBCommand.UpDate(data, sqlFliter);
    }

    /// <summary>
    /// UpDates 批量更新数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="TList"></param>
    /// <param name="sqlList"></param>
    public int UpDates<T>(List<T> TList, List<string> sqlList) where T : BaseDataClass{
        if(TList == null || TList.Count == 0){
            Debug.LogError("upDate List<T> 参数错误");
            return -1;
        }
        return SQLiteDBCommand.UpDates<T>(TList, sqlList);
    }

    public void UpDateByKeyWord<T>(T data, string keyword) where T : BaseDataClass{
        if(keyword == null || keyword == ""){
            Debug.LogError("UpDateByKeyWord 参数错误");
        } else if(data.GetType().GetProperty(keyword) == null){
            Debug.LogError("UpDateByKeyWord 参数错误");
        } else{
            string sql = "";
            Debug.Log("UpDateByKeyWord =="+data.GetType().GetProperty(keyword).GetValue(data).GetType().Name);
            if(string.Equals(data.GetType().GetProperty(keyword).GetValue(data).GetType().Name, "String", StringComparison.OrdinalIgnoreCase) ){
                sql = $"{keyword} = '{data.GetType().GetProperty(keyword).GetValue(data)}'";
            } else {
                sql = $"{keyword} = {data.GetType().GetProperty(keyword).GetValue(data)}";
            }
            UpDate<T>(data, sql);
        }
    }


    public int UpDateByIMEI<T>(T data) where T : BaseDataClass{
        return SQLiteDBCommand.UpDateByIMEI<T>(data);
    }



    /// <summary>
    /// 删除数据
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="id">数据 id</param>
    public void DeleteById<T>(int id) where T : BaseDataClass{
        SQLiteDBCommand.DeleteById<T>(id);
    }

    /// <summary>
    /// 批量删除数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ids">List id</param>
    public void DeleteByIds<T>(List<int> ids) where T : BaseDataClass{
        SQLiteDBCommand.DeleteByIds<T>(ids);
    }

    /// <summary>
    /// 根据sql语句删除数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlFliter"></param>
    public void DeleteBySql<T>(string sqlFliter) where T : BaseDataClass{
        SQLiteDBCommand.DeleteBySql<T>(sqlFliter);
    }


    // public void DeleteAll<T>(){
    //     SQLiteDBCommand.DeleteAll<T>();
    // }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public void Delete<T>(T data) where T : BaseDataClass{
        // DeleteBySql<T>("id = " + data.id);
    }

    public List<T> Select<T>(T data) where T : BaseDataClass{
        StringBuilder stringBuilder = new StringBuilder();
        string sql = null;
        Type type = typeof(T);

        // if(data.id != 0){
        //     stringBuilder.Append($"id = {data.id} ");
        // }
        // AND age > 30

        var publicProperties = type.GetProperties();
        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();

            // stringBuilder.Append($"{attribute.FieldName} = ");
            if(attribute.IsPrimaryKey){
                var propertyValue= property.GetValue(data);
                if(Convert.ToSingle(propertyValue) > 0 ){
                    stringBuilder.Append($"{propertyValue} = ");
                    if(attribute.Type == "string"){
                        stringBuilder.Append($"'{propertyValue}'");
                    } 
                    else if(string.Equals(attribute.Type, "TEXT")){
                        stringBuilder.Append($"'{propertyValue}'");
                    } 
                    else {
                        stringBuilder.Append(propertyValue);
                    }
                    stringBuilder.Append(" AND ");
                }
            }

            if(attribute.IsCreated && !attribute.IsPrimaryKey){

                if(property.GetValue(data) == null){
                }
                // else if(attribute.Type == "float" && Convert.ToSingle(property.GetValue(data)) == 0){
                // } 
                else {
                    stringBuilder.Append($"{attribute.FieldName} = ");

                    if(attribute.Type == "string"){
                        stringBuilder.Append($"'{property.GetValue(data)}'");
                    } 
                    else if(string.Equals(attribute.Type, "TEXT")){
                        stringBuilder.Append($"'{property.GetValue(data)}'");
                    }
                    else {
                        stringBuilder.Append(property.GetValue(data));
                    }
                    stringBuilder.Append(" AND ");
                }
            }
        }
        if(stringBuilder.Length > 0){
            stringBuilder.Remove(stringBuilder.Length - 5, 5); // 移除最后一个AND
        }
        Debug.Log("Select sql = " + stringBuilder.ToString());

        sql = stringBuilder.ToString();

        if(sql != null){
            return SelectBySql<T>(sql);
        }
        return new List<T>();
    }


    /// <summary>
    /// 根据id查询数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id">数据 id</param>
    public List<T> SelectById<T>(int id) where T : BaseDataClass{
        List<T> data = new List<T>();
        if(SQLiteDBCommand.SelectById<T>(id) != null){
            data.Add(SQLiteDBCommand.SelectById<T>(id));
        }
        return data;
    }

    /// <summary>
    /// 根据sql语句查询数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlFliter">sql </param>
    public List<T> SelectBySql<T>(string sqlFliter) where T : BaseDataClass{
       return SQLiteDBCommand.SelectBySql<T>(sqlFliter);
    }


    // SelectAll
    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public List<T> SelectAll<T>() where T : BaseDataClass{
        return SQLiteDBCommand.SelectAll<T>();
    }


    public void Close(){
        // SQLiteDBCommand.Close();
    }

    public void Dispose(){
        SQLiteDBCommand.Dispose();
    }


    /// <summary>
    /// 判断表是否存在
    /// </summary>
    /// <param name="Name">传入值</param>
    /// <returns></returns>
    public bool IsTabelCreated(string Name){
        return SQLiteDBCommand.IsTabelCreated(Name);
    }
    /// <summary>
    /// 判断表是否存在
    /// </summary>
    /// <typeparam name="T">传入泛型</typeparam>
    /// <returns></returns>
    public bool IsTabelCreated<T>(){
        return SQLiteDBCommand.IsTabelCreated(typeof(T).Name);
    }


    public void test<T>(string sql)
    {
        Debug.Log("test = " + sql + "T =="+ typeof(T).Name);
    }
    // public void test(string sql)
    // {
    //     Debug.Log("test = " + sql );
    // }

    // public void test<T>(T sql)
    // {
    //     Debug.Log("test = " + sql + "T =="+ typeof(T).Name);
    // }

    // public void test<T>()
    // {
    //     Debug.Log("test =  T =="+ typeof(T).Name);
    // }
    public void test<T>(List<T> sql)
    {
        Debug.Log("test = " + sql + "T =="+ typeof(T).Name);
    }

}
