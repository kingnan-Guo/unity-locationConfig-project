using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using UnityEngine;


// 继承 PureMVC 的 Facade 类
// 整个项目中独立 的 Facade 对象
public class GameFacade : Facade
{
    // 实现 facade 单例
    // public static IFacade Instance
    // {
    //     get{
    //         if(instance == null){
    //             instance = new GameFacade();
    //         }
    //         return instance;
    //     }
    // }
    public static GameFacade Instance
    {
        get{
            if(instance == null){
                instance = new GameFacade();
            }
            return instance as GameFacade;
        }
    }


    // 初始化 控制层 相关 的 内容 controller
    // 注册命令  
    // 发送 START_UP  的 通知，就会 自动 执行命令

    protected override void InitializeController()
    {

        // base 调用 父 类的 InitializeController
        base.InitializeController();

        // 这里要写一些 关于 命令和通知绑定的 逻辑
        
        RegisterCommand(PureNotification.START_UP, () =>{
            // 返回一个命令
            return new StartUpCommand();
        });


        RegisterCommand(PureNotification.SHOW_PANEL, () =>{
            // 返回一个命令
            return new showPanelCommand();
        });

        RegisterCommand(PureNotification.HIDE_PANEL, () =>{
            // 返回一个命令
            return new HidePanelCommand();
        });


        RegisterCommand(PureNotification.UPDATE_PLAYER_DATA_INFO, () =>{
            // 返回一个命令
            return new upDateDataCommand();
        });
    }

    // 一定要有  启动 函数
    public void StartUp(){
        // 发送 通知
        // 继承了 facade 类，可以直接 调用 SendNotification
        SendNotification(PureNotification.START_UP, "send info");
    }

    public void showPanel(){
        // 发送 通知
        // 继承了 facade 类，可以直接 调用 SendNotification
        // object info = new object();
        
        SendNotification(PureNotification.SHOW_PANEL, "data");
    }

    

}
