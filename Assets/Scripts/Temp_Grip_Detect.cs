using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Grip_Detect : MonoBehaviour
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
        if(pl_move.grab_mode) {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        } else {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }
}
