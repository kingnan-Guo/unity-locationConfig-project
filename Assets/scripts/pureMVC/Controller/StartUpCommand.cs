using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

// 继承 command 相关的脚本
public class StartUpCommand : SimpleCommand
{
    //重写 执行函数
    public override void Execute(INotification notification){
        //执行父类 的 Execute
        base.Execute(notification);

        // 执行 逻辑
        Debug.Log("StartUpCommand");

        // 启动命令中 一般会做 初始化 操作


        // 通过 Facade 得到 注册 的数据 。通过 注册的，名字
        if(!Facade.HasProxy("PlayerProxy")){
            
            // 注册一些操作 ; 注册数据代理
            Facade.RegisterProxy(new PlayerProxy());

            Debug.Log("注册 PlayerProxy");

        }


    }
}
