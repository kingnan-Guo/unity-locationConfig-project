using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameMainManager.GetInstance();
        getLocalModel.GetInstance(); //本地模型
        // getMainModel.getInstance(); // 网络模型
    }
}
