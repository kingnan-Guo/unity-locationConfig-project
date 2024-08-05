using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class moveAxes : AxesBaseClass
{

    private Vector3 screenPosition;
    private Vector3 initialPosition;

    private Vector3 offset;
    protected override void OnDrag(){

        if(axisType == AxisType.None){
            return;
        }
        // 1、 要 点击
        // 2、 要 知道 点击的哪个轴
        // 3、 获取点击了哪个物体
        if(Input.GetMouseButtonDown(0)){
            Debug.Log("点击了"+ currentAxisParent.name);
            // currentAxisParent = Transform.parent;

            // 记录 当前 要拖动 模型的 初始的位置
            initialPosition = currentAxisParent.transform.position;
            // screenPosition = Camera.main.WorldToScreenPoint(initialPosition);
            // screenPosition = Input.mousePosition;
            screenPosition = Camera.main.WorldToScreenPoint(initialPosition);

            Debug.Log("screenPosition =="+ screenPosition);

            offset = initialPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));


            // 获取点击了 哪个轴  然后 绘制直线
            Debug.Log("axisType =="+ axisType + "DragState =="+ DragState);

            
            DragState = DragStateType.mouseButtonDown;

            if(axisType == AxisType.X || axisType == AxisType.Y || axisType == AxisType.Z){
                // 画线
                Transform XYZAxisTransform = transform.Find(axisType.ToString());
                LineRenderer line = XYZAxisTransform.AddComponent<LineRenderer>();
                line.positionCount = 2;
                // line.material = XYZAxisTransform.GetComponent<MeshRenderer>().materials.FirstOrDefault(material => material.name.Contains("arrowShader"));
                line.material = XYZAxisTransform.GetComponent<Renderer>().material;
                line.SetPosition(0, XYZAxisTransform.transform.up * 100 + XYZAxisTransform.transform.position);
                line.SetPosition(1, XYZAxisTransform.transform.up * -100 + XYZAxisTransform.transform.position);
                line.startWidth = 0.02f;
                line.endWidth = 0.02f;
            }



        }

        // if(Input.GetMouseButton(0) && DragState == DragStateType.mouseButtonDown){
        if(Input.GetMouseButton(0)){
            // 准备拖动
            // DragState = DragStateType.isDraging;
            // Debug.Log("准备拖动 DragState =="+ DragState);

            // Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.x);

            // Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            // Vector3 initialMousePosition = Camera.main.ScreenToWorldPoint(screenPosition);

            // Debug.Log("currentPosition - initialMousePosition ===" +  currentPosition + "==" + initialMousePosition);

            // Debug.Log("Input.mousePosition  =="+ Input.mousePosition + " == "+ screenPosition);

            // Vector3 currentscreenPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
    

            // Debug.Log("currentscreenPosition - screenPosition ===" +  currentscreenPosition + "==" + screenPosition);





            // Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
            // Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            // 这个 是 基础算法 计算 鼠标 滑动的 坐标差
            // Vector3 currentPosition  = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z)) + offset - initialPosition;

            Vector3 currentAxisParentrealTimeScreentPosition  = Camera.main.WorldToScreenPoint(currentAxisParent.transform.position);
            Vector3 currentPosition  = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, currentAxisParentrealTimeScreentPosition.z)) + offset - initialPosition;



            // Debug.Log("currentPosition - initialMousePosition ===" +  currentPosition + "==" + initialPosition + " ===" + transform.position);
            Vector3 tempPos = Vector3.zero;
            switch (axisType)
            {
                case AxisType.X:
                    tempPos = Vector3.Project(currentPosition, transform.right);
                    break;
                case AxisType.Y:
                    tempPos = Vector3.Project(currentPosition, transform.up);
                    break;
                case AxisType.Z:
                    tempPos = Vector3.Project(currentPosition, transform.forward);
                    break;
                case AxisType.XY:
                    tempPos = Vector3.Project(currentPosition, transform.right) + Vector3.Project(currentPosition, transform.up);
                    break;
                case AxisType.XZ:
                    tempPos = Vector3.Project(currentPosition, transform.right) + Vector3.Project(currentPosition, transform.forward);
                    break;
                case AxisType.YZ:
                    tempPos = Vector3.Project(currentPosition, transform.up) +  Vector3.Project(currentPosition, transform.forward);
                    break;

            }
            // Debug.Log("tempPos ===" + tempPos);
            currentAxisParent.transform.position = initialPosition + tempPos;


        }

        if(Input.GetMouseButtonUp(0)){
            // 鼠标弹起
            if(axisType != AxisType.None){
                DragState = DragStateType.normal;
            }
            Transform XYZAxisTransform = transform.Find(axisType.ToString());
            if (XYZAxisTransform.TryGetComponent(out LineRenderer line)) Destroy(line); // 删除之前添加的高亮线
        }
    }
}
