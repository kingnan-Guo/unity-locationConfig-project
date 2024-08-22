using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MonoTest
{

    public MonoTest(){
        MonoManager.getInstance().StartCoroutine(TestIEnumerator());
    }

    IEnumerator TestIEnumerator(){
        yield return new WaitForSeconds(1);
        Debug.Log("MonoTest IEnumerator test");
    }

    public void Update(){
        // Debug.Log("MonoTest Update");
    }



}

public class MonoDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MonoTest monoTest = new MonoTest();
        MonoManager.getInstance().AddUpdateListener(monoTest.Update);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
