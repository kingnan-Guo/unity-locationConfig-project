using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : SingletonAutoMono<GameMainManager>
{
    // Start is called before the first frame update
    void Start()
    {
        getGameObjectThroughMousePosition getGameObjectThroughMousePosition = new getGameObjectThroughMousePosition();
        MonoManager.getInstance().AddUpdateListener(getGameObjectThroughMousePosition.Update);

        mouseInputMgr.getInstance();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
