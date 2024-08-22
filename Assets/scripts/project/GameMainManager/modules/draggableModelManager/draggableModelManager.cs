using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draggableModelManager : baseManager<draggableModelManager>
{
    public draggableModelManager(){

        Debug.Log("draggableModelManager init");

        // // 获取当前选中的物体
        // getCurrentGameObject getCurrentGameObject = new getCurrentGameObject();

        // // 给 模型 添加  xyz 轴
        // addAxesToModel addAxesToModel = new addAxesToModel();

        // // 添加模型到场景 的 class
        // // addModelToScene addModelToScene = new addModelToScene();

        // addModelToScene.getInstance();



        // 获取当前选中的物体
        getCurrentGameObject getCurrentGameObject = new getCurrentGameObject();

        // 给 模型 添加  xyz 轴
        addAxesToModel addAxesToModel = new addAxesToModel();

        // 添加模型到场景 的 class
        // addModelToScene addModelToScene = new addModelToScene();
        addModelToScene.getInstance();





    }
}
