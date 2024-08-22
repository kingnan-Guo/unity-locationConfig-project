using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createCubeWithXYZ : MonoBehaviour
{

    class XYZAxis {
        public GameObject x;
        public GameObject y;
        public GameObject z;
    }

    // Start is called before the first frame update
    void Start()
    {
        // GameObject cube = this.cubeCube("testCube", Color.gray);
        GameObject emptyCube =  createEmpty("testCube");
        // cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        emptyCube.transform.position = new Vector3(1, 2, 3);
        this.createXYZ(emptyCube);
        this.CreateXYZPlane(emptyCube);

        
    }

    // 创建 空对象
    GameObject createEmpty(string name) { 
        return new GameObject(name); 
    }



    GameObject cubeCube(string name, Color color){
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0, 0);
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.transform.rotation = Quaternion.Euler(0, 0, 0);
        cube.name = name;
        
        // 设置颜色
        cube.GetComponent<Renderer>().material.color = color;
        
        // 设置材质
        // cube.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
        // cube.GetComponent<Renderer>().material.color =color;
        
        // 设置纹理
        // Texture2D texture = new Texture2D(1, 1);
        // texture.SetPixel(0, 0, Color.red);
        // texture.Apply();
        // cube.GetComponent<Renderer>().material.mainTexture = texture;
        
        // // 设置碰撞器
        // cube.AddComponent<BoxCollider>();
        return cube;
        
        // // 设置刚体
        // Rigidbody rb = cube.AddComponent<Rigidbody>();
        // rb.useGravity = true;
        // rb.mass = 1;
    }


    GameObject createCylinder(string name, Color color){
        // 创建 圆柱
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = new Vector3(0, 0, 0);
        cylinder.transform.localScale = new Vector3(1, 1, 1);
        cylinder.transform.rotation = Quaternion.Euler(0, 0, 0);
        cylinder.name = name;
        
        // 设置颜色
        cylinder.GetComponent<Renderer>().material.color = color;
        cylinder.AddComponent<BoxCollider>();
        return null;
    }


    XYZAxis createXYZ(GameObject gameParent = null){
        Transform parentTransform = gameParent.transform;
        Vector3 parentPosition = gameParent.transform.position;
        // 创建X轴
        GameObject xAxis = this.cubeCube("xAxis", Color.red);
        
        // xAxis.transform.position = new Vector3(parentPosition.x + 2f, 0, 0);
        xAxis.transform.position = new Vector3(2f, 0, 0);
        xAxis.transform.localScale = new Vector3(4, 0.1f, 0.1f);
        xAxis.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        // 创建Y轴
        GameObject yAxis = this.cubeCube("yAxis", Color.green);
        yAxis.transform.position = new Vector3(0, 2f, 0);
        yAxis.transform.localScale = new Vector3(0.1f, 4, 0.1f);
        yAxis.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        // 创建Z轴
        GameObject zAxis = this.cubeCube("zAxis", Color.blue);
        zAxis.transform.position = new Vector3(0, 0, 2f);
        zAxis.transform.localScale = new Vector3(0.1f, 0.1f, 4);
        zAxis.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        // 设置父对象
        // GameObject parent = new GameObject("parent");

        xAxis.transform.parent = gameParent.transform;
        yAxis.transform.parent = gameParent.transform;
        zAxis.transform.parent = gameParent.transform;
        
        // 设置位置
        // xAxis.transform.localPosition = gameParent.transform.position + gameObject.transform.right * 2;
        // yAxis.transform.localPosition = gameParent.transform.position + gameObject.transform.up * 2;
        // zAxis.transform.localPosition = gameParent.transform.position + gameObject.transform.forward * 2;
        // xAxis.transform.localPosition = gameParent.transform.position;
        // yAxis.transform.localPosition = gameParent.transform.position;
        // zAxis.transform.localPosition = gameParent.transform.position;

        xAxis.transform.localPosition = gameObject.transform.right * 2;
        yAxis.transform.localPosition = gameObject.transform.up * 2;
        zAxis.transform.localPosition = gameObject.transform.forward * 2;


        
        // 设置旋转
        xAxis.transform.localRotation = gameParent.transform.rotation;
        yAxis.transform.localRotation = gameParent.transform.rotation;
        zAxis.transform.localRotation = gameParent.transform.rotation;
        XYZAxis XYZAxis = new XYZAxis();

        xAxis.tag = "xAxis";
        yAxis.tag = "yAxis";
        zAxis.tag = "zAxis";

        XYZAxis.x = xAxis;
        XYZAxis.y = yAxis;
        XYZAxis.z = zAxis;
        return XYZAxis;
    }

    GameObject createPlane(string name, Color color){
        // GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        GameObject Plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Plane.transform.position = new Vector3(0, 0, 0);
        Plane.transform.localScale = new Vector3(1, 0.001f, 1);
        Plane.transform.rotation = Quaternion.Euler(0, 0, 0);
        Plane.name = name;
        // 设置颜色
        Plane.GetComponent<Renderer>().material.color = color;
        return Plane;

    }


    public void CreateXYZPlane(GameObject gameParent = null){
        // 创建平面
        GameObject xyPlane = createPlane("xyPlane", Color.green);

        GameObject xzPlane = createPlane("xzPlane", Color.blue);

        GameObject yzPlane = createPlane("yzPlane", Color.red);

        
        xyPlane.transform.parent = gameParent.transform;
        xzPlane.transform.parent = gameParent.transform;
        yzPlane.transform.parent = gameParent.transform;

        xzPlane.transform.RotateAround(gameParent.transform.position, Vector3.right, 90);
        yzPlane.transform.RotateAround(gameParent.transform.position, Vector3.forward, 90);

        // xyPlane.transform.localPosition = Vector3.zero;
        // xzPlane.transform.localPosition = Vector3.zero;
        // yzPlane.transform.localPosition = Vector3.zero;
        xyPlane.transform.localPosition = Vector3.zero + (xyPlane.transform.right /2  ) + (xyPlane.transform.forward /2 );
        xzPlane.transform.localPosition = Vector3.zero + (xyPlane.transform.up /2 ) + (xyPlane.transform.right /2);// gameObject.transform.up * 2;
        yzPlane.transform.localPosition = Vector3.zero + (xyPlane.transform.up /2 ) + (xyPlane.transform.forward /2 );


        



    }

    void Update()
    {
        
    }
}
