using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

public class upDateDataCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        // 更新数据
        base.Equals(notification);


        Debug.Log("upDateDataCommand");
        PlayerProxy playerProxy = Facade.RetrieveProxy(PlayerProxy.NAME) as PlayerProxy;
        playerProxy.add();

        Debug.Log(" ==" + (playerProxy.Data as PlayerDataObj).playerLevel);

        // // // 通知 界面更新
        SendNotification(PureNotification.UPDATE_PLAYER_INFO, playerProxy.Data);
    }
}
