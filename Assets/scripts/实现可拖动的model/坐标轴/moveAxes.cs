using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveAxes : AxesBaseClass
{

    private Vector3 screenPosition;
    protected override void OnDrag(){
        // 1、 要 点击
        // 2、 要 知道 点击的哪个轴
        // 3、 获取点击了哪个物体
        if(Input.GetMouseButtonDown(0)){
            Debug.Log("点击了"+ currentMoveAxisParent.name);
            // currentMoveAxisParent = Transform.parent;

            // 记录 当前 点的位置

            // Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition)

            // screenPosition
            // screenPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);


            // 获取点击了 哪个轴  然后 绘制直线
            


        }
    }
}
