using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class position{
    public float positionX;
    public float positionY;
    public float positionZ;
}
[Serializable]
public class scale{
    public float scaleX;
    public float scaleY;
    public float scaleZ;
}
[Serializable]
public class rotate{
    public float rotateX;
    public float rotateY;
    public float rotateZ;
}

[Serializable]
public class networkDeviceDataInfo{
    public long deviceCategory;
    public int isLabeled;
    public string deviceName;
    public string imei;
    public string modelType;
    public string modelTypeName;
    public string parentModelId;
    public position position;
    public scale scale;
    public rotate rotate;
}

[Serializable]
public class networkPageData {
    public int nextPage;
    public networkDeviceDataInfo[] pageData;
    public int totalCount;
}


public class networkDeviceListClass 
{
    public string code;
    public networkPageData data;
    public string errMsg;
    public string success;
}


