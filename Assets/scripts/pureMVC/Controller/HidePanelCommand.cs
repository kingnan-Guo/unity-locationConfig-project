using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using PureMVC.Patterns.Mediator;
using UnityEngine;

public class HidePanelCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        
        // 隐藏 的目的  
        // 得到 mediator 再得到 mediator 中的 view
        // 然后 隐藏 或者 删除


        Mediator mediator =  notification.Body as Mediator;

        if(mediator != null && mediator.ViewComponent != null){
            // GameObject.Destroy(mediator.ViewComponent.gameObject);

            GameObject.Destroy((mediator.ViewComponent as MonoBehaviour).gameObject);
            // 删除后 要 至空
            mediator.ViewComponent = null;
        }
        
    }
}
