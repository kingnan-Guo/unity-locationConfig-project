using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// 面板 基类
/// 提供找到 所有自己 面板下面的 控件对象
/// 提供显示 隐藏的接口
/// 提供 打开 关闭 面板的接口
/// 提供 面板 之间的 切换接口
/// 提供 面板 之间的 数据传递接口
/// </summary>
public class BasePanel : MonoBehaviour
{
    // 里氏转换 原则 存储 所有的 空间； UIBehaviour 是所有空间的 父类
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        // 获取下面所有 子控件 挂在的脚本
        this.FindChildControl<Button>();
        this.FindChildControl<Image>();
        this.FindChildControl<Text>();
        this.FindChildControl<Toggle>();
        this.FindChildControl<InputField>();

        Debug.Log("controlDic.Count = " + controlDic.Count);
    }


    /// <summary>
    /// 找到 子对象的 对应控件
    /// 
    ///  where T : UIBehaviour 约束 T 必须是 UIBehaviour 或者 其子类
    ///  
    /// 一个 名字 对应 多个组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildControl<T>() where T : UIBehaviour
    {
        // 获取下面所有 子控件 挂在的脚本
        T[] controls = this.GetComponentsInChildren<T>();
        
        // Debug.Log("buttons.Length = " + buttons.Length);
        foreach (T cs in controls)
        {
            string objectName = cs.gameObject.name;
            // 判断 字典中 是否 包含 当前 控件
            if(controlDic.ContainsKey(objectName)){
                controlDic[objectName].Add(cs);
            } 
            else{
                List<UIBehaviour> list = new List<UIBehaviour>();
                list.Add(cs);
                controlDic.Add(objectName, list);

                // 如果是 按钮  直接 添加 监听;
                if(cs is Button){
                    (cs as Button).onClick.AddListener(() => {
                        OnClick(objectName);
                    });
                }

                if(cs is Toggle){
                    (cs as Toggle).onValueChanged.AddListener((value) => {
                        onValueChanged(objectName, value);
                    });
                }
            }
        }

        // for (int i = 0; i < controls.Length; i++)
        // {
        //     string objectName;
        //     // T cs = controls[i];
        //     objectName = controls[i].gameObject.name;
        //     // 判断 字典中 是否 包含 当前 控件
        //     if(controlDic.ContainsKey(objectName)){
        //         controlDic[objectName].Add(controls[i]);
        //     } 
        //     else{
        //         List<UIBehaviour> list = new List<UIBehaviour>();
        //         list.Add(controls[i]);
        //         controlDic.Add(objectName, list);
        //         Debug.Log("objectName = " + objectName);
        //         // 如果是 按钮  直接 添加 监听;
        //         if(controls[i] is Button){
        //             (controls[i] as Button).onClick.AddListener(() => {
        //                 OnClick(objectName);
        //             });
        //         }
        //     }
        // }
    }


    /// <summary>

    protected virtual void OnClick(string buttonName){

    }
    protected virtual void onValueChanged(string buttonName, bool value){

    }


    /// <summary>
    /// 获取 控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    protected T GetControl<T>(string name) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(name))
        {
            foreach (UIBehaviour cs in controlDic[name])
            {
                if (cs is T)
                {
                    return cs as T;
                }
            }
        }
        return null;
    }


    /// <summary>
    /// 显示 隐藏
    /// </summary>

    public virtual void Show()
    {
        // this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        // this.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

