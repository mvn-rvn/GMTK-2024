using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Script : MonoBehaviour
{
    TM_Player_Movement pl_move;
    // Start is called before the first frame update
    void Start()
    {
        pl_move = GameObject.Find("Player").GetComponent<TM_Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            Destroy(pl_move);
            GameObject.Find("Player").AddComponent<TM_Player_Movement>();
        }
    }
}
