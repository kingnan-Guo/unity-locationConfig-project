using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class std
{
    public int a;
    public string n;
}

public class myClass
{
    public string name;
    public int age;
    public bool sex;
    public float testf;
    public List<int> ids;
    public int[] intArr;

    public Dictionary<int, string> dic;

    public std stdu;

    public List<std> stdu2;

    [SerializeField]
    private int privateInt;

}


public class jsonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {




        myClass ts = new myClass();
        ts.name = "asd";
        ts.age = 99;
        ts.sex = false;
        ts.testf = 1.0f;
        ts.ids = new List<int>() { 1, 2, 3 };
        ts.intArr = new int[] { 1, 2, 3, 4 };

        ts.stdu = new std();
        ts.stdu.a = 1;
        ts.stdu.n = "sdd";

        ts.stdu2 = new List<std>() {
            new std()
        };
        string st = JsonUtility.ToJson(ts);
        // Debug.Log(st);
        File.WriteAllText(Application.persistentDataPath + "/Test.json", st);

        Debug.Log("Application.persistentDataPath == " + Application.persistentDataPath);

        string data = File.ReadAllText(Application.persistentDataPath + "/Test.json");
        Debug.Log(data);

        myClass mc = JsonUtility.FromJson(data, typeof(myClass)) as myClass;
        print(mc.age);

        myClass mc2 = JsonUtility.FromJson<myClass>(data);
        print(mc2.age);


        // 反序列化
        


    }

    // Update is called once per frame
    void Update()
    {

    }
}


// 不支持 字典