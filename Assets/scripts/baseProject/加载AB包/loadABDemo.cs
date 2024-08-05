using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadABDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string name = "Assets/resources/dahuyuahqu.prefab";
        // name = "Assets/dahuyuahqu 1.fbx";

        GameObject obj = ABManager.GetInstance().LoadRes("dahuamap", name) as GameObject;
        obj.transform.position = new Vector3(0, 0, 0);



        // Debug.Log("obj.transform.childCount =="+ obj.transform.childCount);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if(obj.transform.GetChild(i).name.Contains("floor")){
                obj.transform.GetChild(i).tag ="building";
            }
        }

        string tree = "Assets/resources/tree.prefab";
        GameObject treeObj = ABManager.GetInstance().LoadRes("treemap", tree) as GameObject;
        treeObj.transform.position = new Vector3(-7.26f, -4.2f, -4.22f);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
