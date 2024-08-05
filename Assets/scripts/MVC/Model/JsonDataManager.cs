using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDataManager
{
    // Start is called before the first frame update
    private static JsonDataManager instance = new JsonDataManager();
    public static JsonDataManager  Instance => instance;
    public Dictionary<string, object> dataDic = new Dictionary<string, object>();
    private JsonDataManager(){
    }
    public T LoadData<T>(string jsonString, JsonType type = JsonType.JsonUtility) where T : new()
    {
        if(jsonString.Equals("")){
            return new T();
        }
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtility:
                data = JsonUtility.FromJson<T>(jsonString);
                break;
            default:
                break;
        }
        // return default(T);
        return data;
    }
    public void SaveData(){
        
    }
}
