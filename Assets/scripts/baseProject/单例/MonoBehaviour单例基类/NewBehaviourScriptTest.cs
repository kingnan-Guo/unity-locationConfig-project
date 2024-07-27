using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScriptTest : singletonMono<NewBehaviourScriptTest>
{
    // Start is called before the first frame update

    protected  override void  Awake(){
        base.Awake();
    }

    void Start(){
        Debug.Log(NewBehaviourScriptTest.GetInstance().name);
    }
}
