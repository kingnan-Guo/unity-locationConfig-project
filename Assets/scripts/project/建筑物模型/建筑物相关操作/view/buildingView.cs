using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




/// <summary>
/// 建筑物 模型的 操作
/// </summary>
public class buildingView : baseManager<buildingView>
{

    private Material material_back = Resources.Load<Material>("Materials/buildingMaterial/floor_bl");
    private Material material_coustom = Resources.Load<Material>("Materials/buildingMaterial/buildingActiveMaterial");

    public buildingView(){

    }

    /// <summary>
    /// 还原 建筑物的材质
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    public void backToBuildMaterial<T>(T gameObject){
        changeMaterial(gameObject, material_back);
    }


    public void changeToBuildMaterialToSelect<T>(T gameObject){
        // Material material = (gameObject  as GameObject).gameObject.GetComponent<Renderer>().material;
        changeMaterial(gameObject, material_coustom);
    }

    /// <summary>
    /// 改变建筑物的材质
    /// </summary>
    public void changeMaterial<T>(T data, Material material ){
        (data as GameObject).GetComponent<Renderer>().material = material;
        // Material material_coustom = (data as GameObject).GetComponent<Renderer>().material;
        // material_coustom.CopyPropertiesFromMaterial(material);
    }



    

    /// <summary>
    /// 改变建筑物的指定 的 属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="buildingName"></param>
    /// <param name="modelName"></param>
    public void changeModelAssemblyAttribute<T, A, S>(T data, A  attribute, S value){
        // typeof(T);

        System.Type type = typeof(T);

        Debug.Log("changeModelAssemblyAttribute type =="+ type);
        // System.Reflection.FieldInfo[] FieldInfoArr = type.GetFields();
        // foreach (var item in FieldInfoArr)
        // {

        //     Debug.Log("changeModelAssemblyAttribute item =="+ item.Name);
        //     // item.SetValue(res, TClass.GetMethod("get"+ item.Name)?.Invoke(res, new object[] { data}));
        // }
        // if(type.GetMethod(attribute as string)){
        //     type.GetMethod(attribute as string)?.Invoke(data, new object[] { value});
        // }

        // System.Reflection.MemberInfo[] memberInfos = type.GetMembers();
        // foreach (var item in memberInfos)
        // {

        //     Debug.Log("changeModelAssemblyAttribute GetMembers = " + item);
        // }

        //    Debug.Log("changeModelAssemblyAttribute GetMember = " + type.GetMember(attribute as string).SetValue(
        //        data, new object[] { value}
        //    );


        // System.Reflection.PropertyInfo propertyInfo = type.GetProperty("name");
        // propertyInfo?.SetValue((data as GameObject).transform, "New Name", null);

        
        // System.Reflection.FieldInfo[] FieldInfoArr = type.GetFields();
        // foreach (var item in FieldInfoArr)
        // {
        //     Debug.Log("FieldInfo.Name = " + item.Name);

        // }
        // System.Reflection.MemberInfo[] memberInfos = type.GetMembers();
        // foreach (var item in memberInfos)
        // {
        //     Debug.Log("GetMembers = " + item.GetType());
        // }

        
        // Debug.Log("GetMembers = " + type.GetMember("name")[0].MemberType());


        // System.Reflection.FieldInfo fieldInfo_key = type.GetField("Transform");
        // Debug.Log("FieldInfo.Name = " + fieldInfo_key.Name);


        System.Reflection.MemberInfo[] membersArr = type.GetMember(attribute as string );
        foreach (var item in membersArr)
        {
            // Debug.Log("GetMembers = " + item.GetType());
            if (item.MemberType == System.Reflection.MemberTypes.Property)
            {
                // System.Reflection.PropertyInfo propertyInfo = type.GetProperty("name");
                (item as System.Reflection.PropertyInfo)?.SetValue(data, value, null);

            }
            else if (item.MemberType == System.Reflection.MemberTypes.Method)
            {
                type.GetMethod(attribute as string)?.Invoke(data, new object[] { value});
            }
        }

        // if (members.Length > 0)
        // {
        //     System.Reflection.MemberInfo member = members[0];
        //     if (member.MemberType == MemberTypes.Property)
        //     {
        //         Console.WriteLine("成员是属性");
        //     }
        //     else if (member.MemberType == MemberTypes.Method)
        //     {
        //         Console.WriteLine("成员是方法");
        //     }
        // }


        // Debug.Log("==="+ type.GetMember("name").SetValue((data as GameObject).name, )
        //  Debug.Log("type.GetField(attribute as string).GetValue(data) = " + fieldInfo_key);

        // type.GetField(attribute as string).SetValue((data as GameObject).name, value as  string);


        //  Debug.Log("type.GetField(attribute as string).GetValue(data) = " + type.GetField(attribute as string).GetValue(data as GameObject) );

        
        
    }

    public void HiddenOrShowBuilding<T>(T data){
        (data as IEnumerable<object>).ToList().ForEach((item) => {
            
            // Debug.Log("item.GetType() =="+ typeof(item));
            BuildingInfoClass info = (item as BuildingInfoClass);
            // Debug.Log("HiddenOrShowBuilding ==" + info.FloorRelativePositionMark);
            if(info.FloorRelativePositionMark <=0){
                // 显示
                showBuilding<string>(info.FloorName);
            } else {
                // 隐藏
                hideBuilding<string>(info.FloorName);
            }

            if(info.FloorRelativePositionMark ==0) {

                GameObject.Find("LookAtCube").transform.position = new Vector3(80, 66, -61);
                GameObject.Find("LookAtCube").transform.rotation = Quaternion.Euler(0, 0, 0);

                Camera.main.transform.position = new Vector3(80, 66, -61) + new Vector3(0,0, -50);
                // GameObject.Find("LookAtCube").transform.rotation = Quaternion.Euler(0, 0, 0);
                 
                Camera.main.transform.RotateAround(GameObject.Find("LookAtCube").transform.position, Vector3.right, 90);
                Camera.main.transform.LookAt(GameObject.Find("LookAtCube").transform.position);

            }

        });
    }

    /// <summary>
    /// 显示建筑物
    /// </summary>
    /// <param name="buildingName"></param>
    public void showBuilding<T>(T data){

        if(!FindeTransform(data as string).gameObject.activeInHierarchy){
            changeModelAssemblyAttribute<GameObject, string, bool>(FindeTransform(data as string).gameObject, "SetActive", true);
            // FindeTransform(data as string).gameObject.SetActive(true);
        }


    }
    /// <summary>
    /// 隐藏建筑物
    /// </summary>
    /// <param name="buildingName"></param>
    public void hideBuilding<T>(T data){
        if(FindeTransform(data as string).gameObject.activeInHierarchy){
            changeModelAssemblyAttribute<GameObject, string, bool>(FindeTransform(data as string).gameObject, "SetActive", false);
            // FindeTransform(data as string).gameObject.SetActive(false);
            // changeModelAssemblyAttribute<Transform, string, string>(FindeTransform(data as string), "name", "newName");
        }
    }

    /// <summary>
    /// 查找建筑物
    /// </summary>
    /// <param name="buildingName"></param>
    /// <returns></returns>
    public Transform FindeTransform(string buildingName){
        return GameObject.Find(buildingName).transform;
    }


    /// <summary>
    /// 把 其他楼幢 的 模型显示 出来 
    /// </summary>
    /// <param name="buildingName"></param>
    /// <param name="isShow"></param>
    public void showOtherBuilding(string buildingName, bool isShow){
        
    }

}
