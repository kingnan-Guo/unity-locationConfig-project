using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using Unity.VisualScripting;
using UnityEngine;

public class showPanelCommand : SimpleCommand
{
    // Start is called before the first frame update
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        // Debug.Log("showPanelCommand =" +  notification.Body);
        // bool isShow = (bool)notification.Body;
        // GameObject.Find("Canvas").transform.Find("Panel").gameObject.SetActive(true);
        
        string name = (string)notification.Body;


        Debug.Log("showPanelCommand = name =" +  name);
        switch (name)
        {
            case "data":
                // 使用 mediator ，要去 facade 中去注册
                // command proxy 都是一样的  要用就要注册

                // 可以 在命令中 直接 使用  Facade 代表 就是 唯一 Facade 实例
                //得到单例
                // 通过 Mediator 的名字 查找， 是否注册过 
                if(!Facade.HasMediator(newMainViewMediator.NAME)){
                    Facade.RegisterMediator(new newMainViewMediator());// 注册后  可以通过 Mediator 的名字得到 它
                }

                // Facade 里 有 得到 mediator 的方法
                newMainViewMediator nmm = Facade.RetrieveMediator(newMainViewMediator.NAME) as newMainViewMediator;


                // 证明 没有  实例话过 对象
                if(nmm.ViewComponent == null){
                    // 创建 预设体
                    GameObject res = Resources.Load<GameObject>("Prefab/UI/Panel");
                    GameObject obj = GameObject.Instantiate(res);

                    obj.transform.SetParent(GameObject.Find("Canvas").transform);
                    obj.transform.localPosition = new Vector3(0, 0, 0);
                    obj.transform.localScale = new Vector3(1, 1, 1);


                    // 得到预设体上的 view 脚本， 关联到 mediator
                    // nmm.ViewComponent = obj.GetComponent<newMainView>();
                    // 创建 预设体
                    nmm.SetView(obj.GetComponent<newMainView>());  
                    
                }



                // PlayerProxy  pp= Facade.RetrieveProxy(PlayerProxy.NAME) as PlayerProxy;
                // // 得到 数据
                // PlayerDataObj dd = pp.Data as PlayerDataObj;
                // Debug.Log(dd +" =="+ dd.playerName);


                // 显示 面板 后需要更新数据; 把数据 一起 传过去
                // 通过 数据注册 的名称 得到数据
                SendNotification(PureNotification.UPDATE_PLAYER_INFO, Facade.RetrieveProxy(PlayerProxy.NAME).Data);

                break;
            case "data2":
        
                Debug.Log("showPanelCommand  data2");

                Debug.Log(" 更改  数据");

                break;
            default:
                break;
        }
    }
}
