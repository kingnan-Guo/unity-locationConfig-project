using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAutoMonoDemo : SingletonAutoMono<SingletonAutoMonoDemo>
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SingletonAutoMonoDemo.GetInstance().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
