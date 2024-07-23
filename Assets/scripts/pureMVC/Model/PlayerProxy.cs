using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Proxy;
using UnityEngine;


/// <summary>
/// 玩家数据代理
/// 主要处理 玩家数据更新相关的 逻辑
///     1 继承 Proxy 父类
///     2、写构造函数 ; 
            // 代理的 名字
            // 代理相关的数据 
/// </summary>
public class PlayerProxy : Proxy
{
    // 重写父类 使用 new
    public static new string NAME = "PlayerProxy";

    // 2、写构造函数 ;

    // base 关联 父类 的 有参 构造；:base() 会先调用 父类的 构造 函数

    // public PlayerProxy(PlayerDataObj data): base(PlayerProxy.NAME,  data) 可以从外部 传值
    public PlayerProxy(): base(PlayerProxy.NAME)
    {
        // 初始化 外部传入的 数据
        PlayerDataObj data =  new PlayerDataObj();

        // 初始化
        data.playerName = PlayerPrefs.GetString("playerName", "kingnan");

        data.playerLevel = PlayerPrefs.GetInt("playerLevel", 1);
        data.playerExp = PlayerPrefs.GetInt("playerExp", 0);
        // 赋值 给自己的 Data 进行 关联; data 来自于 父类
        Data = data;
    }

    public void add(){
        PlayerDataObj data = Data as PlayerDataObj;
        data.playerLevel += 1;

        data.playerExp += data.playerLevel;
        // saveData();
    }

    public void saveData(){
        PlayerDataObj data = Data as PlayerDataObj;
        PlayerPrefs.SetString("playerName", data.playerName);
        PlayerPrefs.SetInt("playerLevel", data.playerLevel);
        PlayerPrefs.SetInt("playerExp", data.playerExp);
    }

    // 3、重写父类的方法
    // 4、写自己的 逻辑



}
