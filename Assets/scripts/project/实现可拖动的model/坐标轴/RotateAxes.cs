using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateAxes : AxesBaseClass
{
    private Vector3 screenPosition;
    private Vector3 initialPosition;

    private Vector3 currenthitRayPoint;

    private Vector3 offset;

    private Vector3 temp = Vector3.zero;
    

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
            screenPosition = Camera.main.WorldToScreenPoint(initialPosition);
            // screenPosition = Input.mousePosition;
            // screenPosition = Camera.main.WorldToScreenPoint(initialPosition);

            // Debug.Log("screenPosition =="+ screenPosition);

            // offset = initialPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));

            // 获取点击了 哪个轴  然后 绘制直线
            Debug.Log("axisType =="+ axisType + "DragState =="+ DragState);
            
            DragState = DragStateType.mouseButtonDown;


            if(axisType == AxisType.X || axisType == AxisType.Y || axisType == AxisType.Z){
                // 画线
                Transform XYZAxisTransform = transform.Find(axisType.ToString());

                // Debug.Log("XYZAxisTransform =="+ XYZAxisTransform.name);
                LineRenderer line = XYZAxisTransform.AddComponent<LineRenderer>();
                line.positionCount = 3;
                line.material = XYZAxisTransform.GetComponent<Renderer>().material;
                
                line.SetPosition(0, currenthitRayPoint);
                line.SetPosition(1, initialPosition);
                line.SetPosition(2, currenthitRayPoint);

                line.startWidth = 0.02f;
                line.endWidth = 0.02f;
            }


            temp = Vector3.zero;




        }

        if(Input.GetMouseButton(0) && DragState == DragStateType.mouseButtonDown){
            // 准备拖动
            // DragState = DragStateType.isDraging;
            Debug.Log("准备拖动 DragState =="+ DragState);

            Vector3 currenthitRayScreenPoint = Camera.main.WorldToScreenPoint(currenthitRayPoint);
            Vector3 currentPosition  = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, currenthitRayScreenPoint.z)) ;
            Vector3 centerPositionToMousePositionVector = (currentPosition-initialPosition);
            Vector3 centerPositionToFirstPositionVector = (currenthitRayPoint-initialPosition);
            // Debug.Log("currentPosition =="+ currentPosition + "===currenthitRayPoint ==" + currenthitRayPoint + " === ");

            // 向量求夹角
            Transform XYZAxisTransform = transform.Find(axisType.ToString());
            if (XYZAxisTransform.TryGetComponent(out LineRenderer line)){

                Vector3 tempPos0 = Vector3.zero;
                Vector3 tempPos2 = Vector3.zero;
                float angle = 0.0f;
                Vector3 QuaternionEulerValue = Vector3.zero;
                Vector3 cross = Vector3.zero;
                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                switch (axisType)
                {

                    case AxisType.Z:
                        tempPos0 = Vector3.Project(centerPositionToMousePositionVector, transform.right) + Vector3.Project(centerPositionToMousePositionVector, transform.up);
                        tempPos2 = Vector3.Project(centerPositionToFirstPositionVector, transform.right) + Vector3.Project(centerPositionToFirstPositionVector, transform.up);
                        angle = Vector3.Angle(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos0));
                        // Vector3 cross = Vector3.Cross(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos));
                        // int angleSign = (int)Mathf.Sign(Vector3.Cross(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos)).y);
                        if((int)Mathf.Sign(Vector3.Cross(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos0)).z) < 0){
                            angle = -angle;
                        }
                        if(temp.z != angle){
                            currentAxisParent.transform.RotateAround(currentAxisParent.transform.position, transform.forward, angle - temp.z);
                            temp.z = angle;
                        }
                        QuaternionEulerValue = new Vector3(0, 0, angle);
                        break;
                    case AxisType.Y:
                        tempPos0 = Vector3.Project(centerPositionToMousePositionVector, transform.right) + Vector3.Project(centerPositionToMousePositionVector, transform.forward);
                        tempPos2 = Vector3.Project(centerPositionToFirstPositionVector, transform.right) + Vector3.Project(centerPositionToFirstPositionVector, transform.forward);
                        angle = Vector3.Angle(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos0));
                        cross = Vector3.Cross(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos0));
                        int angleSign = (int)Mathf.Sign(cross.y);
                        if(angleSign < 0){
                            angle = -angle;
                        }
                        if(temp.y != angle){
                            // currentAxisParent.transform.localEulerAngles = currentAxisParent.transform.localEulerAngles - new Vector3(0, temp.y - angle, 0);
                            currentAxisParent.transform.RotateAround(currentAxisParent.transform.position, transform.up, angle - temp.y);
                            temp.y = angle;
                        }
                        // rotation = Quaternion.Euler(0, angle, 0);
                        // QuaternionEulerValue = new Vector3();
                        // rotation = Quaternion.Euler(0, angle, 0);
                        QuaternionEulerValue = new Vector3(0, angle, 0);
                        break;
                    case AxisType.X:
                        tempPos0 = Vector3.Project(centerPositionToMousePositionVector, transform.forward) + Vector3.Project(centerPositionToMousePositionVector, transform.up);
                        tempPos2 = Vector3.Project(centerPositionToFirstPositionVector, transform.forward) + Vector3.Project(centerPositionToFirstPositionVector, transform.up);
                        angle = Vector3.Angle(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos0));
                        if((int)Mathf.Sign(Vector3.Cross(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos0)).x) < 0){
                            angle = -angle;
                        }
                        if(temp.x != angle){
                            currentAxisParent.transform.RotateAround(currentAxisParent.transform.position, transform.right, angle - temp.x);
                            temp.x = angle;
                        }
                        QuaternionEulerValue = new Vector3(angle, 0, 0);
                        break;
                }
                rotation = Quaternion.Euler(QuaternionEulerValue);
                // 旋转初始向量
                Vector3 vectorAngle = rotation * (currenthitRayPoint - initialPosition);
                // 输出结果
                line.SetPosition(2, vectorAngle + initialPosition);
                // Debug.Log("currentAxisParent.transform.localRotation =="+ currentAxisParent.transform.localEulerAngles);
                transform.rotation = Quaternion.identity;
            }
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



    protected override void Raycast()
    {
        base.Raycast();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;//   碰撞点 是 世界 坐标系
        bool res = Physics.Raycast(ray,out hit, 10000, 1<<arrowLayer);

        if(res){
            // Debug.Log("hit =="+ hit.transform.name);
            bool isXYZ = hit.transform.parent.TryGetComponent(out Axis axis);
            if(isXYZ){
                // Debug.Log("Axis  === "+ hit.transform.gameObject.GetComponent<Axis>());
                // Axis axis = hit.transform.gameObject.GetComponent<Axis>();
                // Debug.DrawLine(currentAxisParent.transform.position, hit.point, Color.red, 0.1f);
                currenthitRayPoint = hit.point;

                DragState = DragStateType.hover;
                axisType = axis.axisType;
                axis.setActive(true);
            }
        } else{
            axisType = AxisType.None;
            DragState = DragStateType.normal;

        }

    }


}
