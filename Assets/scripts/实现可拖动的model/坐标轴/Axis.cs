using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour
{
    public AxisType axisType;
    // Start is called before the first frame update
    void Start()
    {
        switch (transform.name)
        {
            case "X":
                axisType = AxisType.X;
                break;
            case "Y": 
                axisType = AxisType.Y;
                break;
            case "Z":
                axisType = AxisType.Z;
                break;
            case "XY":
                axisType = AxisType.XY;
                break;
            case "XZ":
                axisType = AxisType.XZ;
                break;
            case "YZ":
                axisType = AxisType.YZ;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
