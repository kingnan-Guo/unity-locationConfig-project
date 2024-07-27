using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newMainView : MonoBehaviour
{

    public Button btn;
    public Text txtName;
    public Text textLev;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateInfo(PlayerDataObj data){

        Debug.Log("updateInfo  = " +  data.playerName);
        txtName.text = data.playerName;
        textLev.text =  "level" + data.playerLevel.ToString();
    }
}
