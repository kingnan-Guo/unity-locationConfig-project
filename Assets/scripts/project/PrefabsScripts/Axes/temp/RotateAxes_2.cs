using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RotateAxes_2 : AxesBaseClass
{
    private Vector3 screenPosition;
    private Vector3 initialPosition;

    private Vector3 currenthitRayPoint;

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

                Debug.Log("XYZAxisTransform =="+ XYZAxisTransform.name);
                LineRenderer line = XYZAxisTransform.gameObject.AddComponent<LineRenderer>();
                // LineRenderer line = XYZAxisTransform.AddComponent<LineRenderer>();
                line.positionCount = 3;
                // // line.material = XYZAxisTransform.GetComponent<MeshRenderer>().materials.FirstOrDefault(material => material.name.Contains("arrowShader"));
                line.material = XYZAxisTransform.GetComponent<Renderer>().material;
                
                // line.SetPosition(0, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z)));
                
                line.SetPosition(0, currenthitRayPoint);
                line.SetPosition(1, initialPosition);
                line.SetPosition(2, currenthitRayPoint);

                line.startWidth = 0.02f;
                line.endWidth = 0.02f;
            }





        }

        // if(Input.GetMouseButton(0) && DragState == DragStateType.mouseButtonDown){
        if(Input.GetMouseButton(0) && DragState == DragStateType.mouseButtonDown){
            // 准备拖动
            // DragState = DragStateType.isDraging;
            Debug.Log("准备拖动 DragState =="+ DragState);


            // Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
            // Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            // 这个 是 基础算法 计算 鼠标 滑动的 坐标差
            // Vector3 currentPosition  = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z)) + offset - initialPosition;


            // Vector3 currentAxisParentrealTimeScreentPosition  = Camera.main.WorldToScreenPoint(currentAxisParent.transform.position);

            Vector3 currenthitRayScreenPoint = Camera.main.WorldToScreenPoint(currenthitRayPoint);
            Vector3 currentPosition  = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, currenthitRayScreenPoint.z)) ;


            Debug.Log("currentPosition =="+ currentPosition + "===currenthitRayPoint ==" + currenthitRayPoint + " === ");

            // 向量求夹角
            Transform XYZAxisTransform = transform.Find(axisType.ToString());
            if (XYZAxisTransform.TryGetComponent(out LineRenderer line)){
                
                // line.SetPosition(2, Camera.main.ScreenToWorldPoint(Vector3.Project(aa, transform.right) + Vector3.Project(aa, transform.up)));
                
                // Debug.Log("currentPosition - currenthitRayPoint =="+ );
                
                // line.SetPosition(2, currentPosition);
                // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // RaycastHit hit;//   碰撞点 是 世界 坐标系
                // bool res = Physics.Raycast(ray,out hit, 10000, 1<<arrowLayer);
                // if(res){

                //     // hit.point;
                //     // bool isXYZ = hit.transform.parent.TryGetComponent(out Axis axis);
                //     // if(axis.axisType == axisType){
                //     //     line.SetPosition(2, hit.point);
                //     // }
                // }


                // Vector3 aa = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));

                Vector3 distance = new Vector3(currentPosition.x - currenthitRayPoint.x, currentPosition.y - currenthitRayPoint.y, currentPosition.z - currenthitRayPoint.z);
                // Debug.Log("currentPosition - initialMousePosition ===" +  currentPosition + "==" + initialPosition + " ===" + transform.position);
                Vector3 tempPos = Vector3.zero;
                Vector3 tempPos2 = Vector3.zero;
                Vector3 original = Vector3.up; // 假设原始向量为单位向量(0, 0, 1)

                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                switch (axisType)
                {

                case AxisType.Z:
                    // tempPos = Vector3.Project(currentPosition, transform.right) + Vector3.Project(currentPosition, transform.up);
                    
                    // original = Vector3.right;
                    rotation = Quaternion.Euler(0, 0, 30);
                    // tempPos = Vector3.Project(distance, transform.right) + Vector3.Project(distance, transform.up);
                    break;
                case AxisType.Y:
                    // original = Vector3.forward;
                    
                    // tempPos = Vector3.Project(distance, transform.right) + Vector3.Project(distance, transform.forward);
                    
                    tempPos = Vector3.Project((currentPosition-initialPosition), transform.right) + Vector3.Project((currentPosition-initialPosition), transform.forward);

                    tempPos2 = Vector3.Project((currenthitRayPoint-initialPosition), transform.right) + Vector3.Project((currenthitRayPoint-initialPosition), transform.forward);

                    // Debug.Log("(currenthitRayPoint - initialPosition) =="+ (currenthitRayPoint - initialPosition) + "== (currentPosition-initialPosition) =="+ (currentPosition - initialPosition));
                    // Debug.Log("tempPos: " + tempPos +" == tempPos2: " + tempPos2);


                    float angle = Vector3.Angle(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos));

                    Vector3 cross = Vector3.Cross(Vector3.Normalize(tempPos2), Vector3.Normalize(tempPos));
                    // Debug.Log("cross: " + cross.y);
                    int angleSign = (int)Mathf.Sign(cross.y);
                    // Debug.Log("angle: " + angle);
                    if(angleSign < 0){
                        angle = -angle;
                    }
                    rotation = Quaternion.Euler(0, angle, 0);
                    // tempPos = Vector3.Project(tempPos, transform.up) + Vector3.Project(tempPos, transform.forward);
                    break;
                case AxisType.X:

                    // original = Vector3.up;
                    rotation = Quaternion.Euler(30, 0, 0);
                    // tempPos = Vector3.Project(currentPosition, transform.up) +  Vector3.Project(currentPosition, transform.forward);
                    break;

                }
                // Debug.Log("tempPos ===" + tempPos);


                Debug.Log("currenthitRayPoint - initialPosition  ==="+ Vector3.Normalize((currenthitRayPoint - initialPosition)));

                // line.SetPosition(1, tempPos);
                

                
                // float angle = 30f; // 想要旋转到的角度
                // Vector3 target = Vector3.RotateTowards((currenthitRayPoint - initialPosition), original, angle * Mathf.Deg2Rad, 0.0f);

                // Debug.Log("向量旋转到30度角: " + target);

                // line.SetPosition(2, target + initialPosition);
                // line.SetPosition(2, (currenthitRayPoint - initialPosition));


                
                // 旋转初始向量
                Vector3 vectorAngle = rotation * (currenthitRayPoint - initialPosition);
        
                // 输出结果
                // Debug.Log("初始向量: " + initialVector);

                line.SetPosition(2, vectorAngle + initialPosition);
                

                // transform.rotation = Quaternion.identity;
                // editTarget.transform.Rotate(-angle, 0, 0, Space.World);
                // currentAxisParent.transform.position = initialPosition + tempPos;






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

            Debug.Log("hit =="+ hit.transform.name);

            bool isXYZ = hit.transform.parent.TryGetComponent(out Axis axis);
            if(isXYZ){
                // Debug.Log("Axis  === "+ hit.transform.gameObject.GetComponent<Axis>());
                // Axis axis = hit.transform.gameObject.GetComponent<Axis>();
                // Debug.DrawLine(currentAxisParent.transform.position, hit.point, Color.red, 0.1f);
                currenthitRayPoint = hit.point;

                // Debug.Log("hit.point =="+ hit.point);
                // Debug.Log("axis =="+ axis.name);
                DragState = DragStateType.hover;
                axisType = axis.axisType;
                // axis.isActive = true;
                axis.setActive(true);
            }
        } else{
            axisType = AxisType.None;
            DragState = DragStateType.normal;

        }

    }
}
