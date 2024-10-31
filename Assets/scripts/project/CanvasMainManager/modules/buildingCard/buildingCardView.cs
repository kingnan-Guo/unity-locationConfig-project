using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class buildingCardView : baseManager<buildingCardView>
{
    private Transform BuildingCard;
    private Transform floorCard;


    // Assets/resources/Prefabs/canvas/components/buildingButton.prefab
    public GameObject buildingButtonPrefab = ResourcesMgr.getInstance().LoadPrefab<GameObject>("Prefabs/canvas/components/buildingButton");

    public GameObject floorButtonPrefab = ResourcesMgr.getInstance().LoadPrefab<GameObject>("Prefabs/canvas/components/floorButton");


    private Transform floorButtonActive;
    private Transform buildingButtonActive;

    public buildingCardView(){
        
        //初始化

        EventCenterOptimize.getInstance().AddEventListener<Dictionary<string, object>>(gloab_EventCenter_Name.BUILDING_INFO_DICTIONARY, (res) =>{
            // Debug.Log("buildingInfo ===="+ res);
            showBuildingCardView(res);
            
        });

        
        //楼幢 卡片父节点  有问题
        BuildingCard = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS).transform.Find("BuildingCard");
        //楼层 卡片父节点   有问题
        floorCard = GameObject.FindGameObjectWithTag(gloab_TagName.MAIN_CANVAS).transform.Find("floorCard").Find("box");
    }
    /// <summary>
    /// 显示楼幢 卡片
    /// </summary>
    public void showBuildingCardView(Dictionary<string, object> res){


        // Debug.Log("showBuildingCardView ================"+ res.Count);

        float count =  1;
        res.ToList().ForEach((item) => {

            buildingInfo info = item.Value as buildingInfo;
            // 创建楼幢卡片
            Button btn = createButton(info.name, info.name, info.type, BuildingCard);
            btn.GetComponent<RectTransform>().localPosition = new Vector3((count - ((res.Count  + 1) / 2.0f ) ) * 80, 0, 0);
            btn.onClick.AddListener(() => ButtonClicked(btn.name));
            
            count ++;
        });
        


        

    }

    public void ButtonClicked(string name){
        // Debug.Log("ButtonClicked = "+ ((buildingInfo)gloab_static_data.buildingDictionary[name]).position);


        
        // gloab_static_data.buildingDictionary.ToList().ForEach((item) => {
        //     if((item.Value as buildingInfo).name == name){

        //         Debug.Log("ButtonClicked = "+ (item.Value as buildingInfo).position);
        //     }
        // });


    }

    public void setActiveBuilding(string name){


        if(buildingButtonActive != null ){
            // 判定 是否有变化
            if(buildingButtonActive.name != name){
                // buildingButtonActive.GetComponent<Renderer>().material
                // 还原 上一个  按钮的 背景图片
                buildingButtonActive.Find("buildingNameText").GetComponent<Text>().color = Color.white; 

                buildingButtonActive = BuildingCard.Find(name);
                // 把 buildingButtonActive 设置成 active 的状态

                buildingButtonActive.Find("buildingNameText").GetComponent<Text>().color = Color.yellow;

                showFloorCardView(name);
            } else {
 
            }
        } else {
            buildingButtonActive = BuildingCard.Find(name);
            // 把 buildingButtonActive 设置成 active 的状态
            buildingButtonActive.Find("buildingNameText").GetComponent<Text>().color = Color.yellow;

            showFloorCardView(name);

        }

        


        // showFloorCardView(name);
    }

    /// <summary>
    /// 显示楼层 卡片
    /// </summary>
    public void showFloorCardView(string name){
        floorList[] floorList = (floorList[])(((buildingInfo)gloab_static_data.buildingDictionary[name]).floorList);
        // floorCard.transform.DetachChildren();
        foreach (Transform child in floorCard.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        if(floorList.Count() > 0){
            // 显示 楼层的卡片
            floorCard.parent.gameObject.SetActive(true);

        } else{
            // 隐藏 楼层的卡片 
            floorCard.parent.gameObject.SetActive(false);
        }


        float count = 1;
        floorList.ToList().ForEach((item) => {
            // Debug.Log("floorList = item "+ item.name);
           GameObject go = createGameObjectPrefab(floorButtonPrefab, "F "+item.name, "F "+count, floorCard, gloab_TagName.CANVAS_FLOOR_BUTTON);
           go.transform.localPosition = new Vector3(0, (count - ((floorList.Length  + 1) / 2.0f ) ) * 30, 0);
           count ++;
        });

    }


    // 创建 canvas 下的  按钮 

    public Button createButton(string name, string showText, string typeCode,Transform parent){
        // Debug.Log("createButton  typeCode = "+ typeCode);
     // 创建按钮
        // Button button = new GameObject(name, typeof(RectTransform), typeof(Button), typeof(Text)).GetComponent<Button>();
        // button.transform.SetParent(parent, false);
        // button.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        // button.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        // button.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);

        // Button buildingButton = buildingButton;

        // buildingButton.transform.SetParent(parent, false);
        // buildingButton.name = name;
        // buildingButton.GetComponentInChildren<Text>().text = showText;


        // Debug.Log("buildingButtonPrefab = " + buildingButtonPrefab);
        GameObject newButton = GameObject.Instantiate(buildingButtonPrefab, Vector3.zero, Quaternion.identity);
        newButton.tag = gloab_TagName.CANVAS_BUILDING_BUTTON;
        if(typeCode == gloab_TagName.UNDER_GROUND){
            newButton.tag = gloab_TagName.CANVAS_UNDER_GROUND_BUTTON;
        }
        // 设置新Button的父级为Canvas，以确保它出现在UI之上
        newButton.transform.SetParent(parent, false);
        newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(80f, 50f);
        newButton.name = name;
        
        // 获取Button组件以便进一步配置
        Button btn = newButton.GetComponent<Button>();
        Text text = btn.GetComponentInChildren<Text>();

        text.text = showText;

        text.fontSize = 20;
        text.color = Color.white;
        // text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        // 为Button添加点击事件
        
        // // 为Button添加点击事件
        // btn.onClick.AddListener(() => { Debug.Log("Button Clicked"); });


        // Text text = new GameObject("Text", typeof(Text)).GetComponent<Text>();
        // text.transform.SetParent(button.transform, false);
        // text.text = showText;
        

        // 添加按钮点击事件
        // button.onClick.AddListener(() => { Debug.Log("Button Clicked"); });
        return btn;
    }

    /// <summary>
    /// 创建楼层按钮
    /// </summary>
    /// <param name="name"></param>
    /// <param name="showText"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject createGameObjectPrefab(GameObject GameObjectPrefab,string name, string showText, Transform parent, string tagNme){
        GameObject go = GameObject.Instantiate(GameObjectPrefab, Vector3.zero, Quaternion.identity);
        go.tag = tagNme;
        // 设置新Button的父级为Canvas，以确保它出现在UI之上
        go.transform.SetParent(parent, true);
        // go.transform.parent = parent; // 设置新Button的父级为Canvas，以确保它出现在UI之上
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 30f);
        go.name = name;
        Text text = go.GetComponentInChildren<Text>();
        text.text = showText;
        text.fontSize = 20;
        text.color = Color.white;
        return go;
    }
    


    //// ==================================================================


    private Transform currentFloorHover;
    private Transform currentFloorGameTransform;
    /// <summary>
    /// 按钮 切换 样式  函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform"></param>
    public void buttonMaterialFunction<T>(T transform){
        // Debug.Log("buttonMaterialFunction");
        // Debug.Log("transform = " + transform);

        // (transform as Transform).GetComponent<Image>();

        findTransformTarget<Transform>(
            (transform as Transform),
            out currentFloorHover,
            out currentFloorGameTransform,
            currentHoverTow: currentFloorHover,
            gloab_TagName.CANVAS_FLOOR_BUTTON,//"2dFloorButton",
            () => {
                // changeFloorMaterial()
                // 查找 callbackBeforeData 下的 Text 
                // currentFloorHover.Find("floorNameText").GetComponent<Text>().color = Color.white;
                if(currentFloorHover != null){
                    // changeFloorMaterial<Transform, Text>(ref currentFloorHover, Color.yellow);
                    currentFloorHover.Find("floorNameText").GetComponent<Text>().color = Color.white;
                }
            },
            (Transform callbackCurrentData) => {
                callbackCurrentData.Find("floorNameText").GetComponent<Text>().color = Color.yellow;
                // changeFloorMaterial<Transform, Text>(ref currentFloorHover, Color.yellow);
            },
            () => {
                if(currentFloorGameTransform != null){
                    // changeFloorMaterial<Transform, Text>(ref currentFloorGameTransform, Color.yellow);
                    Transform floorNameText = currentFloorGameTransform.Find("floorNameText");
                    if(floorNameText != null){
                        floorNameText.GetComponent<Text>().color = Color.white;
                    }
                }
            }
        );


        // if(currentFloorGameTransform != null){
        //     currentFloorGameTransform.Find("floorNameText").GetComponent<Text>().color = Color.white;
        // }
        if(floorButtonActive != null){

            // changeFloorMaterial<Transform, Text>(ref floorButtonActive, Color.yellow);
             floorButtonActive.Find("floorNameText").GetComponent<Text>().color = Color.yellow;
        }


    }



    public  void findTransformTarget<T>(
        Transform transformData, 
        out Transform currentHover,
        out Transform currentGameTransform,
        Transform currentHoverTow,
        string tag = null, 
        UnityAction callbackBefore = null, 
        UnityAction<Transform> callbackCurrent= null, 
        UnityAction callbackLast= null
    ) where T : Transform
    {

        if(transformData != null){
            if(currentHoverTow !=null && currentHoverTow?.tag == tag){
                // 返回上一个 记录 的悬浮
                callbackBefore();
                
            }
            currentGameTransform = transformData;
            //临时记录 一下目标 去判断是否符合规则
            currentHover = transformData;

            if(currentHover != null){
        
                while (currentHover != null && !currentHover.CompareTag(tag))
                {
                    currentHover =  currentHover.parent;
                }

                // Debug.Log("currentHover =="+ currentHover.name + "== ");

                if(currentHover != null &&currentHover.CompareTag(tag)){
                    //找到 目标 更改 样式
                    callbackCurrent(currentHover);
                }
                else {
                    if(currentGameTransform !=null && currentGameTransform.tag == tag){
                        callbackLast();
                    }
                }
            } else{
                Debug.Log("没有父级");
            }
        } else {

            callbackLast();
            currentGameTransform = transformData;
            currentHover = transformData;
        }
    }



    public void setButtonActive<T>( T transform){

        if(floorButtonActive != null){
            // changeFloorMaterial<Transform, Text>(ref floorButtonActive, Color.white);
            floorButtonActive.Find("floorNameText").GetComponent<Text>().color = Color.white;
        }

        this.floorButtonActive = transform  as Transform;
    }

    /// <summary>
    /// 修改悬浮楼层 的  颜色
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform"></param>
    /// <param name="color"></param>
    public void changeFloorMaterial<T, V>(ref T transform, Color color){
        (transform as Transform).Find("floorNameText").GetComponent<Text>().color = Color.white;
    }




    public Transform getBuildingButtonActiveTransform(){
        if(buildingButtonActive != null){
            return buildingButtonActive;
        }
        return null;
    }



    public Transform FindTransform(string transformName){
        Transform ts = null;
        ts = GameObject.Find(transformName).transform;
        return ts;
    }
}
