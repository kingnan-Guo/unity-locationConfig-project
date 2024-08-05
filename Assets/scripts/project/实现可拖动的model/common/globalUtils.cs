// using System;
// using System;
// using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public class globalUtils : baseManager<globalUtils>
{


    // public globalUtils(){
    //     // Debug.Log("utils");
    //     //初始化
    //     // MonoManager.getInstance().AddUpdateListener(myUpdate);
    // }


    /// <summary>
    /// 全局 全局 function 阻挡 射线
    /// 不传参数 忽略 所有   canvas 图层
    /// </summary>
    /// <param name="IgnoreRaycastTagList">阻挡射线的 标签 集合名称</param>
    /// <returns></returns>
    public bool UIElementsBlockRaycast(List<string> IgnoreRaycastTagList = null)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        // gr.Raycast(eventData, list);
        EventSystem.current.RaycastAll(eventData, list);
        // Debug.Log("UIElementsBlockRaycast ==" + list.Count + "IgnoreRaycastTagList =="+ IgnoreRaycastTagList?.Count);
        if(IgnoreRaycastTagList != null && IgnoreRaycastTagList.Count > 0){
            bool tagert = false;
            foreach (RaycastResult item in list){
                // Debug.Log("UIElementsBlockRaycast item =="+ item.gameObject.name);
                if (IgnoreRaycastTagList.Contains(item.gameObject.tag)){
                    // return true;
                    tagert = true;
                }
            }
            return tagert;

        } else {
            return list.Count > 0;
        }
    }


    /// 截流
    /// /// <summary>
    /// 
    /// </summary>
    /// <param name="time">间隔时间</param>
    /// <param name="action">执行方法</param>
    /// <returns></returns>
    public IEnumerator DelayInvoke(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }






    // public virtual 

    /// <summary>
    /// 过滤函数
    /// T 输出类型
    /// K 输入类型
    /// </summary>
    // /// <param name="data"></param>
    public T filterFunctionOfClass<T, K>(K data) where T : new()
    {
        T res = new T();

        System.Type TClass = typeof(T);

        // TClass.InvokeMember ("", 
        // BindingFlags.InvokeMethod | BindingFlags.Default, null, new T(),
        //     new object [] {});

        // PropertyInfo[] publicProperties = TClass.GetProperties();
        // foreach (var property in publicProperties)
        // {
        //     Debug.Log("property.Name = " + property);
        // }

        FieldInfo[] FieldInfoArr = TClass.GetFields();
        foreach (var item in FieldInfoArr)
        {
            // Debug.Log("FieldInfo.Name = " + item.Name);

            // Debug.Log("FieldInfo.GetValue(res) = " + item.GetValue(res));

            // Debug.Log("GetMethod  = " + TClass.GetMethod("get"+ item.Name)?.Name);
            
            item.SetValue(res, TClass.GetMethod("get"+ item.Name)?.Invoke(res, new object[] { data}));

        }

        // MemberInfo[] memberInfos = TClass.GetMembers(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly );
        // foreach (var item in memberInfos)
        // {
        //     Debug.Log("GetMembers = " + item);
        // }
        // BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
        //BindingFlags.Instance | BindingFlags.Public

        // BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

        // 获取所有属性
        // PropertyInfo[] propertyInfos = TClass.GetProperties(flag);
        // foreach (var property in propertyInfos)
        // {
        //     // Console.WriteLine($"{property.Name}: {property.GetValue(people)}");

        //     Debug.Log("propertyInfos = " + property);
        // }
        // 获取所有方法
        // MethodInfo[] methodInfos = TClass.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly);
        // foreach (var method in methodInfos)
        // {
        //     // Console.WriteLine($"{method.Name}");
        //     Debug.Log("methodInfos = " + method.Name);

        //     Debug.Log("methodInfos = " +  method.Invoke(res, new object[] { data}));

        //     // fieldInfo_key.SetValue(di, value);

        //     // TClass.

        //     // ParameterInfo[] parameterInfos = method.GetParameters();

        //     // foreach (var parameter in parameterInfos)
        //     // {
        //     //     Debug.Log("parameterInfos = " + parameter.Name + " parameter=="+ parameter.ParameterType.Name);
        //     // }

        // }


        // 查看类构造函数
        // ConstructorInfo[] constructorInfos = TClass.GetConstructors();    
        // foreach (ConstructorInfo constructor in constructorInfos)
        // {
        //     ParameterInfo[] ps = constructor.GetParameters();   //取出每个构造函数的所有参数
        //     foreach (ParameterInfo pi in ps)                    
        //     {
        //         // Console.Write(pi.ParameterType.ToString() + " " + pi.Name + ",");

        //         Debug.Log("pi.ParameterType.ToString() = " + pi.ParameterType.ToString() + " =="+ pi.Name);
        //     }
        // }

        // Debug.Log("res = === " + (res as BuildingInfoClass).BuildingTag);

        return res;
    }


    // public T filterFunctionOfBoject<T, K>(K name) where T : System.Object
    // {
    //     // System.Type TClass = typeof(T);

    //     // T res = ;
    //     return default(T);
    // }
    public void filterFunction(string name)
    {
        // return name;
        Debug.Log("filterFunction name =="+ name);
    }




    /// <summary>
    /// filterSpecified 过滤器 
    /// 用于传入过滤方法 并返回结果
    /// T 输出类型
    /// K 输入类型
    /// </summary>
    /// <param name="generateKey"></param>
    public void filterSpecified<T, V, K>(System.Func<K, V> generateKey, K name, UnityAction<V> callback = null){

        // Func<string> aa = generateKey;
        // generateKey(name);
        // System.Func<T> a = (System.Func<T>)generateKey;

        // object aa = generateKey<T>(name);
        callback(generateKey(name));

        // GenerateKey generateKey = new GenerateKey();
        // typeof (GenerateKey).GetField("filterSpecified").SetValue(generateKey, true);
 
    }




    public TR filterFunctionOfListCustom<TR, T>(T data)
    {
        // T res = new T();
        // return res;
        return default(TR);
    }

    public void filterSpecifiedList<T, TR, L, K>(
        System.Func<T, K, TR> generateKey, 
        L dataList, 
        K keyWorld,
        UnityAction<List<TR>> callback = null){

        // List<B> list= new List<B>();
        // B data = new B();

        // object b = new object();
        // srr<TR> rt = new List<TR>();
        // TR[] rt = new TR[]{};

        List<TR> rt = new List<TR>();

        // typeof(L).GetMethod("ForEach").Invoke(dataList, new object[] {
            
        // });


        // Debug.Log("dataList =="+ typeof(L).IsGenericType);
        // Debug.Log("dataList =="+ typeof(L).GetGenericTypeDefinition());
        ///  适配  objetc[] 和 list<object>
        // (typeof(L).IsGenericType ?  (dataList as List<object>) :  (dataList as object[]).ToList()).ForEach((item) => {

        //     // args[0] = item;
        //     // args[1] = keyCode;
        //     // res = (TR[])typeof(L).GetMethod("filterFunctionOfListCustom").Invoke(dataList, args);
        //     // res = (TR[])generateKey(item, keyCode);
        //     // res = (TR[])generateKey(((item as GameObject).name), keyCode);

        //     TR a =  generateKey((T)(item), keyWorld);
        //     if(a != null){
        //         rt.Add(a);
        //     }

        // });



        // IEnumerable<object>  协方差  
        (dataList as IEnumerable<object>).ToList().ForEach((item) => {
            TR a =  generateKey((T)(item), keyWorld);
            // Debug.Log("a =="+ a + " default(TR) =="+ default(TR) + "==="+ EqualityComparer<T>.Default.Equals(a));
            // Debug.Log("IsDefault<TR>(a) ==" + IsDefault<TR>(a));
            if(a != null && !IsDefault<TR>(a)){
                // Debug.Log("IsDefault ==="+ IsDefault(a) + " ==" + default(TR) == default(TR));
                
                // Debug.Log("IsValueType ==="+ typeof(TR).IsValueType);

                // Debug.Log(" IsNullable =="+ IsDefault<TR>(a));
                
                rt.Add(a);
            }
        });
        // Debug.Log("(dataList as object[])  =="+ ((dataList as object[]) != null ? (dataList as object[]).ToList() : (dataList as List<object>)));


        // if(typeof(L).IsGenericType ){
        //     // 这个特性称为泛型协方差；要获得更多信息，请搜索C#协方差，您将看到大量解释其工作原理的文章。
        //     // 特别要注意的是，协方差只适用于引用类型；您不能将整数序列用作对象序列。你明白为什么吗？
        //     // 或者，如果您手头有一个IEnumerable，则可以将任何序列的副本复制到带有mysequence.Cast<object>().ToList()的对象列表中。但这是一个副本，而不是一个参考转换。
        //     Debug.Log("dataList =="+ typeof(L).GetGenericTypeDefinition());
        //     Debug.Log("(dataList as List<object>).Count(); =="+ (dataList as IEnumerable<object>).ToList());
        // } else {
        //     Debug.Log("(dataList as List<object>).Count(); 222222=="+ (dataList as IEnumerable<object>).ToList());
        // }



        // Debug.Log("dataList as object[]    =="+ dataList + " =="+  (dataList as List<object>));
        // Debug.Log("typeof(L).IsGenericType    ==" +  typeof(L).IsGenericType + " === "+ (typeof(L).IsGenericType ? (dataList as List<object>) :  (dataList as object[])) );



        

        // Debug.Log("dataList =="+ typeof(L));

        // Debug.Log(" rt  =="+ rt);


        callback((rt));





            // generateKey<T, R>();
            // System.Func<T, R> a = (System.Func<T, R>)generateKey;

            // (T)key;
    




 
    }




    /// <summary>
    /// 判断是否为默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDefault<T>(T value)
    {

        return EqualityComparer<T>.Default.Equals(value, default(T));
        // return typeof(T).IsValueType && EqualityComparer<T>.Default.Equals(value, default(T));
    }


    public static bool IsNullable<T>()
    {
        return typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(System.Nullable<>);
        
    }


    
    /// <summary>
    /// 创建线程池
    /// </summary>
    public void creatThreadingPool(){
        ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), null);
    }

    public void DoWork(object stateInfo)
    {
        // 这里写你的耗时操作
        Debug.Log("DoWork");
    }

}

