using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class receiveDataView : baseManager<receiveDataView>
{
    // public GameObject device_40 = ResourcesMgr.getInstance().LoadPrefab<GameObject>("Models/device/40");

    public void AddModelToSecene<T>(IEnumerable<object> data){
        data.ToList().ForEach((item) => {
            // Debug.Log("AddModelToSecene == "+ item);
            // var model = new T();
            Type type = typeof(T);
            Vector3 localPosition = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            Vector3 localScale = Vector3.zero;
            string deviceName = "";
            string parentModeName = "mainMap(Clone)";

            var publicProperties = type.GetProperties();
            foreach (var property in publicProperties)
            {
                // Debug.Log("property.Name =="+ property.Name +"==="+ property.GetValue(item));

                if (property.Name == "localPosition"){
                    localPosition = (Vector3)property.GetValue(item);
                    // device.transform.position = position;
                }
                if (property.Name == "localRotate"){
                    rotation = (Vector3)property.GetValue(item);
                }
                if(property.Name == "localScale"){
                    localScale = (Vector3)property.GetValue(item);
                }
                if(property.Name == "modelType"){
                    deviceName = (string)property.GetValue(item);
                    // Debug.Log("modelType =="+ deviceName);
                }
                if(property.Name == "parentModelId"){
                    parentModeName = (string)property.GetValue(item);
                    // Debug.Log("modelType =="+ deviceName);
                }
            }
            GameObject device = GameObject.Instantiate(
                ResourcesMgr.getInstance().LoadPrefab<GameObject>("Models/device/40"), 
                localPosition,
                Quaternion.Euler(rotation)
            );
            device.tag = gloab_TagName.DEVICE;
            device.name = (item as deviceInfoData).imei;
            device.transform.localScale = new Vector3(5,5,5);

            if(GameObject.Find(parentModeName) != null){
                device.transform.parent = GameObject.Find(parentModeName)?.transform;
            } else {
                device.transform.parent = GameObject.Find("mainMap(Clone)")?.transform;
            }
            // GameObject device = GameObject.Instantiate(device_40, Vector3.zero, Quaternion.identity);
            // device.tag = gloab_TagName.DEVICE;
        });
    }


    public void DeleteModelOfSence<T>(IEnumerable<object> data, string keyName){

        // GameObject.FindGameObjectsWithTag(gloab_TagName.MAIN_MAP).Find();

        data.ToList().ForEach((item) => {
            // // var model = new T();
            Type type = typeof(T);
            var publicProperties = type.GetProperties();
            string modelName = null;
            List<string> nameList = new List<string>();
            foreach (var property in publicProperties)
            {
                if (property.Name == keyName){
                    modelName = (string)property.GetValue(item);
                    nameList.Add((string)property.GetValue(item));
                    // localPosition = (Vector3)property.GetValue(item);
                }
            }

            // GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP)?.transform.Find(modelName).gameObject;

            GameObject m = globalUtils.getInstance().FindGameObjectRecursive(GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP).transform, modelName);

            GameObject.Destroy(m);
          
        });

        // GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_MAP)?.transform.Find(modelName);
        // GameObject.FindGameObjectsWithTag(gloab_TagName.MAIN_MAP).Find<string>(modelName);

        
    }



}
