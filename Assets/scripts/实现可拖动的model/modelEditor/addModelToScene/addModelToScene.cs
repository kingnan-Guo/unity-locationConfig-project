using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addModelToScene : baseManager<addModelToScene>
{
    public addModelToScene()
    {
        MonoManager.getInstance().AddUpdateListener(Update);


        // EventCenterOptimize.getInstance().AddEventListener<string>("2dModelMoveTo3D", (name) => {
        //     Debug.Log("2dModelMoveTo3D ==" + name);
        // });
        EventCenter.getInstance().AddEventListener("2dModelMoveTo3D", (name) => {
            Debug.Log("2dModelMoveTo3D ==" + name);
        });

    }

    void Update()
    {
        
    }
}
