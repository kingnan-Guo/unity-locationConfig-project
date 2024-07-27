using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using UnityEngine;

//// <summary>
/// 写法
/// 1、继承 pureMVC 中的 ediator 脚本
/// 2、 写 构造函数
/// 3、 写 监听 通知的方法
/// 4、 重写 处理通知的方法
/// 5、 （可选） 重写 注册的方法
/// </summary>

public class newMainViewMediator : Mediator
{
    public static new string NAME = "newMainViewMediator";

    public newMainViewMediator() : base(NAME)
    {  
        // 这里 可以创建 页面和 预设体
        // 但是界面显示 应该是  是出发的
        //  创建 界面的代码 重复性 比较高


    }

    public void SetView(newMainView view){
        // 设置 view
        ViewComponent = view;
        Debug.Log("SetView");
        view.btn.onClick.AddListener(() => {
            // SendNotification(PureNotification.SHOW_PANEL, "data2");

            Debug.Log("SetView onClick");

            SendNotification(PureNotification.UPDATE_PLAYER_DATA_INFO);

        });
    }

    // 重写 监听 通知的方法
    public override string[] ListNotificationInterests(){
        // 这时一个 PureMVC 的 规则
        // 就是 你需要 肩痛 哪些通知 那就在这里 把通知  通过字符串数组的形式 返回出去
        // PureMVC 就会帮助 我们监听这些通知
        // 类似于 通过事件名 注册事件监听
        // return base.ListNotificationInterests();

        return new string[]{
            PureNotification.UPDATE_PLAYER_INFO,
            // PureNotification.SHOW_PANEL
        };
    }
    
    // 重写 处理通知的方法
    public override void HandleNotification(INotification notification){
        // INotification 对象 里面 包含 重要参数
        //      1、 通知名
        //      2、 通知 数据

        // base.HandleNotification(notification);
        Debug.Log("newMainViewMediator HandleNotification" + notification.Body);
        switch (notification.Name)
        {
            case PureNotification.UPDATE_PLAYER_INFO:
                // 收到 通知后 做处理 更新 信息
                if(ViewComponent !=  null){
                    (ViewComponent as newMainView).updateInfo(notification.Body as PlayerDataObj);// 脚本相关的view
                }
                break;
            default:
                break;
        }
    }


    // 可选 
    // 重写 注册的方法
    public override void OnRegister()
    {
        base.OnRegister();
        // 可以初始化一些 内容

        // 这里 可以 注册 事件监听
        // 这里 注册 事件监听 比较少见
        // 因为 一般 都是 监听 通知
        // 这里 注册 事件监听 是为了 处理 界面显示

        
    }





}
