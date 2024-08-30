using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class panelFirst : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {

        // Button button = GetControl<Button>("Button1 (Legacy)");
        // Debug.Log(button);
        // button.onClick.AddListener(() => {
        //     Debug.Log("Button1");
        // });


        GetControl<Button>("Button1");



        // 添加 自定义 按钮事件
        // EventTrigger eventTrigger = GetControl<Button>("Button1").gameObject.AddComponent<EventTrigger>();
        // EventTrigger.Entry entry = new EventTrigger.Entry();
        // entry.eventID = EventTriggerType.Drag;
        // entry.callback.AddListener((data) => {
        //     Debug.Log("Button1 Drag 鼠标 拖动");
        // });
        // eventTrigger.triggers.Add(entry);


        // // 添加 自定义 按钮事件
        // EventTrigger eventTrigger2 = GetControl<Button>("Button1").gameObject.AddComponent<EventTrigger>();
        // EventTrigger.Entry entry2 = new EventTrigger.Entry();
        // entry2.eventID = EventTriggerType.PointerClick;
        // entry2.callback.AddListener((data) => {
        //     Debug.Log("Button1 PointerClick");
        // });

        UIManager.AddCustomEventListener(GetControl<Button>("Button2"), EventTriggerType.PointerEnter, (data) => {
            Debug.Log("Button2 PointerEnter");
        });

    }

    protected override  void Awake()
    {
        base.Awake();
        Debug.Log(" 子类 自己的 Awake");
    }

    public void initInfo(){
        Debug.Log("initInfo");
    }

    // 重写 show
    public override void Show()
    {
        base.Show();
        // 显示面板时 想要执行的逻辑， 这个函数 在  UI 管理器 中会自动  帮我们 调用
        // 只要 重写 就会 执行 以下  逻辑

        Debug.Log("public override void Show");

    }


    protected override void OnClick(string buttonMame)
    {
        base.OnClick(buttonMame);
        // 重写 按钮点击事件
        // 只要 重写 就会 执行 以下  逻辑
        Debug.Log("public override void OnClick(string name) = "+ buttonMame);


        switch (buttonMame)
        {
            case "Button1":
                Debug.Log("Button1");
                break;
            case "Button2":
                Debug.Log("Button2");
                break;
            default:
                break;
        }
    }


    protected override void onValueChanged(string name, bool value){
        base.onValueChanged(name, value);

        Debug.Log("public override void onValueChanged(string name) = "+ name + " =" + value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
