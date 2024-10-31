using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 获取当前点击或者悬浮的 设备  物体 
/// </summary>
public class getCurrentGameObject
{

    Material selectMaterial = Resources.Load<Material>("Materials/selectMaterial/selectMaterial");

    Material defaultMaterial = Resources.Load<Material>("Materials/defaultMaterial/default");

    private Transform currentTransform;
    public getCurrentGameObject(){
        EventCenterOptimize.getInstance().AddEventListener<GameObject>(gloab_EventCenter_Name.MOUSE_MOVE_POSITION_PHYSICS, (res) => {
            if(res != null){
                // Debug.Log("transformModelController == " + res.name);
                selectModel(res);
                // return;
                cardManager(res);
            } else {
                hiddenCard();
            }
        });
    }

    // 为 悬浮的  物体 添加 高亮
    // 如果 悬浮 的 物体 是设备 那么 给他  替换 shader ，shader 是 绘制 轮廓

    protected void selectModel(GameObject gameObject){

        if(gameObject != null){
            
            // 添加选中效果
            // if(currentGameObject.tag == "Axis"){
            //      Debug.Log("currentGameObject == " + currentGameObject.name);
            // }
            // if(gameObject.tag == "device" && currentGameObject){
                
            //      Debug.Log("currentGameObject == " + currentGameObject.name);
            //      currentGameObject.GetComponent<Renderer>().material = selectMaterial;
            //     //  currentGameObject.GetComponent
            // } else {
            //     currentGameObject.GetComponent<Renderer>().material = defaultMaterial;
            // }


            if(currentTransform !=null && currentTransform.gameObject.CompareTag(gloab_TagName.DEVICE)){
                currentTransform.gameObject.GetComponent<Renderer>().material = defaultMaterial;
                
            }

            currentTransform = gameObject.transform;


            if(gameObject.CompareTag(gloab_TagName.DEVICE)){
                
                 currentTransform.GetComponent<Renderer>().material = selectMaterial;
                 
                //  currentGameObject.GetComponent
            } 
            
            // else {
            //     currentTransform.GetComponent<Renderer>().material = defaultMaterial;
            // }


        } else{
            if(currentTransform !=null && currentTransform.gameObject.CompareTag(gloab_TagName.DEVICE)){
                currentTransform.gameObject.GetComponent<Renderer>().material = defaultMaterial;
                
            }
            // // 清除选中效果
            // if(currentGameObject != null && currentGameObject = ;){
            //     currentGameObject.GetComponent<Renderer>().material = defaultMaterial;
            // }
        }
    }

    private void cardManager(GameObject gameObject){
        if(gameObject.CompareTag(gloab_TagName.DEVICE)){
            showCard(gameObject);
        } else{
            hiddenCard();
        }
    }


    // 显示卡片
    protected async void showCard(GameObject gameObject){
        Transform dataCard = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS).transform.Find("dataCard");
        if(dataCard.gameObject.activeSelf == false){
            // 数据库搜索 imei
            // IEnumerable<object> deviceInfoDataList = receiveDataController.getInstance().seletByIMEI(gameObject.name);
            // deviceInfoDataList.ToList().ForEach((deviceInfoData) => {
            //     deviceInfoData deviceInfo = deviceInfoData as deviceInfoData;
            //     dataCard.Find("GameObject/deviceNameValue").GetComponent<Text>().text = deviceInfo.deviceName;
            //     dataCard.Find("GameObject/imeiValue").GetComponent<Text>().text = deviceInfo.imei;
            //     dataCard.Find("GameObject/parentValue").GetComponent<Text>().text = deviceInfo.parentModelName;
            // });



            List<networkDeviceDataInfo> dataInfos = await getDataInfoOfIMEI(gameObject.name);
            dataInfos.ToList().ForEach((deviceInfoData) => {
                networkDeviceDataInfo deviceInfo = deviceInfoData as networkDeviceDataInfo;
                dataCard.Find("GameObject/deviceNameValue").GetComponent<Text>().text = deviceInfo.deviceName;
                dataCard.Find("GameObject/imeiValue").GetComponent<Text>().text = deviceInfo.imei;
                dataCard.Find("GameObject/parentValue").GetComponent<Text>().text = deviceInfo.parentModelId;
            });





            dataCard.gameObject.SetActive(true);
        }


    }

    protected void hiddenCard(){
        // Debug.Log("hiddenCard == ");
        if(GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS) != null &&GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS).transform?.Find("dataCard") != null){
            GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS)?.transform?.Find("dataCard")?.gameObject?.SetActive(false);
        }
    }


    private async Task<List<networkDeviceDataInfo>> getDataInfoOfIMEI(string imei){
        // 获取设备信息
        deviceInfoParams infoParams = new deviceInfoParams();
        infoParams.imei = imei;
        infoParams.pageSize = 0;
        infoParams.isLabeled = 1;
        List<networkDeviceDataInfo> data = await receiveDataFromNetworkController.getInstance().aysncGetDeviceList(infoParams);
        // Debug.Log("getDeviceList 拿到数据完成 =="+ System.DateTime.Now);
        Debug.Log("getDataInfoOfIMEI 拿到数据完成 =="+ data );

        return data;

    }

    

}
