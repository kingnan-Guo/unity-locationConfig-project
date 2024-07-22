using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.getInstance().ShowPanel<panelFirst>("baseProject/UI/PanelFirst", E_UI_Layer.Middle, ( panel) => {
            Debug.Log("PanelFirst show");
            panel.initInfo();

            Invoke("DelayHide", 30);
        });
    }

    private void DelayHide(){
        UIManager.getInstance().HidePanel("baseProject/UI/PanelFirst");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
