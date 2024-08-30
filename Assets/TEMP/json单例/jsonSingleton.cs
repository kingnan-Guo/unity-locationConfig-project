using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




// 單例模式
public class JsonManager{
    private static JsonManager instance = new JsonManager();
    public static JsonManager  Instance => instance;
    private JsonManager(){

    }

    // 存儲 json 數據 序列化
    public void SaveData(object data, string fileName, JsonType type = JsonType.JsonUtility){
        // 存儲路徑
        string path = Application.persistentDataPath + "/" + fileName + ".json";

        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtility:
                jsonStr = JsonUtility.ToJson(data);
                break;
            default:
                break;
        }

        File.WriteAllText(path, jsonStr);

    }

    public T LoadData<T>(string fileName, JsonType type = JsonType.JsonUtility) where T : new()
    {
        string path = Application.streamingAssetsPath  + "/" + fileName + ".json";

        if(!File.Exists(path)){
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }

        if(!File.Exists(path)){
            return new T();
        }

        string jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtility:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            default:
                break;
        }

        // return default(T);
        return data;
    }



}


public class myClass2
{
    public string name;
    public int age;
    public bool sex;
}




public class jsonSingleton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        myClass2 ts2 = new myClass2();
        ts2.name = "asd";
        ts2.age = 99;
        ts2.sex = false;

        JsonManager.Instance.SaveData(ts2, "aass");

        myClass2 mc2 = JsonManager.Instance.LoadData<myClass2>("aass");

        print("mc2 -=="+ mc2.age);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
