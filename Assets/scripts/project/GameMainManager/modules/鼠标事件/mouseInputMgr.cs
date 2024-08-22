using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class mouseInputMgr : baseManager<mouseInputMgr>
{
    // Start is called before the first frame update
    public static bool isStart = false;
    public mouseInputMgr(){
        Debug.Log("mouseInputMgr 初始化");
        //初始化
        MonoManager.getInstance().AddUpdateListener(myUpdate);
    }


    /// <summary>
    /// 是否开启  输入检测
    /// </summary>
    /// <param name=""></param>
    public void StartOrEndCheck(bool isOpen){
        isStart = isOpen;
    }


    /// <summary>
    /// 检测按键
    /// 1.按下
    /// 2.弹起
    /// </summary>
    /// <param name="keyCode"></param>
    public void checkedKeyCode(){


        // Debug.Log("按下 = "+keyCode);


        // EventCenter.getInstance().EventTrigger("KeyDown", keyCode);

        // Debug.Log("弹起 = "+keyCode);
        // EventCenter.getInstance().EventTrigger("KeyUp", keyCode);
        if(Input.GetMouseButton(0)){
            EventCenter.getInstance().EventTrigger("鼠标点击左键", Input.mousePosition);
        }

        if(Input.GetMouseButton(1)){
            EventCenter.getInstance().EventTrigger("鼠标点击右键", Input.mousePosition);
        }

        if(Input.GetMouseButtonDown(0)){
            EventCenter.getInstance().EventTrigger("鼠标按下左键", Input.mousePosition);
        }

        if(Input.GetMouseButtonDown(1)){
            EventCenter.getInstance().EventTrigger("鼠标按下右键", Input.mousePosition);
        }

        if(Input.GetMouseButtonUp(0)){
            // EventCenter.getInstance().EventTrigger("鼠标弹起左键", Input.mousePosition);
            EventCenter.getInstance().EventTrigger("MouseButtonUp", Input.mousePosition);
        }
        
        if(Input.GetMouseButtonUp(1)){
            EventCenter.getInstance().EventTrigger("鼠标弹起右键", Input.mousePosition);
        }

        if(Input.GetMouseButton(2)){
            EventCenter.getInstance().EventTrigger("鼠标中键按下", Input.mousePosition);
        }

        if(Input.GetMouseButtonDown(2)){
            EventCenter.getInstance().EventTrigger("鼠标按下中键", Input.mousePosition);
        }

        if(Input.GetMouseButtonUp(2)){
            EventCenter.getInstance().EventTrigger("鼠标弹起中键", Input.mousePosition);
        }



        if(true){
            EventCenter.getInstance().EventTrigger(gloab_EventCenter_Name.MOUSE_POSITION, Input.mousePosition);
        }



        // Debug.Log(" 鼠标位置 == "+ Input.mousePosition);
        // if(Input.GetKeyDown(keyCode)){
        //     Debug.Log("按下 = "+keyCode);
        //     EventCenter.getInstance().EventTrigger("KeyDown", keyCode);
        // }

        // if(Input.GetKeyUp(keyCode)){
        //     Debug.Log("弹起 = "+keyCode);
        //     EventCenter.getInstance().EventTrigger("KeyUp", keyCode);
        // }
    }

    private void myUpdate(){
        // Debug.Log("鼠标检测");
        // if(!isStart){
        //     return;
        // }
        // checkedKeyCode(KeyCode.W);
        // checkedKeyCode(KeyCode.S);
        // checkedKeyCode(KeyCode.A);
        // checkedKeyCode(KeyCode.D);


        // checkedKeyCode(MouseButton.Left);
        // checkedKeyCode(MouseButton.Left);
        checkedKeyCode();

    }
}
