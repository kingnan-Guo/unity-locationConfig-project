using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPush : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("push", 1);
    }

    // Update is called once per frame
    void push()
    {
        // poolManager.getInstance().pushObj(this.gameObject.name, this.gameObject);

        poolManagerOptimize.getInstance().pushObj(this.gameObject.name, this.gameObject);
    }
}
