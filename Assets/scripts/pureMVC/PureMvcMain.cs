using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureMvcMain : MonoBehaviour
{
    // Start is called before the first frame update

    // bool isStart = true;
    void Start()
    {
        GameFacade.Instance.StartUp();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space)){
            GameFacade.Instance.showPanel();
            // isStart = !isStart;
        } else if(Input.GetKeyDown(KeyCode.N)){
            GameFacade.Instance.SendNotification(PureNotification.HIDE_PANEL, GameFacade.Instance.RetrieveMediator(newMainViewMediator.NAME));
        }
    }
}



