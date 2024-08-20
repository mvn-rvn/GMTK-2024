using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Reload_Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && GameObject.Find("Player").GetComponent<TM_Player_Movement>().move_enabled == true) {
            GameObject.Find("LevelLoad").GetComponent<Level_Loader>().Load(GameObject.Find("LevelLoad").GetComponent<Level_Loader>().level_counter);
        }
    }
}
